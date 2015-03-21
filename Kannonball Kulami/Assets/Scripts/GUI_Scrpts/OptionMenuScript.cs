using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionMenuScript : MonoBehaviour {
	
	public AudioSource soundSource;
	public Slider soundSlider;

	void Start ()
	{

	}

	public void concedeGame ()
	{
		Application.LoadLevel ("LoseScene");
	}

	public void quitGame ()
	{
		Application.Quit();
	}

	public void soundSliderChange ()
	{
		soundSource.volume = soundSlider.value;
	}
}
