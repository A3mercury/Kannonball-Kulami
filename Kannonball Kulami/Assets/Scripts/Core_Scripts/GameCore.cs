using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class GameCore : MonoBehaviour 
{
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
    private int turnsLeft;
    private int boardSize = 8;

    public ReadGameboard boardReader;

	// Use this for initialization
	void Start () 
    {
        turnsLeft = 56;

        turn = "red";

        gamePlaces = new GamePlace[boardSize, boardSize];

        boardReader = new ReadGameboard(gamePlaces, 5);

        boardReader.Output();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void PlacePiece(ClickGameboard sender)
    {
        gamePlaces[sender.boardX, sender.boardY].owner = turn;
        sender.gameObject.renderer.enabled = true;
        sender.gameObject.renderer.material = solid;

        if(turn == "red")
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

        // TODO: check for winner
    }

    public bool isValidMove(int x, int y)
    {
        bool result = true;
        Debug.Log(gamePlaces[x, y].pieceNum);
        //Debug.Log(redLastPiece);

        if (gamePlaces[x, y].pieceNum == redLastPiece || gamePlaces[x, y].pieceNum == blackLastPiece)
            result = false;

        if (turn == "red" && x != blackLastRow && y != blackLastCol)
            result = false;

        if (turn == "black" && x != redLastRow && y != redLastCol)
            result = false;

        if (gamePlaces[x, y].pieceNum == redLastPiece || gamePlaces[x, y].pieceNum == blackLastPiece)
            result = false;

        Debug.Log("isValidMove: " + result);
        return result;
    }
}
