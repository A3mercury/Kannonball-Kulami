using UnityEngine;
using System.Collections;

public class LookAtMouseScript : MonoBehaviour {

	public GameObject target;

	void Update () 
	{
		transform.LookAt(target.transform);
	}
}
