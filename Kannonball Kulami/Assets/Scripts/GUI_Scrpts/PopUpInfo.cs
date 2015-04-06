using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpInfo : MonoBehaviour {

	private bool doWindowSinglePlayer = false;
	private bool doWindowMultiPlayer = false;
	private bool doWindowOptions = false;
	private bool doWindowCredits = false;
	private Vector3 mousePos;
	public Toggle assistanceCheck;
	
	//private Texture2D boxTexture;
	public Texture2D onHoverImage;
	public GUISkin onHoverSkin;

	void Start () {

	}
	
	void Update () {
		mousePos.x = (Input.mousePosition.x) - 300;
		mousePos.y = -(Input.mousePosition.y - Screen.height);
		mousePos.z = (0);
	}

	void OnMouseEnter () {
        if (assistanceCheck.isOn)
        {
            if (gameObject.name.ToString() == "singleplayer") ;
            doWindowSinglePlayer = true;

            if (gameObject.name.ToString() == "networkplay")
                doWindowMultiPlayer = true;

            if (gameObject.name.ToString() == "options")
                doWindowOptions = true;

            if (gameObject.name.ToString() == "credits")
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

    void OnGUI()
    {
        GUI.skin = onHoverSkin;
        GUI.skin.window.normal.background = onHoverImage;

        if (doWindowSinglePlayer)
            GUI.Window(0, new Rect(mousePos.x + 20, mousePos.y - 47, 200, 95), DoWindow0, "This takes you to a single player game, where you can either play against an easy or hard AI.");

        if (doWindowMultiPlayer)
            GUI.Window(0, new Rect(mousePos.x + 20, mousePos.y - 45, 150, 110), DoWindow0, "This takes you to a multi-player game, where you can play against other people.");

        if (doWindowOptions)
            GUI.Window(0, new Rect(mousePos.x + 20, mousePos.y - 20, 200, 60), DoWindow0, "Don't press this. Really. Don't");

        if (doWindowCredits)
            GUI.Window(0, new Rect(mousePos.x + 20, mousePos.y - 45, 150, 110), DoWindow0, "This takes you to the credits screen, where you can see who made this game!");

    }
}
