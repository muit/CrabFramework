using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public void Play(int i){
		Application.LoadLevel(i);
	}
}
