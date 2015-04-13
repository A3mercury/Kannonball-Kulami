using UnityEngine;
using System.Collections;

public class SinglePlayerOptions : MonoBehaviour {

	public GameObject easyboard;
	public GameObject hardboard;
	public GameObject expertboard;

    public GameObject first;
    public GameObject second;

	// Use this for initialization
	void Start () {
		easyboard = GameObject.Find ("easyboard");
		hardboard = GameObject.Find ("hardboard");
		expertboard = GameObject.Find ("expertboard");

        first = GameObject.Find("first");
        second = GameObject.Find("second");

		easyboard.SetActive (false);
		hardboard.SetActive (false);
		expertboard.SetActive (false);

        first.SetActive(false);
        second.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown()
	{
		if (SceneTransitionScript.isClickable)
		{
            if (!easyboard.activeSelf)
            {
                easyboard.SetActive(true);
                hardboard.SetActive(true);
                expertboard.SetActive(true);
            }
            else if(this.name == "easyboard" || this.name == "hardboard" || this.name == "expertboard")
            {
                first.SetActive(true);
                second.SetActive(true);
            }
		}
	}
}
