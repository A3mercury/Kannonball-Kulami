using UnityEngine;
using System.Collections;

public class OpenOptions : MonoBehaviour {

	public Canvas optionsCanvas;

	// Use this for initialization
	void Start () {
		optionsCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (optionsCanvas.enabled == true)
			{
				optionsCanvas.enabled = false;
			}
			else
			{
				optionsCanvas.enabled = true;
			}
		}
	}

	
	public void resumeGame ()
	{
		optionsCanvas.enabled = false;
	}
}
