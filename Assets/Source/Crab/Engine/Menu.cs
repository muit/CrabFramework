using UnityEngine;

public class Menu : MonoBehaviour {

	public void Play(int i){
		Application.LoadLevel(i);
	}

	public void Pause(bool value)
	{
		Time.timeScale = value ? 0 : 1;
		AudioListener.pause = value;
	}

	public void Quit()
	{
		Application.Quit();
	}
}
