using UnityEngine;
using System.Collections;

public class SplashScreens : MonoBehaviour
{
    private bool firstLogo;

    GameObject ssLogo;
    GameObject kkLogo;

    float countdown;

	// Use this for initialization
	void Start () 
    {
        countdown = 5f;

        firstLogo = true;

        ssLogo = GameObject.Find("sslogo");
        ssLogo.SetActive(true);

        kkLogo = GameObject.Find("kklogo");
        kkLogo.SetActive(false);
	}
	

	// Update is called once per frame
	void Update ()
    {
        countdown -= Time.deltaTime;

	    if(countdown <= 0 || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            //if(firstLogo)
            //{
            //    ssLogo.SetActive(false);
            //    kkLogo.SetActive(true);
            //    firstLogo = false;
            //    countdown = 15f;
            //}
            //else
            //{
                Application.LoadLevel("IntroAnimationScene");
            //}
        }
	}


}
