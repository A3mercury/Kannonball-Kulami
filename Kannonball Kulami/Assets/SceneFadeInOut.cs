using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
    RawImage img;

    private bool sceneStarting = false;      // Whether or not the scene is still fading in.


    void Awake()
    {
        // Set the texture so that it is the the size of the screen and covers it.
        //guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
        img = GetComponent<RawImage>();

    }


    void Update()
    {
        // If the scene is starting...
        if (sceneStarting)
        {
            //... call the StartScene function.
            StartScene();
        }
    }

    void FadeToClear()
    {
        // Lerp the colour of the texture between itself and transparent.
        img.color = Color.Lerp(img.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToBlack()
    {
        // Lerp the colour of the texture between itself and black.
        img.color = Color.Lerp(img.color, Color.white, fadeSpeed * Time.deltaTime);
    }


    void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (img.color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the GUITexture.
            img.color = Color.clear;
            img.enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }


    public void EndScene()
    {
        // Make sure the texture is enabled.
        img.enabled = true;

        // Start fading towards black.
        FadeToBlack();

        // If the screen is almost black...
        if (guiTexture.color.a >= 0.95f)
            // ... reload the level.
            Application.LoadLevel(0);
    }
}
