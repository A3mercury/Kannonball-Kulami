using UnityEngine;
using System.Collections;

public class TieToMouseScript : MonoBehaviour {

    ////////////private float actualDistance;

    private Vector3 lastKnownPosition = new Vector3(0f, 0f, 0f);
    private Network_Manager networkManager;
    float depthIntoScene = 10f;
    GameObject PlayerCannon;

    Camera mainCam;

	// Use this for initialization
	void Start () {
        //////////actualDistance = (transform.position - Camera.main.transform.position).magnitude;
        PlayerCannon = GameObject.Find("PlayerCannon");
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        networkManager = GameObject.FindObjectOfType<Network_Manager>();
	}
	
    void SendLocationAsServer()
    {
       // Debug.Log("sending " + lastKnownPosition + " to client");
        networkManager.networkView.RPC("MoveOpponentCannon", RPCMode.Others, lastKnownPosition.x, lastKnownPosition.y, lastKnownPosition.z);
       // Debug.Log(networkManager);
    }

    void SendLocationAsClient()
    {
        //Debug.Log("sending " + lastKnownPosition + " to server");
        networkManager.networkView.RPC("MoveOpponentCannon", RPCMode.Server, lastKnownPosition.x, lastKnownPosition.y, lastKnownPosition.z);
        //Debug.Log(networkManager);
    }

	// Update is called once per frame
	void Update () {
        //////////Vector3 myMousePosition = Input.mousePosition;
        //////////myMousePosition.z = actualDistance;
        //////////transform.position = Camera.main.ScreenToWorldPoint (myMousePosition);

        MoveToMouseAtObjectDepth();
	}

    public void BeginSendingOverNetwork()
    {
        if (Network.isServer)
        {
            InvokeRepeating("SendLocationAsServer", 2f, 0.1f);
        }
        else
        {
            InvokeRepeating("SendLocationAsClient", 2f, 0.1f);
        }
    }

    void MoveToMouseAtObjectDepth()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        // create a ray that goes into the scene from the camera, through the mouse position
        if (mainCam.enabled)
        {
            int layerMask = 1 << 9; //will only hit objects in the Board Space layer
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            float depth;
            RaycastHit hitInfo; // var to store info about the object hit (if any)

            // Check to see if ray hits any objects in scene (namely gameboard pieces)
            // pass in hitInfo, so that Raycast can store the info about the hit there
            // the 'out' keyword is a parameter modifier used to tell C# that this obj should be passed by ref
            //   to properly access hitInfo
            // objets we hope to hit must have a collider component
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
            {
                lastKnownPosition = hitInfo.point;
                // move this obj to the position we hit
                this.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
				//Debug.Log("Mouse x = " + hitInfo.point.x + ", y = " +hitInfo.point.y + ", z = " + hitInfo.point.z);
				//if (hitInfo.point.x > -16 && hitInfo.point.x < 15 && hitInfo.point.y > 3.8 && hitInfo.point.y < 4.3)
                MoveCannon(this.transform.position);
            }
            else // didn't hit anything
            {
                depth = depthIntoScene;
                MoveToMouseAtSpecifiedDepth(depthIntoScene);
            }
        }
    }

    void MoveToMouseAtSpecifiedDepth(float depth)
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = depth;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        this.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, mouseWorldPosition.z);
    }

    void MoveCannon(Vector3 target)
    {
        PlayerCannon.transform.LookAt(target);
        //Debug.Log("Euler angle x: " + PlayerCannon.transform.eulerAngles.x + " y: " + PlayerCannon.transform.eulerAngles.y + 90 + " z: " + PlayerCannon.transform.eulerAngles.z);
        PlayerCannon.transform.eulerAngles = new Vector3(0, PlayerCannon.transform.eulerAngles.y + 90, 0);
    }
}
