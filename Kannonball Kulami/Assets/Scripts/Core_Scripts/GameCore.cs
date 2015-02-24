using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// I added several if statements that will turn the AI off if networkplay is detected.  Also there is an if statement in the 
/// PlacePiece function again for handling network play. 
/// </summary>
public class GameCore : MonoBehaviour 
{
    //public static bool isOnline;
    public Material solid;
    public MeshRenderer meshRenderer;

    public GamePlace[,] gamePlaces;
    public string turn;
    private int redLastCol;
    private int redLastRow;
    private int redLastPiece;
    private int blackLastCol;
    private int blackLastRow;
    private int blackLastPiece;
    public int currentRow;
    public int currentCol;
    private int turnsLeft;
    private int boardSize = 8;
	public bool GameIsOver = false;
    public List<KeyValuePair<int, int>> Moves;
    public AIJob myJob;

    public ReadGameboard boardReader;

	// Use this for initialization
	void Start () 
    {
        Moves = new List<KeyValuePair<int, int>>();

        turnsLeft = 56;

        turn = "red";

        gamePlaces = new GamePlace[boardSize, boardSize];

        // Gameboard number is send as second parameter
        boardReader = new ReadGameboard(gamePlaces, 1);

        boardReader.Output();

        for (int i = 0; i < boardSize; i++)
            for (int j = 0; j < boardSize; j++)
                gamePlaces[i, j].isValid = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (myJob != null)
        {
            if (myJob.Update())
            {
                PlaceAIMove();
                myJob = null;
            }
        }
    }

    public void PlacePiece(ClickGameboard sender)
    {
        int networkplayer = 0;

        if(Network_Manager.isOnline)
        {
            networkplayer = 1;
        }

        Moves.Add(new KeyValuePair<int, int>(sender.boardX, sender.boardY));
        gamePlaces[sender.boardX, sender.boardY].owner = turn;
        gamePlaces[sender.boardX, sender.boardY].isValid = false;
        sender.gameObject.renderer.enabled = true;
        sender.gameObject.renderer.material = solid;

        if (Network_Manager.isOnline)
        {
            if (networkplayer == 1)
            {
                sender.gameObject.renderer.material.color = Color.red;
                redLastRow = sender.boardX;
                redLastCol = sender.boardY;
                redLastPiece = sender.pieceNum;
                networkplayer = 2;
            }
            else
            {
                sender.gameObject.renderer.material.color = Color.black;
                blackLastRow = sender.boardX;
                blackLastCol = sender.boardY;
                blackLastPiece = sender.pieceNum;
                networkplayer = 1;
            }

            turnsLeft--;
            if (isGameOver())
            {
                GameIsOver = true;
            }
        }
        else
        {

            if (turn == "red")
            {
                sender.gameObject.renderer.material.color = Color.red;
                redLastRow = sender.boardX;
                redLastCol = sender.boardY;
                redLastPiece = sender.pieceNum;
                turn = "black";
            }
            else
            {
                sender.gameObject.renderer.material.color = Color.black;
                blackLastRow = sender.boardX;
                blackLastCol = sender.boardY;
                blackLastPiece = sender.pieceNum;
                turn = "red";
            }

            turnsLeft--;
            if (isGameOver())
            {
                GameIsOver = true;
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////
    }

    public bool isGameOver()
    {
    //    bool gameOver = false;

        if (turnsLeft == 0)
            return true;
        else if (noValidMoves())
            return true;

        return false;
    }

    // return true if there are no valid moves left on the board
    public bool noValidMoves()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for(int col = 0; col < boardSize; col++)
            {
               // if (row != currentRow || col != currentCol)
                //{
                    //if (row == currentRow || col == currentCol)
                        if (isValidMove(row, col))
							return false;
                //}
            }
        }

        return true;
    }

    public bool isValidMove(int x, int y)
    {
      //  bool result = true;
        //Debug.Log(gamePlaces[x, y].pieceNum);
        //Debug.Log("last red piece: " + redLastPiece);
        //Debug.Log("last black piece: " + blackLastPiece);

        // if not the last 2 board pieces
        if (gamePlaces[x, y].pieceNum == redLastPiece || gamePlaces[x, y].pieceNum == blackLastPiece)
            return false;

        if (turn == "red" && x != blackLastRow && y != blackLastCol)
            return false;

        if (turn == "black" && x != redLastRow && y != redLastCol)
            return false;

        if (!gamePlaces[x, y].isValid)
            return false;

        //Debug.Log("gamePlace[" + x + ", " + y + "]" + " | valid: " + gamePlaces[x, y].isValid);

        return true;
    }


    public void MakeAIMove()
    {
        if (!Network_Manager.isOnline)
        {
            myJob = new AIJob();
            myJob.AIMoveArray = new KeyValuePair<int, int>[Moves.Count];
            for (var i = 0; i < Moves.Count; i++)
            {
                myJob.AIMoveArray[i] = Moves[i];
            }
            myJob.Start();
        }
    }


    public void PlaceAIMove()
    {
        if (!Network_Manager.isOnline)
        {
            KeyValuePair<int, int> AIChosenMove = myJob.AIChosenMove;
            string CannonBallObjectString = "CannonBall" + AIChosenMove.Key.ToString() + AIChosenMove.Value.ToString();
            GameObject chosenObject = GameObject.Find(CannonBallObjectString);
            Moves.Add(new KeyValuePair<int, int>(AIChosenMove.Key, AIChosenMove.Value));
            gamePlaces[AIChosenMove.Key, AIChosenMove.Value].owner = turn;
            gamePlaces[AIChosenMove.Key, AIChosenMove.Value].isValid = false;
            chosenObject.gameObject.renderer.enabled = true;
            chosenObject.gameObject.renderer.material = solid;

            if (turn == "red")
            {
                chosenObject.gameObject.renderer.material.color = Color.red;
                redLastRow = AIChosenMove.Key;
                redLastCol = AIChosenMove.Value;
                redLastPiece = gamePlaces[AIChosenMove.Key, AIChosenMove.Value].pieceNum;
                turn = "black";
            }
            else
            {
                chosenObject.gameObject.renderer.material.color = Color.black;
                blackLastRow = AIChosenMove.Key;
                blackLastCol = AIChosenMove.Value;
                blackLastPiece = gamePlaces[AIChosenMove.Key, AIChosenMove.Value].pieceNum;
                turn = "red";
            }
            turnsLeft--;
            if (isGameOver())
            {
                GameIsOver = true;
            }
        }
    }

}
