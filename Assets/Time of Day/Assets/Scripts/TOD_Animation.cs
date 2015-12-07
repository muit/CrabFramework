using UnityEngine;

/// Cloud animation class.
///
/// Component of the sky dome parent game object.

public class TOD_Animation : MonoBehaviour
{
	/// Wind direction in degrees.
	/// 0 for wind blowing in northern direction.
	/// 90 for wind blowing in eastern direction.
	/// 180 for wind blowing in southern direction.
	/// 270 for wind blowing in western direction.
	[Tooltip("Wind direction in degrees.")]
	public float WindDegrees = 0.0f;

	/// Speed of the wind that is acting on the clouds.
	[Tooltip("Speed of the wind that is acting on the clouds.")]
	public float WindSpeed = 1.0f;

	/// Adjust the cloud coordinates when the sky dome moves.
	[Tooltip("Adjust the cloud coordinates when the sky dome moves.")]
	public bool WorldSpaceCloudUV = true;

	/// Randomize the cloud coordiantes at startup.
	[Tooltip("Randomize the cloud coordinates at startup.")]
	public bool RandomInitialCloudUV = true;

	/// Current cloud UV coordinates.
	/// Can be synchronized between multiple game clients to guarantee identical cloud positions.
	internal Vector4 CloudUV
	{
		get; set;
	}

	/// Current offset UV coordinates.
	/// Is being calculated from the sky dome world position.
	internal Vector4 OffsetUV
	{
		get
		{
			if (!WorldSpaceCloudUV) return Vector4.zero;

			Vector3 pos = transform.position;
			Vector3 scale = transform.lossyScale;
			Vector3 offset = new Vector3(pos.x / scale.x, 0, pos.z / scale.z);
			offset = Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * offset;
			return new Vector4(offset.x, offset.z, offset.x, offset.z);
		}
	}

	private TOD_Sky sky;

	private void AddUV(Vector4 uv)
	{
		CloudUV += uv;
		CloudUV = new Vector4(CloudUV.x % sky.Clouds.Scale1.x,
		                      CloudUV.y % sky.Clouds.Scale1.y,
		                      CloudUV.z % sky.Clouds.Scale2.x,
		                      CloudUV.w % sky.Clouds.Scale2.y);
	}

	protected void Start()
	{
		sky = GetComponent<TOD_Sky>();

		if (RandomInitialCloudUV)
		{
			AddUV(new Vector4(Random.value, Random.value, Random.value, Random.value) * 1000);
		}
	}

	protected void Update()
	{
		// Wind direction and speed calculation
		Vector2 v1 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (WindDegrees + 15)),
		                         Mathf.Sin(Mathf.Deg2Rad * (WindDegrees + 15)));
		Vector2 v2 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (WindDegrees - 15)),
		                         Mathf.Sin(Mathf.Deg2Rad * (WindDegrees - 15)));
		Vector4 wind = WindSpeed / 100f * new Vector4(v1.x, v1.y, v2.x, v2.y);

		// Update cloud UV coordinates
		AddUV(Time.deltaTime * wind);

		// Rotate billboards
		sky.Components.Billboards.transform.Rotate(0, Time.deltaTime * WindSpeed / 10, 0);
	}
}
