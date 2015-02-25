using UnityEngine;
using System.Collections;

public class Buoyancy : MonoBehaviour 
{
    public float UpwardForce = 12.72f; 
    public bool isInWater = false;
    GameObject w = GameObject.Find("Water4Example (Advanced)");

    void InWater()
    {
        float height = w.transform.position.y;

        if(this.transform.position.y <= 0)
        {
            isInWater = true;
            rigidbody.drag = 5f;
        }
    }

	void OnTriggerEnter(Collider collider)
    {
        isInWater = true;
        rigidbody.drag = 5f;
    }

    void OnTriggerExit(Collider collider)
    {
        isInWater = false;
        rigidbody.drag = 0.05f;
    }

    void FixedUpdate()
    {
        if(isInWater)
        {
            // apply upward force
            Vector3 force = transform.up * UpwardForce;
            this.rigidbody.AddRelativeForce(force, ForceMode.Acceleration);
            Debug.Log("Upward force: " + force + " @" + Time.time);
        }
    }
}
