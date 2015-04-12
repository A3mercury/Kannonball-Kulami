﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenuTT : MonoBehaviour 
{
    Canvas parentCanvas;

	GameCore clickScript;
	SceneTransitionScript mainMenuClickScript;

    Image[] images;
    GameObject optionPanel;
    private Network_Manager networkManager;

    GameObject soundsParent;
    //AudioSource[] sounds;
    AudioSource backgroundShipNoise;
    AudioSource KannonballKulamiTheme;
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

	public static Toggle AssistanceToggle;

    // starts at half
    float sliderStartVol = 0.5f;
    float cannonballStartVol = 0.5f;
    float musicSliderCurrentVol = 0.5f;
    float soundsSliderCurrentVol = 0.5f;

    public static bool areSoundsMuted = false;
    public static bool areMusicMuted = false;
	public static bool isAssistanceChecked = true;	
	public static bool PlayerGoesFirst = true;
	public static string AIDifficulty = "Easy";

    // on gui
    public GUISkin OptionsSkin;

	// Use this for initialization
	void Start () 
    {
        // load the sounds for GameScene
        if (Application.loadedLevelName == "GameScene") 
		{
            soundsParent = GameObject.Find("AudioSounds");
            backgroundShipNoise = GameObject.Find("BackgroundShipNoise").GetComponent<AudioSource>();

			clickScript = GameObject.FindObjectOfType (typeof(GameCore)) as GameCore;
            networkManager = GameObject.FindObjectOfType<Network_Manager>();

		}
        else if (Application.loadedLevelName == "MainMenuScene")
        {
            KannonballKulamiTheme = GameObject.Find("KannonBallKulamiTheme").GetComponent<AudioSource>();
            if (!areMusicMuted)
            {
                KannonballKulamiTheme.Play();
            }
        }

        AssignSliders();
        AssignAnimators();
        AssignButtons();

		AssistanceToggle = GameObject.Find ("assistance_checkbox").GetComponent<Toggle>();

		if (isAssistanceChecked) 
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

			if(Application.loadedLevelName == "MainMenuScene")
				mainMenuClickScript.ToggleClickability();

			if (Application.loadedLevelName != "MainMenuScene")
				clickScript.ToggleClickability();
        }
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			PlayerGoesFirst = false;
		}

        if (areSoundsMuted)
        {
            soundSlider.interactable = false;
            soundSlider.value = 0.0f;
			soundSliderChange();
        }
        else
        {
            soundSlider.interactable = true;
			soundSliderChange();
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
			isAssistanceChecked = true;
		} 
		else 
		{
			isAssistanceChecked = false;
		}

        // if the options menu is open, call soundSliderChange()
        if(optionPanel.gameObject.activeSelf == true)
            soundSliderChange();

        changeSoundsButton();
        changeMusicButton();

        if (AIDifficulty == "Expert" && Application.loadedLevelName != "MainMenuScene")
			AssistanceToggle.interactable = false;
        else
        {
            AssistanceToggle.interactable = true;
        }
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
                soundSlider.value = soundsSliderCurrentVol;
               // SetSoundsToHalf();
            }
            else if (slider.name == "music_slider")
            {
                musicSlider = slider;
                musicSlider.interactable = true;
                musicSlider.value = musicSliderCurrentVol;
              //  SetMusicToHalf();
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
        else if (Application.loadedLevelName == "MainMenuScene")
        {
            KannonballKulamiTheme.volume = musicSlider.value;
        }

        // sliders for the MainMenuScene
    }

    public void MuteSounds()
    {
        if(areSoundsMuted) // if sounds are muted, unmute them
        {
            areSoundsMuted = false;
            soundSlider.value = soundsSliderCurrentVol;
            soundSlider.interactable = true;
        }
        else // if sounds are not muted, mute them
        {
            areSoundsMuted = true;
            soundsSliderCurrentVol = soundSlider.value;
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
            musicSlider.value = musicSliderCurrentVol;
            musicSlider.interactable = false;
            if (Application.loadedLevelName == "MainMenuScene")
            {
                if (!KannonballKulamiTheme.isPlaying)
                {
                    KannonballKulamiTheme.Play();
                }
            }
            // set music volume
        }
        else
        {
            areMusicMuted = true;
            musicSliderCurrentVol = musicSlider.value;
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
            clickScript.ToggleClickability();
        if (Application.loadedLevelName == "MainMenuScene")
            mainMenuClickScript.ToggleClickability();
    }

    public void concedeGame()
    {
        //Added for network play to return to lobby
        if (networkManager.isOnline)
        {
            networkManager.isOnline = true;
            networkManager.networkView.RPC("Concede", RPCMode.Others, true);
            networkManager.StartServer();
            GameObject.FindObjectOfType<GameCore>().RemoveGameBoard();
            optionPanel.gameObject.SetActive(false);
        }
        else
        {
            AssistanceToggle.interactable = true;
            Application.LoadLevel("MainMenuScene");
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }

	public void ToggleClickScript ()
	{
		clickScript.ToggleClickability();
	}

	void OnGUI () 
	{
        GUI.skin = OptionsSkin;
		if (Application.loadedLevelName == "MainMenuScene" || Application.loadedLevelName == "GameScene") {
			if (GUI.Button (new Rect(20, Screen.height - (48), 48, 48), "", GUI.skin.customStyles[0])) {
				if (optionPanel.activeSelf == true)
				{
					resumeGame();
				}
				else
				{
					optionPanel.gameObject.SetActive (true);
					if (Application.loadedLevelName == "MainMenuScene")
						mainMenuClickScript.ToggleClickability();
					if (Application.loadedLevelName == "GameScene")
						ToggleClickScript();
				}
			}
		}
	}
}
