using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionMenuScript : MonoBehaviour {

    public Canvas optionsCanvas;

	public AudioSource soundSource;

	public Slider soundSlider;
    public Slider musicSlider;

    public Animator[] optionAnimators;
    public Animator SoundsButtonAnimator;
    public Animator MusicButtonAnimator;

	void Start ()
	{
        optionsCanvas = GetComponent<Canvas>();
        optionsCanvas.enabled = false;

        // get animators for the music and sounds in options menu
        optionAnimators = GetComponentsInChildren<Animator>();
        Debug.Log("number of animators in optns = " + optionAnimators.Length);
        Debug.Log(optionAnimators[0].runtimeAnimatorController.name);
        foreach(Animator anim in optionAnimators)
        {
           Debug.Log(anim.runtimeAnimatorController.name);
            /*
            if (a.runtimeAnimatorController.name == "SoundsButtonController")
            {
                SoundsButtonAnimator = a;
                SoundsButtonAnimator.SetBool("isSoundsMuted", false);
            }
            else if (a.runtimeAnimatorController.name == "MusicButtonController")
            {
                MusicButtonAnimator = a;
                MusicButtonAnimator.SetBool("isMusicMuted", false);
            }
            */
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

	public void concedeGame ()
	{
		Application.LoadLevel ("LoseScene");
	}

	public void quitGame ()
	{
		Application.Quit();
	}

    public void resumeGame()
    {
        optionsCanvas.enabled = false;
    }

	public void soundSliderChange ()
	{
		soundSource.volume = soundSlider.value;
	}

    public void musicSliderChange()
    {
        //musicSource.volume = musicSlider.value;
    }

    public void MuteSounds()
    {
        if (SoundsButtonAnimator.GetBool("isSoundsMuted"))
        {
            SoundsButtonAnimator.SetBool("isSoundsMuted", false);
            soundSlider.interactable = true;
        }
        else
        {
            SoundsButtonAnimator.SetBool("isSoundsMuted", true);
            soundSlider.interactable = false;
        }

        

        // turn off all sounds
    }

    public void MuteMusic()
    {
        if (MusicButtonAnimator.GetBool("isMusicMuted"))
        {
            MusicButtonAnimator.SetBool("isMusicMuted", false);
            musicSlider.interactable = true;
        }
        else
        {
            MusicButtonAnimator.SetBool("isMusicMuted", true);
            musicSlider.interactable = false;
        }
        // turn off all music
    }

}
