﻿using UnityEngine;
using System.Collections;

public class SceneTransitionScript : MonoBehaviour {

	static bool isClickable = true;

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

    //public void Options()
    //{
    //    OptionsScirpt

    //    opScript.OpenOptionsMenu();

    //    //Application.LoadLevel ("OptionsScene");
    //}

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

    public void OnMouseDown()
    {
        Debug.Log("isClickable " + isClickable);
		if (isClickable) 
		{
				if (gameObject.name.ToString () == "MenuCube") {
					MainMenu ();
				}

				if (gameObject.name.ToString () == "singleplayer") {
					SinglePlayer ();
				}

				if (gameObject.name.ToString () == "networkplay") {
					NetworkPlay ();
				}

				//if (gameObject.name.ToString() == "options")
				//{
				//    Options();
				//}

				if (gameObject.name.ToString () == "credits") {
					Credits ();
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
