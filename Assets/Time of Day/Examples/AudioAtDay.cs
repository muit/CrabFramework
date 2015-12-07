using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAtDay : MonoBehaviour
{
	public TOD_Sky sky;

	public  float fadeTime = 1;
	private float lerpTime = 0;

	private AudioSource audioComponent;
	private float audioVolume;

	protected void Start()
	{
		if (!sky) sky = TOD_Sky.Instance;

		audioComponent = GetComponent<AudioSource>();
		audioVolume    = audioComponent.volume;

		if (!sky.IsDay) audioComponent.volume = 0;
	}

	protected void Update()
	{
		int sign = (sky.IsDay) ? +1 : -1;
		lerpTime = Mathf.Clamp01(lerpTime + sign * Time.deltaTime / fadeTime);

		audioComponent.volume = Mathf.Lerp(0, audioVolume, lerpTime);
	}
}
