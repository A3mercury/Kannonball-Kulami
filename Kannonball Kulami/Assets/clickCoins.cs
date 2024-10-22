﻿using UnityEngine;
using System.Collections;

public class clickCoins : MonoBehaviour {

    public Transform spoutTransform;
    public ParticleSystem coinSpout;
    public AudioSource coinSound;

    public static float volume;
   // public BoxCollider mesh;
   // public SphereCollider sphere;
    //private Camera mainCam;

    void Start()
    {
        //mainCam = GameObject.Find("Main Camera").GetComponent<Camera>() as Camera;
		spoutTransform = GameObject.Find("Coin Spout").transform;
        //coinSpout = GetComponentInChildren<ParticleSystem>();
        //mesh = GetComponent<MeshCollider>();
        //sphere = GetComponent<SphereCollider>();
    }

	void OnMouseDown()
    {
       
            spoutTransform.position = collider.bounds.center;
            //Debug.Log("clicked");
            coinSpout.audio.volume = GetVolume();
            coinSpout.Play();
           // coinSound.PlayOneShot(coinSound.clip);
            //RaycastHit hit;
            //Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit);
            //Vector3 closestSphere = sphere.ClosestPointOnBounds(hit.point);
            //Vector3 closestMesh = mesh.ClosestPointOnBounds(hit.point);
            ////if (hit.collider.name == "Sphere Collider" )
            //{
            //    spoutTransform.position = closestSphere;
            //	Debug.Log(closestSphere + "sphere");
            //    coinSpout.Play();
            //}
            //else
            //{
            //    spoutTransform.position = closestMesh;
            //    coinSpout.Play();
            //	Debug.Log(closestMesh + "mesh");
            //}
    
    }
    public static void SetVolume(float v)
    {
        volume = v;
    }

    public float GetVolume()
    {
        return volume;
    }
}
