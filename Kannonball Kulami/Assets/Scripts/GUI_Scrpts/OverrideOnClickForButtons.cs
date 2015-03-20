using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class OverrideOnClickForButtons : MonoBehaviour 
{
    Button acceptButton;

	// Use this for initialization
	void Start () 
    {
        acceptButton = this.GetComponent<Button>();
	}

    void OnClick()
    {

    }
}
