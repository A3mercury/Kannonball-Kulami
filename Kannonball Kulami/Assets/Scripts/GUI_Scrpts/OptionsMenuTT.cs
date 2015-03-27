using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenuTT : MonoBehaviour 
{
    Canvas parentCanvas;

	ClickGameboard clickScript;
	SceneTransitionScript mainMenuClickScript;

    Image[] images;
    GameObject optionPanel;

    GameObject soundsParent;
    //AudioSource[] sounds;
    AudioSource backgroundShipNoise;
    //AudioSource cannonballFireSound;

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

	Toggle AssistanceToggle;

    // starts at half
    float sliderStartVol = 0.5f;
    float cannonballStartVol = 0.5f;

    public static bool areSoundsMuted = false;
    public static bool areMusicMuted = false;
	public static bool isAssitanceChecked = true;

	// Use this for initialization
	void Start () 
    {
        // load the sounds for GameScene
        if (Application.loadedLevelName == "GameScene") 
		{
            soundsParent = GameObject.Find("AudioSounds");
            backgroundShipNoise = GameObject.Find("BackgroundShipNoise").GetComponent<AudioSource>();
            
            clickScript = GameObject.FindObjectOfType(typeof(ClickGameboard)) as ClickGameboard;		
		}

        AssignSliders();
        AssignAnimators();
        AssignButtons();
		AssistanceToggle = GameObject.Find ("assistance_checkbox").GetComponent<Toggle>();

		if (isAssitanceChecked) 
		{
			AssistanceToggle.isOn = true;
		} 
		else 
		{
			AssistanceToggle.isOn = false;
		}

        optionPanel = GameObject.Find("OptionsPanel");
        optionPanel.gameObject.SetActive(false);

        parentCanvas = GetComponentInParent<Canvas>();

		mainMenuClickScript = GameObject.FindObjectOfType (typeof(SceneTransitionScript)) as SceneTransitionScript;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.gameObject.activeSelf == true)
            {
                optionPanel.gameObject.SetActive(false);
            }
            else
            {
                optionPanel.gameObject.SetActive(true);
                
            }
			mainMenuClickScript.ToggleClickability();

		//	if (Application.loadedLevelName == "GameScene")
				//clickScript.ToggleClickablity();
        }


        if (areSoundsMuted)
        {
            soundSlider.interactable = false;
            soundSlider.value = 0.0f;
        }
        else
        {
            soundSlider.interactable = true;
        }
        if (areMusicMuted)
        {
            musicSlider.interactable = false;
            musicSlider.value = 0.0f;
        }
        else
        {
            musicSlider.interactable = true;
        }
		if (AssistanceToggle.isOn) 
		{
			isAssitanceChecked = true;
		} 
		else 
		{
			isAssitanceChecked = false;
		}

        // if the options menu is open, call soundSliderChange()
        if(optionPanel.gameObject.activeSelf == true)
            soundSliderChange();

        changeSoundsButton();
        changeMusicButton();
	}

    #region Assignments

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

    #endregion

    public void soundSliderChange()
    {
        // sliders for the GameScene
        if (Application.loadedLevelName == "GameScene")
        {
            CannonFireSound.SetVolume(soundSlider.value);
            backgroundShipNoise.volume = soundSlider.value;
        }

        // sliders for the MainMenuScene
    }

    public void MuteSounds()
    {
        if(areSoundsMuted) // if sounds are muted, unmute them
        {
            areSoundsMuted = false;
            soundSlider.value = sliderStartVol;
            soundSlider.interactable = true;

            SetSoundsToHalf();
        }
        else // if sounds are not muted, mute them
        {
            areSoundsMuted = true;
            soundSlider.value = 0.0f;
            soundSlider.interactable = false;
        }

        changeSoundsButton();
    }

    

    public void MuteMusic()
    {
        if (areMusicMuted)
        {
            areMusicMuted = false;
            musicSlider.value = sliderStartVol;
            musicSlider.interactable = false;

            // set music volume
        }
        else
        {
            areMusicMuted = true;
            musicSlider.value = 0.0f;
            musicSlider.interactable = true;
        }
    }

    void changeSoundsButton()
    {
        SoundsButtonAnimator.SetBool("isSoundsMuted", areSoundsMuted);
    }

    void changeMusicButton()
    {
        MusicButtonAnimator.SetBool("isMusicMuted", areMusicMuted);
    }

    public void SetSoundsToHalf()
    {
        if (Application.loadedLevelName == "GameScene")
        {
            backgroundShipNoise.volume = 0.5f;
            CannonFireSound.SetVolume(0.5f);
        }
    }

    public void SetMusicToHalf()
    {

    }

    public void resumeGame()
    {
        optionPanel.gameObject.SetActive(false);
        if (Application.loadedLevelName == "GameScene")
            clickScript.ToggleClickablity();
        if (Application.loadedLevelName == "MainMenuScene")
            mainMenuClickScript.ToggleClickability();
    }

    public void concedeGame()
    {
		mainMenuClickScript.ToggleClickability();
		Application.LoadLevel("MainMenuScene");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
