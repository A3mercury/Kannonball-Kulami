using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class core : MonoBehaviour {

	public Material solid;
	
	public GamePlace[,] gamePlaces;
	public string turn;
	private int redLastCol;
	private int redLastRow;
	private int redLastPiece;
	private int blackLastCol;
	private int blackLastRow;
	private int blackLastPiece;
	private int turnsLeft;
    private int boardSize = 8;

    public ReadGameboard boardReader;

	// Use this for initialization
	void Start () 
    {
        turnsLeft = 56;
		
		turn = "red";

        gamePlaces = new GamePlace[boardSize, boardSize];

        //boardReader = new ReadGameboard(gamePlaces);

        StreamReader reader = new StreamReader("gameBoard.txt");
        string[] pieceNumbers = reader.ReadToEnd().Split(',');
        //Debug.Log (pieceNumbers);
        gamePlaces = new GamePlace[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                GamePlace gp = new GamePlace();
                //Debug.Log(gp);
                //Debug.Log(pieceNumbers[(8 * i) + k]);
                //System.Console.Write(pieceNumbers[(8 * i) + k]);
                gp.pieceNum = int.Parse(pieceNumbers[(8 * i) + k]);
                gp.owner = "open";
                //Debug.Log(gp);
                gamePlaces[i, k] = gp;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                Debug.Log("pieceNum " + gamePlaces[i, k].pieceNum);
                Debug.Log("owner " + gamePlaces[i, k].owner);
            }
            Debug.Log('\n');
        }
	}

	public void PlacePiece (test sender) 
    {
		gamePlaces [sender.boardX, sender.boardY].owner = turn;
        sender.gameObject.renderer.enabled = true;
		sender.gameObject.renderer.material = solid;

		if (turn == "red") 
        {
			sender.gameObject.renderer.material.color = Color.red;
			redLastCol = sender.boardX;
			redLastRow = sender.boardY;
			redLastPiece = sender.pieceNum;
			turn = "black";
		}
        else
        {
			sender.gameObject.renderer.material.color = Color.black;
			blackLastCol = sender.boardX;
			blackLastRow = sender.boardY;
			blackLastPiece = sender.pieceNum;
			turn = "red";
		}

        turnsLeft--;
		char winner = checkForWin ();
	}

	char checkForWin() {
		char winner = '*';
		bool gameOver = false;
		if (turn == "black" && turnsLeft == 0) {
			gameOver = true;
		}
		if (noValidMove())
			gameOver = true;
		return winner;
	}

	bool noValidMove() {
        bool result = true;
		for (int i = 0; i < 8; i++) {
			for (int k = 0; k < 8; k++) {
                if (isValidMove(i, k))
                    result = false;
			}
		}
        return result;
	}

	public bool isValidMove(int x, int y)
    {
		bool result = true;
		Debug.Log (gamePlaces[x, y].pieceNum);
		Debug.Log (redLastPiece);

        if (gamePlaces[x, y].pieceNum == redLastPiece || gamePlaces[x, y].pieceNum == blackLastPiece)
                result = false;

            if (turn == "red" && x != blackLastRow && y != blackLastCol)
                result = false;

            if (turn == "black" && x != redLastRow && y != redLastCol)
                result = false;

            if (gamePlaces[x, y].pieceNum == redLastPiece || gamePlaces[x, y].pieceNum == blackLastPiece)
                result = false;
        
        Debug.Log(result);
		return result;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
