using UnityEngine;

/// Sky dome management camera component.
///
/// Move and scale the sky dome every frame after the rest of the scene has fully updated.

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Time of Day/Camera Main Script")]
public class TOD_Camera : MonoBehaviour
{
	/// Sky dome reference inspector variable.
	/// Will automatically be searched in the scene if not set in the inspector.
	public TOD_Sky sky;

	/// Automatically move the sky dome to the camera position in OnPreCull().
	public bool DomePosToCamera = true;

	/// The sky dome position offset relative to the camera.
	public Vector3 DomePosOffset = Vector3.zero;

	/// Automatically scale the sky dome to the camera far clip plane in OnPreCull().
	public bool DomeScaleToFarClip = true;

	/// The sky dome scale factor relative to the camera far clip plane.
	public float DomeScaleFactor = 0.95f;

	internal bool HDR
	{
		get { return cameraComponent ? cameraComponent.hdr : false; }
	}

	private Camera cameraComponent = null;
	private Transform cameraTransform = null;

	protected void OnValidate()
	{
		DomeScaleFactor = Mathf.Clamp(DomeScaleFactor, 0.01f, 1.0f);
	}

	protected void OnEnable()
	{
		cameraComponent = GetComponent<Camera>();
		cameraTransform = GetComponent<Transform>();

		if (!sky) sky = FindObjectOfType(typeof(TOD_Sky)) as TOD_Sky;
	}

	protected void Update()
	{
		if (!sky) sky = FindObjectOfType(typeof(TOD_Sky)) as TOD_Sky;

		sky.Components.Camera = this;

	}

	protected void OnPreCull()
	{
		if (DomeScaleToFarClip) DoDomeScaleToFarClip();

		if (DomePosToCamera) DoDomePosToCamera();
	}

	public void DoDomeScaleToFarClip()
	{
		if (!sky || !sky.Initialized) return;

		float size = DomeScaleFactor * cameraComponent.farClipPlane;
		var localScale = new Vector3(size, size, size);

		#if UNITY_EDITOR
		if (sky.Components.DomeTransform.localScale != localScale)
		#endif
		{
			sky.Components.DomeTransform.localScale = localScale;
		}
	}

	public void DoDomePosToCamera()
	{
		if (!sky || !sky.Initialized) return;

		var position = cameraTransform.position + cameraTransform.rotation * DomePosOffset;

		#if UNITY_EDITOR
		if (sky.Components.DomeTransform.position != position)
		#endif
		{
			sky.Components.DomeTransform.position = position;
		}
	}
}
