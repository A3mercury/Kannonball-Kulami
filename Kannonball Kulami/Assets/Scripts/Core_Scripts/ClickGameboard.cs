using UnityEngine;
using System.Collections;
using System.Threading;
public class ClickGameboard : MonoBehaviour 
{
	public int row;
    public int col;
    public int pieceNum;
    private static bool firstMove = true;
    private GameCore gameCore;
    private Network_Manager network;

    private bool gameOver = false;

	// Use this for initialization
	void Start () 
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        network = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
	}
	
	// Update is called once per frame
	void Update () { }

    void OnMouseDown()
    {
		gameCore.currentRow = row;
		gameCore.currentCol = col;

		if (!gameCore.GameIsOver)
        {
            if (firstMove)
            {
                if (network.isOnline)
                {
                    network.networkView.RPC("SendMove", RPCMode.All, this);
                    firstMove = false;
                }
                else
                {
                    gameCore.PlacePiece(row, col);
                    firstMove = false;
                    gameCore.MakeAIMove();
                }
            }
            else //if (gameCore.isValidMove(boardX, boardY))
            {
				if (gameCore.isValidMove(row, col))
                {
                    if (network.isOnline)
                    {
                        network.networkView.RPC("SendMove", RPCMode.All, row, col);
                    }
                    else
                    {
                        gameCore.PlacePiece(row, col);
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
