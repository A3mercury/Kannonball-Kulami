using UnityEngine;
using System.Collections;

public class SplashScreens : MonoBehaviour
{
    private bool firstLogo;

    GameObject ssLogo;
    GameObject kkLogo;

	// Use this for initialization
	void Start () 
    {
        firstLogo = true;

        ssLogo = GameObject.Find("sslogo");
        ssLogo.SetActive(true);

        kkLogo = GameObject.Find("kklogo");
        kkLogo.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            if(firstLogo)
            {
                ssLogo.SetActive(false);
                kkLogo.SetActive(true);
                firstLogo = false;
            }
            else
            {
                Application.LoadLevel("MainMenuS");
            }
        }
	}


}
