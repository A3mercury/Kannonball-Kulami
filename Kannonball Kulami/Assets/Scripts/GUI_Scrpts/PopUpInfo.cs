using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpInfo : MonoBehaviour {

    private GameCore gameCore;

	private bool doWindowSinglePlayer = false;
	private bool doWindowMultiPlayer = false;
	private bool doWindowBottle = false;
	private bool doWindowCredits = false;
    private bool doWindowPlayerMoves = false;
    private bool doWindowOpponentMoves = false;
    private bool doWindowChestInfo = false;
	private Vector3 mousePos;
	public Toggle assistanceCheck;
	
	//private Texture2D boxTexture;
	public Texture2D onHoverImage;
	public GUISkin onHoverSkin;

	void Start () {
    }
	
	void Update () {
        if (gameCore == null && GameObject.Find("GameCore") != null)
        {
            gameCore = GameObject.Find("GameCore").GetComponent<GameCore>() as GameCore;
        }
		mousePos.x = (Input.mousePosition.x) - 300;
		mousePos.y = -(Input.mousePosition.y - Screen.height);
		mousePos.z = (0);
	}

	void OnMouseEnter () {
        if (assistanceCheck.isOn)
        {
            if (gameObject.name.ToString() == "singleplayer")
            doWindowSinglePlayer = true;

            if (gameObject.name.ToString() == "networkplay")
                doWindowMultiPlayer = true;

            if (gameObject.name.ToString() == "how_to_play_bottle")
                doWindowBottle = true;

            if (gameObject.name.ToString() == "credits")
                doWindowCredits = true;

            if (gameObject.name.ToString() == "PlayerMovesRemaining")
                doWindowPlayerMoves = true;

            if (gameObject.name.ToString() == "OpponentMovesRemaining" && !Chat_Script.ChatOpen)
                doWindowOpponentMoves = true;

            if(gameObject.name.ToString() == "New Chest")
                doWindowChestInfo = true;

            Debug.Log(gameObject.name.ToString());
        }
	}

	void OnMouseExit () {
		doWindowSinglePlayer = false;
		doWindowMultiPlayer = false;
        doWindowBottle = false;
		doWindowCredits = false;
        doWindowPlayerMoves = false;
        doWindowOpponentMoves = false;
        doWindowChestInfo = false;
	}
	
	void DoWindow0(int windowID) {

	}

    void OnGUI()
    {
        GUI.skin = onHoverSkin;
        GUI.skin.window.normal.background = onHoverImage;

        if (doWindowSinglePlayer)
            GUI.Window(0, new Rect(48, 105, 200, 95), DoWindow0, "Test your skills against an easy, hard, or expert computer!");

        if (doWindowMultiPlayer)
            GUI.Window(0, new Rect(70, 255, 150, 80), DoWindow0, "Battle against your friends online!");

        if (doWindowBottle)
            GUI.Window(0, new Rect(48, 415, 200, 60), DoWindow0, "Learn how to play here!");

        if (doWindowCredits)
           // GUI.Window(0, new Rect(25, 370, 150, 112), DoWindow0, "This takes you to the credits screen, where you can see who made this game!");
            GUI.Window(0, new Rect(70, 560, 150, 80), DoWindow0, "Disabled For Now To Ensure Fair Testing Evaluations");

        if (doWindowPlayerMoves)
            GUI.Window(25, new Rect(945, 640, 175, 60), DoWindow0, "Moves remaining: " + (gameCore.blackTurnsLeft));
		
        if (doWindowOpponentMoves)
			GUI.Window(25, new Rect(200, 400, 175, 60), DoWindow0, "Moves remaining: " + (gameCore.redTurnsLeft));

        if (doWindowChestInfo)
            GUI.Window(25, new Rect(900, 135, 175, 60), DoWindow0, "Arrrrggg, here be treasure!");
    }
}
