using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenuTT : MonoBehaviour 
{
    Canvas optionsCanvas;
    public AudioSource soundSource;

    Slider[] optionSliders;
    Slider soundSlider;
    Slider musicSlider;

    Animator[] optionAnimators;
    Animator SoundsButtonAnimator;
    Animator MusicButtonAnimator;

    Button[] optionButtons;
    Button ConcedeButton;
    Button QuitButton;
    Button ResumeButton;
    Button SoundsButton;
    Button MusicButton;

	// Use this for initialization
	void Start () 
    {
        optionsCanvas = GetComponent<Canvas>();
        optionsCanvas.enabled = false;

        AssignSliders();
        AssignAnimators();
        AssignButtons();
	}
	
	// Update is called once per frame
	void Update () 
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

    void AssignSliders()
    {
        Debug.Log("Make sure this is called");
        optionSliders = GetComponentsInChildren<Slider>();
        foreach (Slider slider in optionSliders)
        {
            if (slider.name == "sounds_slider")
            {
                soundSlider = slider;
                soundSlider.interactable = true;
                soundSlider.value = 0.5f;
                SetSoundsToHalf();
            }
            else if (slider.name == "music_slider")
            {
                musicSlider = slider;
                musicSlider.interactable = true;
                musicSlider.value = 0.5f;
                SetMusicToHalf();
            }
        }
    }
    void AssignAnimators()
    {
        optionAnimators = GetComponentsInChildren<Animator>();
        Debug.Log("Now for the animators " + optionAnimators.Length);

        foreach(Animator animator in optionAnimators)
        {
            if (animator.runtimeAnimatorController.name == "SoundsButtonController")
                SoundsButtonAnimator = animator;
            else if (animator.runtimeAnimatorController.name == "MusicButtonController")
                MusicButtonAnimator = animator;
        }
    }
    void AssignButtons()
    {
        optionButtons = GetComponentsInChildren<Button>();
        foreach(Button button in optionButtons)
        {
            if (button.name == "concede_button")
                ConcedeButton = button;
            else if (button.name == "quit_button")
                QuitButton = button;
            else if (button.name == "resume_button")
                ResumeButton = button;
            else if (button.name == "sounds_button")
                SoundsButton = button;
            else if (button.name == "music_button")
                MusicButton = button;
        }

        // button even listeners
        ConcedeButton.onClick.AddListener(concedeGame);
        QuitButton.onClick.AddListener(quitGame);
        ResumeButton.onClick.AddListener(resumeGame);
        SoundsButton.onClick.AddListener(MuteSounds);
        MusicButton.onClick.AddListener(MuteMusic);
    }

    public void soundSliderChange()
    {
        soundSource.volume = soundSlider.value;
    }

    public void MuteSounds()
    {
        if (SoundsButtonAnimator.GetBool("isSoundsMuted"))
        {
            Debug.Log("test ths");
            SoundsButtonAnimator.SetBool("isSoundsMuted", false);
            soundSlider.value = 0.5f;
            soundSlider.interactable = false;

            // TURN ON ALL SOUNDS
            SetSoundsToHalf();
        }
        else
        {
            SoundsButtonAnimator.SetBool("isSoundsMuted", true);
            soundSlider.value = 0.0f;
            soundSlider.interactable = true;

            // TURN OFF ALL SOUNDS
            soundSource.volume = 0.0f;
        }
    }

    public void MuteMusic()
    {
        if(MusicButtonAnimator.GetBool("isMusicMuted"))
        {
            MusicButtonAnimator.SetBool("isMusicMuted", false);
            musicSlider.interactable = false;

            // TURN ON ALL MUSIC
        }
        else
        {
            MusicButtonAnimator.SetBool("isMusicMuted", true);
            musicSlider.interactable = true;

            // TURN OFF ALL MUSIC
        }
    }

    public void SetSoundsToHalf()
    {
        soundSource.volume = 0.5f;
    }

    public void SetMusicToHalf()
    {

    }

    public void resumeGame()
    {
        optionsCanvas.enabled = false;
    }

    public void concedeGame()
    {
        Application.LoadLevel("LoseScene");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
