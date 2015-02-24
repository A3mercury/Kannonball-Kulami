using UnityEngine;
using System.Collections;

public class SceneTransitionScript : MonoBehaviour {

	public void SinglePlayer () {
        GameCore.isOnline = false;
		Application.LoadLevel("gameboardTestScene");
	}
	
	public void NetworkPlay () {
        GameCore.isOnline = true;
		Application.LoadLevel("gameboardTestScene");
	}

	public void Options () {
		Application.LoadLevel ("OptionsScene");
	}

	public void Credits () {
		Application.LoadLevel("CreditsScene");
	}	

	public void MainMenu () {
		Application.LoadLevel("MainMenuScene");
	}

	public void OnMouseDown () {
		if (gameObject.name.ToString () == "MenuCube") 
		{
			MainMenu ();
		}
	}
}
