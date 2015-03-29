using UnityEngine;
using System.Collections;

public class CameraGameSceneMovement : MonoBehaviour 
{
    Network_Manager network;
    GameObject CameraLookat1;
    GameObject CameraLookat2;

    Canvas GameSceneCanvas;
    GameObject ServerPanel;

	// Use this for initialization
	void Start () 
    {
        GameSceneCanvas = GameObject.Find("GameSceneCanvas").GetComponent<Canvas>();
        ServerPanel = GameObject.Find("ServerPanel");

        network = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
        CameraLookat1 = GameObject.Find("CameraLookat1");
        CameraLookat2 = GameObject.Find("CameraLookat2");

        SelectCameraPosition();
    }

    public void SelectCameraPosition()
    {

        if (network.isOnline)
        {
            transform.LookAt(CameraLookat1.transform.position);
            ServerPanel.SetActive(true);
        }
        else
        {
            transform.LookAt(CameraLookat2.transform.position);
            ServerPanel.SetActive(false);
        }
    }
}
