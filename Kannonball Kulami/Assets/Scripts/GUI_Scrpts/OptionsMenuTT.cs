using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenuTT : MonoBehaviour 
{
    Canvas parentCanvas;

	ClickGameboard clickScript;

    Image[] images;
    GameObject optionPanel;
    public AudioSource soundSource;
    AudioSource cannonballFire;

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

    float sliderStartVol = 0.75f;
    float cannonballStartVol = 0.6f;

	// Use this for initialization
	void Start () 
    {
        if(Application.loadedLevelName == "GameScene")
            cannonballFire = GameObject.Find("GameCore").GetComponent<AudioSource>();

        AssignSliders();
        AssignAnimators();
        AssignButtons();

        optionPanel = GameObject.Find("OptionsPanel");
        optionPanel.gameObject.SetActive(false);

        parentCanvas = GetComponentInParent<Canvas>();

		clickScript = GameObject.FindObjectOfType (typeof(ClickGameboard)) as ClickGameboard;

        // disable rest of game's OnMouseDown methods
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.gameObject.activeSelf == true)
            {
                optionPanel.gameObject.SetActive(false);
				clickScript.ToggleClickablity();
            }
            else
            {
                optionPanel.gameObject.SetActive(true);
				clickScript.ToggleClickablity();
            }
        }

        if(optionPanel.gameObject.activeSelf == true)
            soundSliderChange();

        
	}

    void AssignSliders()
    {
        optionSliders = GetComponentsInChildren<Slider>();
        foreach (Slider slider in optionSliders)
        {
            if (slider.name == "sounds_slider")
            {
                soundSlider = slider;
                soundSlider.interactable = true;
                soundSlider.value = sliderStartVol;
                SetSoundsToHalf();
            }
            else if (slider.name == "music_slider")
            {
                musicSlider = slider;
                musicSlider.interactable = true;
                musicSlider.value = sliderStartVol;
                SetMusicToHalf();
            }
        }
    }
    void AssignAnimators()
    {
        optionAnimators = GetComponentsInChildren<Animator>();

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
        if(Application.loadedLevelName == "GameScene")
            ConcedeButton.onClick.AddListener(concedeGame);
        QuitButton.onClick.AddListener(quitGame);
        ResumeButton.onClick.AddListener(resumeGame);
        SoundsButton.onClick.AddListener(MuteSounds);
        MusicButton.onClick.AddListener(MuteMusic);
    }

    public void soundSliderChange()
    {
        if (Application.loadedLevelName == "GameScene")
        {
            soundSource.volume = soundSlider.value;
            cannonballFire.volume = soundSlider.value;
        }
    }

    public void MuteSounds()
    {
        if (SoundsButtonAnimator.GetBool("isSoundsMuted"))
        {
            SoundsButtonAnimator.SetBool("isSoundsMuted", false);
            soundSlider.value = sliderStartVol;
            soundSlider.interactable = true;

            // TURN ON ALL SOUNDS
            SetSoundsToHalf();
        }
        else
        {
            SoundsButtonAnimator.SetBool("isSoundsMuted", true);
            soundSlider.value = 0.0f;
            soundSlider.interactable = false;

            // TURN OFF ALL SOUNDS
            soundSource.volume = 0.0f;
        }
    }

    public void MuteMusic()
    {
        if(MusicButtonAnimator.GetBool("isMusicMuted"))
        {
            MusicButtonAnimator.SetBool("isMusicMuted", false);
            musicSlider.value = sliderStartVol;
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
        if (Application.loadedLevelName == "GameScene")
        {
            soundSource.volume = sliderStartVol;
            cannonballFire.volume = cannonballStartVol;
        }
    }

    public void SetMusicToHalf()
    {

    }

    public void resumeGame()
    {
        optionPanel.gameObject.SetActive(false);
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
