using UnityEngine;
using System.Collections;

public class Attached : MonoBehaviour {
	private Vector3 attachedPosition;

	// Use this for initialization
	void Start () {
		attachedPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = attachedPosition;
	}
}
