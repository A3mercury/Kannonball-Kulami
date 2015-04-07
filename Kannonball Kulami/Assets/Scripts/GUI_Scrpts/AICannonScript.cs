using UnityEngine;
using System.Collections;

public class AICannonScript : MonoBehaviour {

	GameObject OpponentCannon;
	Vector3 shotPosition;
	private GameCore coreScript;

	// Use this for initialization
	void Start () {
		OpponentCannon = gameObject;//GameObject.Find("OpponentCannon");
		coreScript = FindObjectOfType (typeof(GameCore)) as GameCore;
	}
	
	// Update is called once per frame
	void Update () {
		if(coreScript.AIMove != null)
			MoveCannon (coreScript.AIMove.transform.position);
			//MoveCannon(new Vector3(10f, 10f, 5f));
	}

	void MoveCannon(Vector3 target)
	{
		OpponentCannon.transform.LookAt(target);
		//Debug.Log("Euler angle x: " + PlayerCannon.transform.eulerAngles.x + " y: " + PlayerCannon.transform.eulerAngles.y + 90 + " z: " + PlayerCannon.transform.eulerAngles.z);
		OpponentCannon.transform.eulerAngles = new Vector3(0, OpponentCannon.transform.eulerAngles.y + 90, 0);
	}
}
