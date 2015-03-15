using UnityEngine;
using System.Collections;

public class SceneTransitionScript : MonoBehaviour {

	public void SinglePlayer () {
        Network_Manager.fromtransition = false;
		Application.LoadLevel("gameboardTestScene");
	}
	
	public void NetworkPlay () {
        Network_Manager.fromtransition = true;
		Application.LoadLevel("gameboardTestScene");
	}

	public void Options () {
		//Application.LoadLevel ("OptionsScene");
	}

	public void Credits () {
		Application.LoadLevel("CreditsScene");
	}	

	public void MainMenu () {
		Application.LoadLevel("MainMenuS");
	}

    public void ExitGame()
    {
        Application.Quit();
    }

    //public void OnMouseDown () {
    //    if (!optionsMenu)
    //    {
    //        if (gameObject.name.ToString() == "MenuCube")
    //        {
    //            MainMenu();
    //        }

    //        if (gameObject.name.ToString() == "singleplayer")
    //        {
    //            SinglePlayer();
    //        }

    //        if (gameObject.name.ToString() == "networkplay")
    //        {
    //            NetworkPlay();
    //        }

    //        if (gameObject.name.ToString() == "options")
    //        {
    //            Options();
    //        }

    //        if (gameObject.name.ToString() == "credits")
    //        {
    //            Credits();
    //        }
    //    }
    //}
}
