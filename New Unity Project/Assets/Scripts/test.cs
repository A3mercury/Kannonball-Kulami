using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {


	public int boardX;
	public int boardY;
	public int pieceNum;
	private core gameCore;

	// Use this for initialization
	void Start () {
		gameCore = GameObject.Find ("GameCore").GetComponent<core>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown(){
		Debug.Log (boardX);
		Debug.Log (boardY);
		if (gameCore.isValidMove(boardX, boardY)) {
			gameCore.PlacePiece(this);
			gameObject.collider.enabled = false;
		}
	}

    void OnMouseOver()
    {
        if(gameCore.isValidMove(boardX, boardY))
        {
            gameCore.ShowLight(this);
        }
    }
}
