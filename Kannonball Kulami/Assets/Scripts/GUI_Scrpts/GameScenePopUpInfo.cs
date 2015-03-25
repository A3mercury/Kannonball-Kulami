using UnityEngine;
using System.Collections;

public class GameScenePopUpInfo : MonoBehaviour {

	private bool doGameStartInfo = false;
	// Use this for initialization
	void Start () {
		OnLevelWasLoaded ();
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
		yield return new WaitForSeconds(3);

		doGameStartInfo = true;
	}

	IEnumerator endGameStartInfo ()
	{
		yield return new WaitForSeconds (10);

		doGameStartInfo = false;
	}

	void DoWindow0(int windowID) {
		
	}

	void OnGUI ()
	{
		if (doGameStartInfo)
			GUI.Window(0, new Rect(500, 10, 200, 80), DoWindow0, "You go first this game. Place a piece anywhere on the board to begin.");	

		StartCoroutine (endGameStartInfo ());
	}
}
