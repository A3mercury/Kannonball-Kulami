using UnityEngine;
using System.Collections;

public class TieToMouseScript : MonoBehaviour {

	private float actualDistance;

	// Use this for initialization
	void Start () {
		actualDistance = (transform.position - Camera.main.transform.position).magnitude;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 myMousePosition = Input.mousePosition;
		myMousePosition.z = actualDistance;
		transform.position = Camera.main.ScreenToWorldPoint (myMousePosition);
	}
}
