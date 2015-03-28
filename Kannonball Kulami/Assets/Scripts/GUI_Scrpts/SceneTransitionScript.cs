using UnityEngine;
using System.Collections;

public class SceneTransitionScript : MonoBehaviour {

	static bool isClickable = true;
	public GameObject optionsPanel;
	OptionsMenuTT optionsScript;

	void Start () {
		optionsScript = GameObject.FindObjectOfType (typeof(OptionsMenuTT)) as OptionsMenuTT;
	}

	public void ToggleClickability () {
		if (isClickable == true) 
		{
			isClickable = false;
		} else 
		{
			isClickable = true;
		}
	}

	public void SinglePlayer () {
        Network_Manager.fromtransition = false;
		Application.LoadLevel("GameScene");
	}
	
	public void NetworkPlay () {
        Network_Manager.fromtransition = true;
		Application.LoadLevel("GameScene");
	}

	public void Credits () {
		Application.LoadLevel("CreditsScene");
	}	

	public void MainMenu () {
		Application.LoadLevel("MainMenuScene");
	}

    public void ExitGame()
    {
        Application.Quit();
    }

	IEnumerator delayedSinglePlayer ()
	{
		yield return new WaitForSeconds(1);
		SinglePlayer ();
	}

	IEnumerator delayedNetworkPlay ()
	{
		yield return new WaitForSeconds(1);
		NetworkPlay ();
	}

	IEnumerator delayedCredits ()
	{
		yield return new WaitForSeconds(1);
		Credits ();
	}

    public void OnMouseDown()
    {
        Debug.Log("isClickable " + isClickable);
		if (isClickable) 
		{
				if (gameObject.name.ToString () == "singleplayer") {
					StartCoroutine(delayedSinglePlayer());
				}

				if (gameObject.name.ToString () == "networkplay") {
					StartCoroutine(delayedNetworkPlay());
				}

				if (gameObject.name.ToString () == "credits") {
					StartCoroutine(delayedCredits());
				}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float depth;
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo))
			{
				gameObject.rigidbody.AddForceAtPosition(new Vector3(0f, -100f, 0f), new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z));
			}

		}
    }

	void OnGUI () 
	{
		if (Application.loadedLevelName == "MainMenuScene" || Application.loadedLevelName == "GameScene") {
			if (GUI.Button (new Rect (3, 570, 75, 50), "Options")) {
				if (optionsPanel.activeSelf == true)
				{
					optionsPanel.SetActive (false);
					isClickable = true;
					optionsScript.ToggleClickScript();
				}
				else
				{
					optionsPanel.SetActive (true);
					isClickable = false;
					optionsScript.ToggleClickScript();
				}
			}
		}
	}
}
