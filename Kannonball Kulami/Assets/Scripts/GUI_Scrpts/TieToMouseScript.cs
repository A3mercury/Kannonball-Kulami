using UnityEngine;
using System.Collections;

public class TieToMouseScript : MonoBehaviour {

	private float speed = 100.0f;
	private Vector3 position;

	// Use this for initialization
	void Start () {
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//float distance = -transform.position.y + Camera.main.transform.position.y;

		position = new Vector3((Input.mousePosition.x/8) - 47, Input.mousePosition.z, (Input.mousePosition.y/6) - 25);
		//position = Camera.main.ScreenToWorldPoint(position);

		position.y = 15;

		transform.position = Vector3.MoveTowards (transform.position, position, speed * Time.deltaTime);
	}
}
