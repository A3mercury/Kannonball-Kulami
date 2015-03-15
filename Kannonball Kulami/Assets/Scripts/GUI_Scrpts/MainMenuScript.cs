using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour 
{
    SceneTransitionScript Scenes;
	// Use this for initialization
	void Start () 
    {
        Scenes = new SceneTransitionScript();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void OnMouseDown()
    {
        if(gameObject.name.ToString() == "MenuCube")
        {
            Scenes.MainMenu();
        }

        if (gameObject.name.ToString() == "singleplayer")
        {
            Scenes.SinglePlayer();
        }

        if (gameObject.name.ToString() == "networkplay")
        {
            Scenes.NetworkPlay();
        }

        if (gameObject.name.ToString() == "options")
        {
            Scenes.Options();
        }

        if (gameObject.name.ToString() == "credits")
        {
            Scenes.Credits();
        }
    }
}
