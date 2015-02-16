using UnityEngine;
using System.Collections;

public class ClickGameboard : MonoBehaviour 
{
    public int boardX;
    public int boardY;
    public int pieceNum;
    private bool firstMove;
    private GameCore gameCore;

	// Use this for initialization
	void Start () 
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        firstMove = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnMouseDown()
    {
        Debug.Log("x: " + boardX + " y: " + boardY);

        if(firstMove)
        {
            gameCore.PlacePiece(this);
            gameObject.collider.enabled = false;
            firstMove = false;
        }
        else if(gameCore.isValidMove(boardX, boardY))
        {
            gameCore.PlacePiece(this);
            gameObject.collider.enabled = false;
        }
    }
}
