using UnityEngine;
using System.Collections;

public class CannonFireAnimate : MonoBehaviour 
{
    public Animator FireCannon;
    public static bool letTheBassCannonKickIt = false;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (letTheBassCannonKickIt)
        {
            FireCannon.SetTrigger("CannonFire");
            letTheBassCannonKickIt = false;
        }
	}
}
