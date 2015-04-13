using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionScript : MonoBehaviour {

	public static bool isClickable = true;
    //private Network_Manager networkManager;
	GameObject optionsPanel;
	OptionsMenuTT optionsScript;
    bool clickedOnFirst;

	void Start () {
		optionsScript = GameObject.FindObjectOfType (typeof(OptionsMenuTT)) as OptionsMenuTT;
        //networkManager = GameObject.FindObjectOfType<Network_Manager>();
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
        //Network_Manager.fromtransition = false;
        //OptionsMenuTT.PlayerGoesFirst = true;
	}

    public void OrderOfPlayers()
    {
        Network_Manager.fromtransition = false;
        if (clickedOnFirst)
        {
            OptionsMenuTT.PlayerGoesFirst = true;

        }
        else
        {
            OptionsMenuTT.PlayerGoesFirst = false;

        }
        StartCoroutine(delayedSinglePlayer());
    }

	public void NetworkPlay () {
        Network_Manager.fromtransition = true;
        Application.LoadLevel("GameScene");
	}

	public void Credits () {
		Application.LoadLevel("CreditsScene");
	}	

	public void MainMenu () {
        if (Application.loadedLevelName == "HowToPlayScene")
            Application.LoadLevel("MainMenuScene");

        if(Network_Manager.fromtransition)
        {
            Application.LoadLevel("GameScene");
        }
        else
		    Application.LoadLevel("MainMenuScene");
	}

    public void ExitGame()
    {
        Application.Quit();
    }
    public void HowToPlay()
    {
        Application.LoadLevel("HowToPlayScene");
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

    IEnumerator delayedHowToPlay()
    {
        yield return new WaitForSeconds(1);
        HowToPlay();
    }

    public void OnMouseDown()
    {
        //Debug.Log("isClickable " + isClickable);
		if (isClickable) 
		{
            //if (gameObject.name.ToString () == "singleplayer") {
            //    OrderOfPlayers();
            //}

			if (gameObject.name.ToString () == "networkplay") {
				StartCoroutine(delayedNetworkPlay());
			}

			if (gameObject.name.ToString () == "credits") {
				StartCoroutine(delayedCredits());
			}

            if (gameObject.name.ToString() == "easyboard")
            {
                OptionsMenuTT.AIDifficulty = "Easy";
                OptionsMenuTT.AssistanceToggle.isOn = true;
                //StartCoroutine(delayedSinglePlayer());
            }

			if (gameObject.name.ToString () == "hardboard") {
				OptionsMenuTT.AIDifficulty = "Hard";
				//StartCoroutine(delayedSinglePlayer());
			}

			if (gameObject.name.ToString () == "expertboard") {
				OptionsMenuTT.AIDifficulty = "Expert";
				OptionsMenuTT.AssistanceToggle.isOn = false;
				//StartCoroutine(delayedSinglePlayer());
			}
            if (gameObject.name.ToString() == "how_to_play_bottle")
            {
                StartCoroutine(delayedHowToPlay());
            }

            if(gameObject.name.ToString() == "first")
            {
                clickedOnFirst = true;
                OrderOfPlayers();
                //StartCoroutine(delayedSinglePlayer());
            }
            if(gameObject.name.ToString() == "second")
            {
                clickedOnFirst = false;
                OrderOfPlayers();
                //StartCoroutine(delayedSinglePlayer());
            }
            if(gameObject.name.ToString() == "Back_To_Main_Menu")
            {
                MainMenu();
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
