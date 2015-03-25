using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpInfo : MonoBehaviour {

	private bool doWindowSinglePlayer = false;
	private bool doWindowMultiPlayer = false;
	private bool doWindowOptions = false;
	private bool doWindowCredits = false;
	public Toggle assistanceCheck;

	void OnMouseEnter () {
		if (assistanceCheck.isOn) 
		{
			if (gameObject.name.ToString () == "singleplayer")
					doWindowSinglePlayer = true;

			if (gameObject.name.ToString () == "networkplay")
					doWindowMultiPlayer = true;

			if (gameObject.name.ToString () == "options")
					doWindowOptions = true;

			if (gameObject.name.ToString () == "credits")
					doWindowCredits = true;
		}
	}

	void OnMouseExit () {
		doWindowSinglePlayer = false;
		doWindowMultiPlayer = false;
		doWindowOptions = false;
		doWindowCredits = false;
	}
	
	void DoWindow0(int windowID) {

	}

	void OnGUI() {
		GUI.skin.window.wordWrap = true;
		GUI.skin.window.alignment = TextAnchor.MiddleCenter;
		GUI.skin.window.padding.bottom = 0;

		if (doWindowSinglePlayer)
			GUI.Window(0, new Rect(325, 20, 200, 95), DoWindow0, "This takes you to a single player game, where you can either play against an easy or hard AI.");	

		if (doWindowMultiPlayer)
			GUI.Window(0, new Rect(20, 120, 150, 90), DoWindow0, "This takes you to a multi-player game, where you can play against other people.");

		if (doWindowOptions)
			GUI.Window(0, new Rect(325, 180, 200, 40), DoWindow0, "Don't press this. Really. Don't.");

		if (doWindowCredits)
			GUI.Window(0, new Rect(20, 240, 150, 90), DoWindow0, "This takes you to the credits screen, where you can see who made this game!");

	}
}
