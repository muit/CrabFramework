using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightAtNight : MonoBehaviour
{
	public TOD_Sky sky;

	public  float fadeTime = 1;
	private float lerpTime = 0;

	private Light lightComponent;
	private float lightIntensity;

	protected void Start()
	{
		if (!sky) sky = TOD_Sky.Instance;

		lightComponent = GetComponent<Light>();
		lightIntensity = lightComponent.intensity;
	}

	protected void Update()
	{
		int sign = (sky.IsNight) ? +1 : -1;
		lerpTime = Mathf.Clamp01(lerpTime + sign * Time.deltaTime / fadeTime);

		lightComponent.intensity = Mathf.Lerp(0, lightIntensity, lerpTime);
		lightComponent.enabled   = (lightComponent.intensity > 0);
	}
}
