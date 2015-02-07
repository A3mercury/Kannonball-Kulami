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

	// Use this for initialization
	void Start () {
        turnsLeft = 56;
		
		turn = "red";
		StreamReader reader = new StreamReader ("gameBoard.txt");
		string[] pieceNumbers = reader.ReadToEnd().Split(',');
		//Debug.Log (pieceNumbers);
		gamePlaces = new GamePlace[8,8];
		for (int i = 0; i < 8; i++) {
			for (int k = 0; k < 8; k++) {
				GamePlace gp = new GamePlace();
				//Debug.Log(gp);
				Debug.Log(pieceNumbers[(8 * i) + k]);
				//System.Console.Write(pieceNumbers[(8 * i) + k]);
				gp.pieceNum = int.Parse(pieceNumbers[(8 * i) + k]);
				gp.owner = "open";
				//Debug.Log(gp);
				gamePlaces[i,k] =  gp;
			}
		}
		for (int i = 0; i < 8; i++) {
			for (int k = 0; k < 8; k++) {
				Debug.Log(gamePlaces[i,k].pieceNum);
				Debug.Log(gamePlaces[i,k].owner);
			}
			Debug.Log('\n');
		}
	}

	public void PlacePiece (test sender) {
		gamePlaces [sender.boardX, sender.boardY].owner = turn;
        sender.gameObject.renderer.enabled = true;
		sender.gameObject.renderer.material = solid;
		if (turn == "red") {
			sender.gameObject.renderer.material.color = Color.red;
			redLastCol = sender.boardX;
			redLastRow = sender.boardY;
			redLastPiece = sender.pieceNum;
			turn = "black";
		} else {
			sender.gameObject.renderer.material.color = Color.black;
			blackLastCol = sender.boardX;
			blackLastRow = sender.boardY;
			blackLastPiece = sender.pieceNum;
			turn = "red";
		}
        turnsLeft--;
		char winner = checkForWin ();
	}

    public void ShowLight(test sender)
    {
        //Light light = GameObject.Find("TestLight").GetComponent<Light>();

        //light.light.intensity = 5;
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

	public bool isValidMove(int x, int y){
		bool result = true;
		Debug.Log (gamePlaces [y, x].pieceNum);
		Debug.Log (redLastPiece);

            if (gamePlaces[y, x].pieceNum == redLastPiece ||
                gamePlaces[y, x].pieceNum == blackLastPiece)
            {
                result = false;
            }
            if (turn == "red" && x != blackLastCol && y != blackLastRow)
                result = false;

            if (turn == "black" && x != redLastCol && y != redLastRow)
                result = false;

            if (gamePlaces[y, x].pieceNum == redLastPiece ||
                gamePlaces[y, x].pieceNum == blackLastPiece)
            {
                result = false;
            }
        
        Debug.Log(result);
		return result;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
