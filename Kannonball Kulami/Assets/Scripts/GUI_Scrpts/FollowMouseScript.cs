using UnityEngine;
using System.Collections;

public class FollowMouseScript : MonoBehaviour { 

	Vector3 ScreenMouse;
	Vector3 WorldMouse1;
	Vector3 WorldMouse2;
	public Camera tether1;
	public Camera tether2;
	public GameObject cannon1;
	public GameObject cannon2;

	// Use this for initialization
	void Start () {

	}

	void FixedUpdate () {
		ScreenMouse.x = Input.mousePosition.x;
		ScreenMouse.z = 1;
		ScreenMouse.y = Input.mousePosition.y;
		WorldMouse1 = tether1.ScreenToWorldPoint (ScreenMouse);
		WorldMouse2 = tether2.ScreenToWorldPoint (ScreenMouse);
	}

	void OnMouseOver () {

		cannon1.transform.LookAt (WorldMouse1); 
		cannon2.transform.LookAt (WorldMouse2);
	}
}
