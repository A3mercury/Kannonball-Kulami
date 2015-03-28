﻿using UnityEngine;
using System.Collections;

public class ReactToClick : MonoBehaviour {

    public float force;

    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo; // var to store info about the object hit (if any)

        // Check to see if ray hits any objects in scene (namely gameboard pieces)
        // pass in hitInfo, so that Raycast can store the info about the hit there
        // the 'out' keyword is a parameter modifier used to tell C# that this obj should be passed by ref
        //   to properly access hitInfo
        // objets we hope to hit must have a collider component
        if (Physics.Raycast(ray, out hitInfo))
        {
            rigidbody.AddForceAtPosition(new Vector3(0f, force, 0f), new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z));
        }
    }
}