using UnityEngine;
using System.Collections;

public class LookAtMouseScript : MonoBehaviour {

	public Transform target;
	public Transform self;
	public float speed = 1.0f;
		
	void Update () 
	{
		Vector3 targetDir = target.position - transform.position;
		float step = speed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		Debug.DrawRay(transform.position, newDir, Color.red);
		self.GetChild(0).transform.rotation = Quaternion.LookRotation(newDir);
	}
}
