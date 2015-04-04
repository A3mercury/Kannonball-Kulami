using UnityEngine;
using System.Collections;

public class SinglePlayerOptions : MonoBehaviour {

	public GameObject easyboard;
	public GameObject hardboard;
	public GameObject expertboard;
	// Use this for initialization
	void Start () {
		easyboard = GameObject.Find ("easyboard");
		hardboard = GameObject.Find ("hardboard");
		expertboard = GameObject.Find ("expertboard");

		easyboard.SetActive (false);
		hardboard.SetActive (false);
		expertboard.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown()
	{
		if (SceneTransitionScript.isClickable)
		{
			easyboard.SetActive (true);
			hardboard.SetActive (true);
			expertboard.SetActive (true);
		}
	}
}
