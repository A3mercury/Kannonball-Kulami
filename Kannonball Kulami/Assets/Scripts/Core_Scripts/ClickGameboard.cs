using UnityEngine;
using System.Collections;
using System.Threading;
public class ClickGameboard : MonoBehaviour 
{
    public int boardX;
    public int boardY;
    public int pieceNum;
    private static bool firstMove = true;
    private GameCore gameCore;
    private Network_Manager network;

    private bool gameOver = false;

	// Use this for initialization
	void Start () 
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        Network_Manager network = GetComponent<Network_Manager>();
	}
	
	// Update is called once per frame
	void Update () { }

    void OnMouseDown()
    {
        gameCore.currentRow = boardX;
        gameCore.currentCol = boardY;

		if (!gameCore.GameIsOver)
        {
            if (firstMove)
            {
                if (network.isOnline)
                {
                    network.networkView.RPC("SendMove", RPCMode.All, this);
                    gameObject.collider.enabled = false;
                    firstMove = false;
                }
                else
                {
                    gameCore.PlacePiece(this);
                    gameObject.collider.enabled = false;
                    firstMove = false;
                    gameCore.MakeAIMove();
                }
            }
            else //if (gameCore.isValidMove(boardX, boardY))
            {
                if (gameCore.isValidMove(boardX, boardY))
                {
                    if (network.isOnline)
                    {
                        network.networkView.RPC("SendMove", RPCMode.All, this);
                    }
                    else
                    {
                        gameCore.PlacePiece(this);
                        gameObject.collider.enabled = false;
                    }

					if (gameCore.GameIsOver)
                        gameOver = true;
                    else
                    {
                        gameCore.MakeAIMove();
                    }
                }
            }
        }
        
        // if game is over
        // do game over stuff
        // (call a method in GameCore.cs)

        if(gameOver)
        {
            Debug.Log("Game Over!");
        }

        //Debug.Log("gameOver: " + gameOver);
    }
}
