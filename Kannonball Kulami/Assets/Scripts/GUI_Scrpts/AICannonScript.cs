using UnityEngine;
using System.Collections;

public class AICannonScript : MonoBehaviour {

	GameObject OpponentCannon;
	Vector3 targetPosition = new Vector3(-14f, 11f, -14f);
	Vector3 oldPosition;
	private GameCore coreScript;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	private bool firing = false;
	private bool firstMove = true;
	private int targetRow;
	private int targetCol;
	
	// Use this for initialization
	void Start () {
		OpponentCannon = gameObject;//GameObject.Find("OpponentCannon");
		coreScript = FindObjectOfType (typeof(GameCore)) as GameCore;
	}
	
	// Update is called once per frame
	void Update () {
		//if(coreScript.myJob != null && coreScript.AIMove != null)
		//{
		//	//MoveCannon (coreScript.AIMove.transform.position);
		//	//MoveCannon(new Vector3(10f, 10f, 5f));
		//	oldPosition = targetPosition;
		//	targetPosition = coreScript.AIMove.transform.position;
		//	startTime = Time.time;
		//	journeyLength = Vector3.Distance(oldPosition, targetPosition);
		//}
		//if(!firstMove)
		//{
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			if(fracJourney >= 1)
			{
				//Debug.Log ("done moving cannon");
				if(firing)
				{
					//Debug.Log("firing cannon");
					//FIRE!
					coreScript.PlacePiece(targetRow, targetCol);
					firing = false;
				}
			}
			else
			{
				OpponentCannon.transform.LookAt(Vector3.Lerp(oldPosition, targetPosition, fracJourney));
				OpponentCannon.transform.eulerAngles = new Vector3(0, OpponentCannon.transform.eulerAngles.y + 90, 0);
			}
		//}

	}

	public void MoveCannon(Vector3 target)
	{
		//OpponentCannon.transform.LookAt(target);
		////Debug.Log("Euler angle x: " + PlayerCannon.transform.eulerAngles.x + " y: " + PlayerCannon.transform.eulerAngles.y + 90 + " z: " + PlayerCannon.transform.eulerAngles.z);
		//OpponentCannon.transform.eulerAngles = new Vector3(0, OpponentCannon.transform.eulerAngles.y + 90, 0);
        if(!firing)
        {
            oldPosition = targetPosition;
            targetPosition = target;
            startTime = Time.time;
            journeyLength = Vector3.Distance(oldPosition, targetPosition) * 2;
        }
	}

	public void MoveCannonAndFire(Vector3 target, int row, int col)
	{
		targetRow = row;
		targetCol = col;
		oldPosition = targetPosition;
		targetPosition = target;
		startTime = Time.time;
		journeyLength = Vector3.Distance(oldPosition, targetPosition);
		firing = true;
		firstMove = false;
	}
}
