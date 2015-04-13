using UnityEngine;
using System.Collections;

public class IntroAnimation : MonoBehaviour 
{
    public AudioSource IntroMusic;
    public AudioSource Welcome;

    public Animator CameraAnimator;
    public Animation Anim;

    float countdown = 0;
    bool stopbeinginhere = true;

	// Use this for initialization
	void Start () 
    {
        //CameraAnimator = GameObject.Find("MainCamera").GetComponent<Animator>();
        //IntroMusic = GameObject.Find("IntroMusic").GetComponent<AudioSource>();
        //Welcome = GameObject.Find("Welcome").GetComponent<AudioSource>();

	}

	// Update is called once per frame
    void Update()
    {
        countdown += Time.deltaTime;

        if (stopbeinginhere)
        {
            if (countdown >= 14.75f)
            {
                Welcome.Play();
                stopbeinginhere = false;
            }
        }

        if (countdown >= 18.5f || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel("MainMenuScene");
    }
}
