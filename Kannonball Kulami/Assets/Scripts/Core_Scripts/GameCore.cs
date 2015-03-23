using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;


public class GameCore : MonoBehaviour 
{
    //public static bool isOnline;
    public Material solid;
    public MeshRenderer meshRenderer;

    public GamePlace[,] gamePlaces;
    public string turn;
    public string playerColor;
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

    public Camera mainCam;
    public Camera serverCam;
    private Network_Manager networkManager;

	// Use this for initialization
	void Start () 
    {
        Moves = new List<KeyValuePair<int, int>>();

        turnsLeft = 56;

        turn = "red";
        playerColor = "black";

        gamePlaces = new GamePlace[boardSize, boardSize];

        // Gameboard number is sent as second parameter
        boardReader = new ReadGameboard(gamePlaces, 1);

        //boardReader.Output();

        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        serverCam = GameObject.Find("ServerCamera").GetComponent<Camera>();
        networkManager = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
        ChooseCamera();
	}
	
	// Update is called once per frame
	void Update () {
        if (myJob != null)
        {
            if (myJob.Update())
            {
                PlacePiece(myJob.AIChosenMove.Key, myJob.AIChosenMove.Value);
                myJob = null;
            }
        }
    }

    public void PlacePiece(int row, int col)
    {
        if (turn == playerColor)
        {
            HideValidMoves();
        }

        audio.Play();

        Moves.Add(new KeyValuePair<int, int>(row, col));
		string CannonBallObjectString = "CannonBall" + row.ToString() + col.ToString();
		GameObject chosenObject = GameObject.Find(CannonBallObjectString);
        gamePlaces[row, col].owner = turn;
        //gamePlaces[row, col].isValid = false;
		chosenObject.collider.enabled = false;
        chosenObject.renderer.enabled = true;
		chosenObject.renderer.material = solid;

            if (turn == "red")
            {
				chosenObject.renderer.material.color = Color.red;
                redLastRow = row;
                redLastCol = col;
				redLastPiece = gamePlaces[row, col].pieceNum;
                turn = "black";
            }
            else
            {
				chosenObject.renderer.material.color = Color.grey;
                blackLastRow = row;
                blackLastCol = col;
				blackLastPiece = gamePlaces[row, col].pieceNum;
                turn = "red";
            }

            turnsLeft--;
            if (turn == playerColor)
            {
                ShowValidMoves();
            }
            if (isGameOver())
            {
                GameIsOver = true;
				Debug.Log("Game is over!");
				KeyValuePair<int, int> score = GetScore();
				Debug.Log("Red score: " + score.Key);
				Debug.Log("Black score: " + score.Value);
                if ((playerColor == "red" && score.Key > score.Value) || (playerColor == "black" && score.Key < score.Value) )
                {
                    Application.LoadLevel("VictoryScene");
                }
                else
                {
                    Application.LoadLevel("LoseScene");
                }
            }
       
    }

	private struct GamePiece
	{
		public int red;
		public int black;
		public int open;
	};

	public KeyValuePair<int, int> GetScore()
	{
		int red = 0;
		int black = 0;
		GamePiece[] gamePieces = new GamePiece[17];
		for (int i = 0; i < 17; i++) {
			GamePiece piece;
			piece.red = 0;
			piece.black = 0;
			piece.open = 0;
			gamePieces[i] = piece;
		}
		for (int i = 0; i < 8; i++) {
			for (int k = 0; k < 8; k++) {
				if (gamePlaces[i,k].owner == "red") {
					gamePieces[gamePlaces[i,k].pieceNum - 1].red++;
				}
				else if (gamePlaces[i,k].owner == "black") {
					gamePieces[gamePlaces[i,k].pieceNum - 1].black++;
				}
				else if (gamePlaces[i,k].owner == "open") {
					gamePieces[gamePlaces[i,k].pieceNum - 1].open++;
				}
			}
		}
		foreach (GamePiece piece in gamePieces) {
			int total = piece.red + piece.black + piece.open;
			if (piece.red > piece.black) {
				red += total;
			}
			else if (piece.black > piece.red) {
				black += total;
			}
		}
        ScoreCard.redScore = red;
        ScoreCard.blackScore = black;
		return new KeyValuePair<int, int> (red, black);
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

    public bool isValidMove(int row, int col)
    {
      //  bool result = true;
        //Debug.Log(gamePlaces[x, y].pieceNum);
        //Debug.Log("last red piece: " + redLastPiece);
        //Debug.Log("last black piece: " + blackLastPiece);

		if (gamePlaces[row, col].owner != "open")
			return false;

        // if not the last 2 board pieces
		if (gamePlaces[row, col].pieceNum == redLastPiece || gamePlaces[row, col].pieceNum == blackLastPiece)
            return false;

		if (turn == "red" && row != blackLastRow && col != blackLastCol)
            return false;

		if (turn == "black" && row != redLastRow && col != redLastCol)
            return false;


        //Debug.Log("gamePlace[" + x + ", " + y + "]" + " | valid: " + gamePlaces[x, y].isValid);

        return true;
    }

    public void MakeAIMove()
    {
            myJob = new AIJob();
            myJob.AIMoveArray = new KeyValuePair<int, int>[Moves.Count];
            for (var i = 0; i < Moves.Count; i++)
            {
                myJob.AIMoveArray[i] = Moves[i];
            }
            myJob.Start();
        
    }

    public void PlaceAIMove()
    {
        
            KeyValuePair<int, int> AIChosenMove = myJob.AIChosenMove;
            string CannonBallObjectString = "CannonBall" + AIChosenMove.Key.ToString() + AIChosenMove.Value.ToString();
            GameObject chosenObject = GameObject.Find(CannonBallObjectString);
            Moves.Add(new KeyValuePair<int, int>(AIChosenMove.Key, AIChosenMove.Value));
            gamePlaces[AIChosenMove.Key, AIChosenMove.Value].owner = turn;
            //gamePlaces[AIChosenMove.Key, AIChosenMove.Value].isValid = false;
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

    public void ShowValidMoves()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                if (isValidMove(i, j))
                {
                    string CannonBallObjectString = "CannonBall" + i.ToString() + j.ToString();
                    GameObject chosenObject = GameObject.Find(CannonBallObjectString);
                    chosenObject.renderer.enabled = true;
                    chosenObject.renderer.material = solid;
                    chosenObject.renderer.material.color = Color.white;
                }
            }
        }
    }

    public void HideValidMoves()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                    string CannonBallObjectString = "CannonBall" + i.ToString() + j.ToString();
                    GameObject chosenObject = GameObject.Find(CannonBallObjectString);
                    if (chosenObject.renderer.material.color == Color.white)
                    {
                        chosenObject.renderer.enabled = false;
                        chosenObject.renderer.material = solid;
                        chosenObject.renderer.material.color = Color.white;
                    }
            }
        }

    }

    public void ChooseCamera()
    {
        //if(networkManager.isOnline)
        //{
        //    serverCam.enabled = true;
        //    mainCam.enabled = false;
        //}
        //else
        //{
        serverCam.enabled = false;
        mainCam.enabled = true;
        //}
    }
}
