using UnityEngine;
using System.Collections;

public class FireAt : MonoBehaviour {

	public GameObject theTarget;
	public string turn;
	public int row;
	public int col;
	public GameCore gameCore;
	public float speed;

	public void OnMouseDown()
	{
		//Vector3 heading = theTarget.transform.position - transform.position;
		//float distance = heading.magnitude;
		//Vector3 direction = heading / distance;
		//if(distance < direction.magnitude * speed)
		//{
		//	//gameCore.PlacePhysical(row, col, turn);
		//	Destroy(gameObject);
		//}
		//else
		//{
		//	transform.position += direction * speed;
		//}
	}

	public void FixedUpdate()
	{
		Vector3 heading = theTarget.transform.position - transform.position;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;
		if(distance < direction.magnitude * speed * 3)
		{
			gameCore.PlacePhysical(row, col, turn);
			Destroy(gameObject);
		}
		else
		{
			transform.position += direction * speed;
		}
	}

	public void set(int r, int c, string t, GameObject target, float s, GameCore core)
	{
		row = r;
		col = c;
		turn = t;
		theTarget = target;
		speed = s;
		gameCore = core;
	}
	
}
