using UnityEngine;
using System.Collections;

public class OptionsScript : MonoBehaviour 
{
    public static bool isOptionsOpen = false;

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void ResumeApplication()
    {

    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
