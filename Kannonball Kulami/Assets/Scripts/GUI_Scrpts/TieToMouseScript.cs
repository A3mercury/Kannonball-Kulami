using UnityEngine;
using System.Collections;

public class TieToMouseScript : MonoBehaviour {

    ////////////private float actualDistance;

    float depthIntoScene = 10f;
    GameObject PlayerCannon;

    Camera mainCam;

	// Use this for initialization
	void Start () {
        //////////actualDistance = (transform.position - Camera.main.transform.position).magnitude;
        PlayerCannon = GameObject.Find("PlayerCannon");
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        //////////Vector3 myMousePosition = Input.mousePosition;
        //////////myMousePosition.z = actualDistance;
        //////////transform.position = Camera.main.ScreenToWorldPoint (myMousePosition);

        MoveToMouseAtObjectDepth();
	}

    void MoveToMouseAtObjectDepth()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        // create a ray that goes into the scene from the camera, through the mouse position
        if (mainCam.enabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            float depth;
            RaycastHit hitInfo; // var to store info about the object hit (if any)

            // Check to see if ray hits any objects in scene (namely gameboard pieces)
            // pass in hitInfo, so that Raycast can store the info about the hit there
            // the 'out' keyword is a parameter modifier used to tell C# that this obj should be passed by ref
            //   to properly access hitInfo
            // objets we hope to hit must have a collider component
            if (Physics.Raycast(ray, out hitInfo))
            {
                // move this obj to the position we hit
                this.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);

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
        PlayerCannon.transform.eulerAngles = new Vector3(0, PlayerCannon.transform.eulerAngles.y + 90, 0);
    }
}
