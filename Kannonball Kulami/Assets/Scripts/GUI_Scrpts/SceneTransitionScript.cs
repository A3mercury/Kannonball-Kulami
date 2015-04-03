using UnityEngine;
using System.Collections;

public class SceneTransitionScript : MonoBehaviour {

	public static bool isClickable = true;
	GameObject optionsPanel;
	OptionsMenuTT optionsScript;

	void Start () {
		optionsScript = GameObject.FindObjectOfType (typeof(OptionsMenuTT)) as OptionsMenuTT;
		optionsPanel = GameObject.Find ("OptionsPanel");
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
		Application.LoadLevel ("GameScene");
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
				SinglePlayer();
			}

			if (gameObject.name.ToString () == "networkplay") {
				StartCoroutine(delayedNetworkPlay());
			}

			if (gameObject.name.ToString () == "credits") {
				StartCoroutine(delayedCredits());
			}

			if (gameObject.name.ToString () == "easyboard") {	
				OptionsMenuTT.AIDifficulty = "Easy";
				optionsScript.AssistanceToggle.isOn = true;
				StartCoroutine(delayedSinglePlayer());
			}

			if (gameObject.name.ToString () == "hardboard") {
				OptionsMenuTT.AIDifficulty = "Hard";
				optionsScript.AssistanceToggle.isOn = false;
				StartCoroutine(delayedSinglePlayer());
			}

			if (gameObject.name.ToString () == "expertboard") {
				OptionsMenuTT.AIDifficulty = "Expert";
				optionsScript.AssistanceToggle.isOn = false;
				StartCoroutine(delayedSinglePlayer());
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
}
