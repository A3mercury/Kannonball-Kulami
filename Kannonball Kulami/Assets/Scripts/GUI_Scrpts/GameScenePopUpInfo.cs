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
		yield return new WaitForSeconds(2);

		doGameStartInfoRed = true;
	}

	IEnumerator endGameStartInfo ()
	{
		yield return new WaitForSeconds (10);

		doGameStartInfoRed = false;
	}

	void DoWindow0(int windowID) {
		
	}

	void OnGUI ()
	{
		GUI.skin = onHoverSkin;
		GUI.skin.window.normal.background = onHoverImage;

		if (assistanceToggle.isOn && !network.isOnline) 
		{
			if (doGameStartInfoRed)
			{
				GUI.Window (0, new Rect (760, 10, 200, 80), DoWindow0, "You go first this game. Place a piece anywhere on the board to begin.");	
				StartCoroutine (endGameStartInfo ());
			}
		}

		if (assistanceToggle.isOn && network.isOnline && network.ingame && gameCore.turn == gameCore.playerColor) 
		{
			if (doGameStartInfoRed)
			{
				GUI.Window (0, new Rect (760, 10, 200, 80), DoWindow0, "You go first this game. Place a piece anywhere on the board to begin.");
				StartCoroutine (endGameStartInfo ());
			}
		}

		if (assistanceToggle.isOn && network.isOnline && network.ingame && gameCore.turn != gameCore.playerColor) 
		{
			if (doGameStartInfoRed)
			{
				GUI.Window (0, new Rect (760, 10, 200, 80), DoWindow0, "You go second this game. Wait for the other player to place a piece.");
				StartCoroutine (endGameStartInfo ());
			}
		}
	}
}