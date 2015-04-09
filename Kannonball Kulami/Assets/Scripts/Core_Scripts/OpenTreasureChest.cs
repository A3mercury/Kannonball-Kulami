using UnityEngine;
using System.Collections;

public class OpenTreasureChest : MonoBehaviour 
{
    public Animator LockPress;
    public Animator OpenChest;

    public GameObject TreasureChest;
    public GameObject Top;
    public GameObject Lock;

    private int numberOfClicksToOpen;
    private int numberClicked;

	// Use this for initialization
	void Start () 
    {
        numberOfClicksToOpen = Random.Range(1, 24);
        numberClicked = 0;
        Debug.Log("number of clicks needed " + numberOfClicksToOpen);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "ChestLock")
                {
                    LockPress.SetTrigger("OpenLockTrigger");
                    numberClicked++;

                    if (numberClicked >= numberOfClicksToOpen)
                        OpenChest.SetTrigger("OpenTopTrigger");
                }
            }

        }


	}


}
