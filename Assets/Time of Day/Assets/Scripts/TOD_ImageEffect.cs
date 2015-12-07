using UnityEngine;

/// Image effect base class.
///
/// Based on PostEffectsBase from the Unity Standard Assets.
/// Extended for image effects that depend on a TOD_Sky reference.

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public abstract class TOD_ImageEffect : MonoBehaviour
{
	/// Sky dome reference inspector variable.
	/// Will automatically be searched in the scene if not set in the inspector.
	public TOD_Sky sky = null;

	protected Camera cam = null;

	protected Material CreateMaterial(Shader shader)
	{
		if (!shader)
		{
			Debug.Log("Missing shader in " + this.ToString());
			enabled = false;
			return null;
		}

		if (!shader.isSupported)
		{
			Debug.LogError("The shader " + shader.ToString() + " on effect " + this.ToString() + " is not supported on this platform!");
			enabled = false;
			return null;
		}

		var material = new Material(shader);
		material.hideFlags = HideFlags.DontSave;

		return material;
	}

	protected void Awake()
	{
		if (!cam) cam = GetComponent<Camera>();
		if (!sky) sky = FindObjectOfType(typeof(TOD_Sky)) as TOD_Sky;
	}

	protected bool CheckSupport(bool needDepth = false, bool needHdr = false)
	{
		if (!cam) return false;

		if (!sky || !sky.Initialized) return false;

		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
			enabled = false;
			return false;
		}

		if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it requires a depth texture.");
			enabled = false;
			return false;
		}

		if (needHdr && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it requires HDR.");
			enabled = false;
			return false;
		}

		if (needDepth)
		{
			cam.depthTextureMode |= DepthTextureMode.Depth;
		}

		if (needHdr)
		{
			cam.hdr = true;
		}

		return true;
	}

	protected void DrawBorder(RenderTexture dest, Material material)
	{
		float x1;
		float x2;
		float y1;
		float y2;

		RenderTexture.active = dest;
		bool invertY = true; // source.texelSize.y < 0.0f;

		// Set up the simple Matrix
		GL.PushMatrix();
		GL.LoadOrtho();

		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);

			float y1_;
			float y2_;
			if (invertY)
			{
				y1_ = 1.0f; y2_ = 0.0f;
			}
			else
			{
				y1_ = 0.0f; y2_ = 1.0f;
			}

			// Left
			x1 = 0.0f;
			x2 = 0.0f + 1.0f / (dest.width*1.0f);
			y1 = 0.0f;
			y2 = 1.0f;
			GL.Begin(GL.QUADS);

			GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
			GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
			GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
			GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

			// Right
			x1 = 1.0f - 1.0f / (dest.width*1.0f);
			x2 = 1.0f;
			y1 = 0.0f;
			y2 = 1.0f;

			GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
			GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
			GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
			GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

			// Top
			x1 = 0.0f;
			x2 = 1.0f;
			y1 = 0.0f;
			y2 = 0.0f + 1.0f / (dest.height*1.0f);

			GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
			GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
			GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
			GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

			// Bottom
			x1 = 0.0f;
			x2 = 1.0f;
			y1 = 1.0f - 1.0f / (dest.height*1.0f);
			y2 = 1.0f;

			GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
			GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
			GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
			GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

			GL.End();
		}

		GL.PopMatrix();
	}

	protected void CustomBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr = 0)
	{
		RenderTexture.active = dest;

		fxMaterial.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		fxMaterial.SetPass(passNr);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.Vertex3(1.0f, 0.0f, 2.0f); // BR

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

		GL.End();
		GL.PopMatrix();
	}
}
