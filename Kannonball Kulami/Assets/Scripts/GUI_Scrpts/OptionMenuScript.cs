using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionMenuScript : MonoBehaviour {

	public Canvas optionsCanvas;
	public AudioSource soundSource;
	public Slider soundSlider;

	void Start ()
	{
		//optionsCanvas.enabled = false;
	}

	public void concedeGame ()
	{
		Application.LoadLevel ("LoseScene");
	}

	public void quitGame ()
	{
		Application.Quit();
	}

	public void resumeGame ()
	{
		optionsCanvas.enabled = false;
	}

	public void soundSliderChange ()
	{
		soundSource.volume = soundSlider.value;
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
}
