using UnityEngine;
using System.Collections;

public class OptionsScript : MonoBehaviour 
{
    public Animator optionsButton;

    public void OpenOptionsMenu()
    {
        Debug.Log("clicked");
        optionsButton.SetBool("isVisible", true);
    }
}
