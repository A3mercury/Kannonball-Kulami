using UnityEngine;
using System.Collections;

public enum CameraPosition { Sun, Lobby, Board};

public class CameraGameSceneMovement : MonoBehaviour 
{
    public CameraPosition position = CameraPosition.Sun;
    private CameraPosition target = CameraPosition.Sun;
    Network_Manager network;
    GameObject CameraLookat1;
    GameObject CameraLookat2;
    public float speed = 1;
    float startTime;
    Vector3 lookTarget;
    Vector3 oldLook;
    float journeyLength;
    Canvas GameSceneCanvas;
    //GameObject ServerPanel;

	// Use this for initialization
	void Start () 
    {
        startTime = Time.time;
        GameSceneCanvas = GameObject.Find("GameSceneCanvas").GetComponent<Canvas>();
        //ServerPanel = GameObject.Find("ServerPanel");
        
        network = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
        CameraLookat1 = GameObject.Find("CameraLookat1");
        CameraLookat2 = GameObject.Find("CameraLookat2");
        journeyLength = Vector3.Distance(CameraLookat1.transform.position, CameraLookat2.transform.position);
        oldLook = CameraLookat1.transform.position;
        SelectCameraPosition();
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.LookAt(Vector3.Lerp(oldLook, lookTarget, fracJourney));
        if (fracJourney > 0.95f)
        {
            position = target;
        }
        //OpponentCannon.transform.eulerAngles = new Vector3(0, OpponentCannon.transform.eulerAngles.y + 90, 0);
        
    }

    public void SelectCameraPosition()
    {

        if (network.isOnline && !network.ingame)
        {
            lookTarget = CameraLookat1.transform.position;
            target = CameraPosition.Lobby;
            //transform.LookAt(CameraLookat1.transform.position);
            //ServerPanel.SetActive(true);
        }
        else
        {
            lookTarget = CameraLookat2.transform.position;
            target = CameraPosition.Board;
            //transform.LookAt(CameraLookat2.transform.position);
            //ServerPanel.SetActive(false);
        }
    }
}
