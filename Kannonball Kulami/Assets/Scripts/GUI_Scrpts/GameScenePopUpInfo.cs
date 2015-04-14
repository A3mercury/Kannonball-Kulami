using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScenePopUpInfo : MonoBehaviour {

	public Toggle assistanceToggle;
	public Texture2D onHoverImage;
	public GUISkin onHoverSkin;

	private GameCore gameCore;
    private Network_Manager network;

	private bool doGameStartInfoRed = false;
    private bool PlaySecondFirstMove = false;

	// Use this for initialization
	void Start () {
		OnLevelWasLoaded ();
		gameCore = FindObjectOfType (typeof(GameCore)) as GameCore;
		network = FindObjectOfType (typeof(Network_Manager)) as Network_Manager;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLevelWasLoaded ()
	{
		StartCoroutine (gameStartInfo ());
	}

	IEnumerator gameStartInfo ()
	{
		yield return new WaitForSeconds(0);

		doGameStartInfoRed = true;
	}

	IEnumerator endGameStartInfo ()
	{
        if (OptionsMenuTT.PlayerGoesFirst)
        {
            if (gameCore.blackTurnsLeft == 28)
            {
                yield return false;
            }
            else
            {
                yield return new WaitForSeconds(0);

                doGameStartInfoRed = false;
            }
        }
        else
        {
            if (gameCore.redTurnsLeft == 28)
            {
                yield return false;
            }
            else
            {
                yield return new WaitForSeconds(0);

                doGameStartInfoRed = false;
                PlaySecondFirstMove = true;
            }
        }
	}
    IEnumerator endSecondStartInfo()
    {
        if (gameCore.blackTurnsLeft == 28)
        {
            yield return false;
        }
        else
        {
            yield return new WaitForSeconds(0);
            PlaySecondFirstMove = false;
        }
    }

	void DoWindow0(int windowID) {
		
	}

	void OnGUI ()
	{
		GUI.skin = onHoverSkin;
		GUI.skin.window.normal.background = onHoverImage;

        if (assistanceToggle.isOn && !network.isOnline) 
		{
            if (doGameStartInfoRed && GameObject.Find("OptionsPanel") == null)
			{
				GUI.Window (0, new Rect (603, 325, 200, 80), DoWindow0, "You go first this game. Place a piece anywhere on the board to begin.");	
				StartCoroutine (endGameStartInfo ());
			}
		}

        if (assistanceToggle.isOn && !network.isOnline && !OptionsMenuTT.PlayerGoesFirst)
        {
            if (doGameStartInfoRed && GameObject.Find("OptionsPanel") == null)
            {
                GUI.Window(0, new Rect(603, 325, 200, 80), DoWindow0, "You go second this game. Wait for the other player to place a piece.");
                StartCoroutine(endGameStartInfo());
            }
            else if (PlaySecondFirstMove && GameObject.Find("OptionsPanel") == null)
            {
                GUI.Window(784, new Rect(603, 325, 200, 100), DoWindow0, "You may now place a piece on any of the available moves (highlighted in white).");
                StartCoroutine(endSecondStartInfo());
            }
        }

		if (assistanceToggle.isOn && network.isOnline && network.ingame && OptionsMenuTT.PlayerGoesFirst) 
		{
            if (doGameStartInfoRed && GameObject.Find("OptionsPanel") == null && !network.popUpOpen)
			{
				GUI.Window (0, new Rect (603, 325, 200, 80), DoWindow0, "You go first this game. Place a piece anywhere on the board to begin.");
				StartCoroutine (endGameStartInfo ());
			}
		}

		if (assistanceToggle.isOn && network.isOnline && network.ingame && !OptionsMenuTT.PlayerGoesFirst) 
		{
            if (doGameStartInfoRed && GameObject.Find("OptionsPanel") == null && !network.popUpOpen)
			{
				GUI.Window (0, new Rect (603, 325, 200, 80), DoWindow0, "You go second this game. Wait for the other player to place a piece.");
				StartCoroutine (endGameStartInfo ());
			}
            else if (PlaySecondFirstMove && GameObject.Find("OptionsPanel") == null)
            {
                GUI.Window(784, new Rect(603, 325, 200, 100), DoWindow0, "You may now place a piece on any of the available moves (highlighted in white).");
                StartCoroutine(endSecondStartInfo());
            }
		}
	}
}