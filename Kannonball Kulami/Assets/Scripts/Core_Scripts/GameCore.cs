using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class GameCore : MonoBehaviour 
{
    //public static bool isOnline;
    public Material solid;
    public MeshRenderer meshRenderer;

	public ParticleSystem playerCannonSmoke;
	public ParticleSystem opponentCannonSmoke;

	public GameObject cannonBallToFire;
	public float fireSpeed;

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
    public int turnsLeft;
    private int boardSize = 8;
    public bool GameIsOver;
	private bool assistanceOn;
    public List<KeyValuePair<int, int>> Moves;
    public AIJob myJob;
	public int currentBoard;
	private List<KeyValuePair<int, int>> MovesBlockedByOptions;
	private bool EasyAI;
	private bool CannonballsInitialized = false;

	public GameObject AIMove;

    public ReadGameboard boardReader;

    public Camera mainCam;
    public GameObject CameraLookat1;
    public GameObject CameraLookat2;

    public Camera serverCam;
    private Network_Manager networkManager;
	private AICannonScript aiCannon;

	public bool isClickable;

    public Canvas GameSceneCanvas;
    public GameObject ChatBoxPanel;

    // materials for pieces
    public Material BlackPiece;
    public Material BlackLastPiece;
    public Material RedPiece;
    public Material RedLastPiece;

	public GameObject[][] Cannonballs;

	// Use this for initialization
	void Start () 
    {
		isClickable = true;
        networkManager = GameObject.FindObjectOfType<Network_Manager>();
		aiCannon = GameObject.FindObjectOfType<AICannonScript>();
        Debug.Log("Gamecore here");
        Debug.Log("Gamecore isOnline " + networkManager.isOnline);
        //if (!networkManager.isOnline )
        //{
        int rand = Random.Range(1, 8);
        if (!networkManager.isOnline) 
		{
			MakeGameboard (rand);
		}

            networkManager.randomBoard = rand;
        //}

		GameIsOver = false;
		Moves = new List<KeyValuePair<int, int>> ();
		MovesBlockedByOptions = new List<KeyValuePair<int, int>> ();

		if (OptionsMenuTT.AIDifficulty == "Easy") 
		{
			EasyAI = true;
		} 
		else 
		{
			EasyAI = false;
		}
	

        turnsLeft = 56;
        if (OptionsMenuTT.PlayerGoesFirst)
        {
            turn = "black";
        }
        else
        {
            turn = "red";
        }

        gamePlaces = new GamePlace[boardSize, boardSize];

        // Gameboard number is sent as second parameter
        if (!networkManager.isOnline)
        {
            boardReader = new ReadGameboard(gamePlaces, currentBoard);
            InitializeCannonballs();
        }

        //boardReader.Output();

        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();

    }
	
    public void InitializeCannonballs()
    {
        Cannonballs = new GameObject[8][];
        for (int i = 0; i < 8; i++)
        {
            Cannonballs[i] = new GameObject[8];
            for (int j = 0; j < 8; j++)
            {
                string CannonBallObjectString = "CannonBall" + i.ToString() + j.ToString();
                GameObject chosenObject = GameObject.Find(CannonBallObjectString);
                Cannonballs[i][j] = chosenObject;
            }
        }
		CannonballsInitialized = true;
        Debug.Log("Done with cannonballs");

        assistanceOn = OptionsMenuTT.isAssistanceChecked;
        if (OptionsMenuTT.PlayerGoesFirst)
        {
            if (assistanceOn)
            {
                ShowValidMoves();
            }
        }
        else if (!networkManager.isOnline)
        {
            MakeAIMove();
        }
    }

	// Update is called once per frame
	void Update () {
        if (myJob != null)
        {
            if (myJob.Update())
            {
				AIMove = Cannonballs[myJob.AIChosenMove.Key][myJob.AIChosenMove.Value];
                //PlacePiece(myJob.AIChosenMove.Key, myJob.AIChosenMove.Value);
				aiCannon.MoveCannonAndFire(AIMove.transform.position, myJob.AIChosenMove.Key, myJob.AIChosenMove.Value);
                myJob = null;
            }
        }

        if (turn == "black" && OptionsMenuTT.isAssistanceChecked && !assistanceOn && CannonballsInitialized)
		{
			ShowValidMoves ();
			assistanceOn = true;
		} 
		else if (turn == "black" && !OptionsMenuTT.isAssistanceChecked && assistanceOn && CannonballsInitialized) 
		{
			ShowValidMoves();
			HideValidMoves();
			assistanceOn = false;
		}

		if (MovesBlockedByOptions.Count > 0 && isClickable) 
		{
			PlacePiece(MovesBlockedByOptions[0].Key, MovesBlockedByOptions[0].Value);
			MovesBlockedByOptions.Clear();
		}
    }

    public void MakeGameboard(int boardNum)
    {
        Instantiate(Resources.Load("GameScene Prefabs/Gameboards/Gameboard " + boardNum.ToString()));
		currentBoard = boardNum;
    }

	public void PlacePhysical(int row, int col, string turn)
	{
		Cannonballs[row][col].renderer.enabled = true;
		if (turn == "black")
		{
			// get textures for cannonballs
			if(assistanceOn)
			{
				Cannonballs[row][col].renderer.material = BlackLastPiece;
			}
			else 
			{
				Cannonballs[row][col].renderer.material = BlackPiece;
			}
		}
		else
		{
			if(assistanceOn)
			{
				Cannonballs[row][col].renderer.material = RedLastPiece;
			}
			else 
			{
				Cannonballs[row][col].renderer.material = RedPiece;
			}
		}
		Cannonballs[row][col].rigidbody.AddForceAtPosition (new Vector3 (0f, -250f, 0f), Cannonballs[row][col].rigidbody.worldCenterOfMass);
	}

    public void PlacePiece(int row, int col)
    {
		if (isClickable) {
			HideValidMoves ();

			//audio.Play();
			CannonFireSound.Instance.FireCannon ();

			Moves.Add (new KeyValuePair<int, int> (row, col));
			gamePlaces [row, col].owner = turn;
			//gamePlaces[row, col].isValid = false;
			Cannonballs[row][col].collider.enabled = false;


			if (turn == "black") 
			{
				GameObject shot = Instantiate(cannonBallToFire, playerCannonSmoke.transform.position, Quaternion.identity) as GameObject;
				if(assistanceOn)
				{
					shot.renderer.material = BlackLastPiece;
				}
				else
				{
					shot.renderer.material = BlackPiece;
				}
				//shot.rigidbody.velocity = ((Cannonballs[row][col].transform.position - shot.transform.position).normalized * 500);
				shot.GetComponent<FireAt>().set(row, col, turn, Cannonballs[row][col], fireSpeed, this);
				Cannonballs[row][col].renderer.enabled = false;
				Cannonballs[row][col].tag = "Player";
				blackLastRow = row;
				blackLastCol = col;
				blackLastPiece = gamePlaces [row, col].pieceNum;
				turn = "red";
				//CannonParticleFire.Instance.CreateParticles ("PlayerParticleObject");
				playerCannonSmoke.Play();
			} 
			else 
			{
				GameObject shot = Instantiate(cannonBallToFire, opponentCannonSmoke.transform.position, Quaternion.identity) as GameObject;
				if(assistanceOn)
				{
					shot.renderer.material = RedLastPiece;
				}
				else
				{
					shot.renderer.material = RedPiece;
				}
				//shot.rigidbody.velocity = ((Cannonballs[row][col].transform.position - shot.transform.position).normalized * 500);
				shot.GetComponent<FireAt>().set(row, col, turn, Cannonballs[row][col], fireSpeed, this);
				Cannonballs[row][col].renderer.enabled = false;	
				Cannonballs[row][col].tag = "Opponent";
				redLastRow = row;
				redLastCol = col;
				redLastPiece = gamePlaces [row, col].pieceNum;
				turn = "black";

				//CannonParticleFire.Instance.CreateParticles ("OpponentParticleObject");
				opponentCannonSmoke.Play();
			}

			turnsLeft--;
			ShowValidMoves ();

			if (isGameOver ()) {
				GameIsOver = true;
				Debug.Log ("Game is over!");
				KeyValuePair<int, int> score = GetScore ();
				Debug.Log ("Red score: " + score.Key);
				Debug.Log ("Black score: " + score.Value);
				if ( score.Key < score.Value) {
					GameIsOver = false;
					Application.LoadLevel ("VictoryScene");
				} else {
					GameIsOver = false;
					Application.LoadLevel ("LoseScene");
				}
			}


		} 
		else if (turn != "black") 
		{
			MovesBlockedByOptions.Add(new KeyValuePair<int, int>(row, col));
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
        if (turnsLeft == 0)
            return true;
        return noValidMoves();
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
		if (turnsLeft == 56) 
			return true;

		if (gamePlaces[row, col].owner != "open")
			return false;

        // if not the last 2 board pieces
		if (gamePlaces[row, col].pieceNum == redLastPiece || gamePlaces[row, col].pieceNum == blackLastPiece)
            return false;

		if (turn == "red" && row != blackLastRow && col != blackLastCol)
            return false;

		if (turn == "black" && row != redLastRow && col != redLastCol)
            return false;

        return true;
    }

    public void MakeAIMove()
    {
		myJob = new AIJob();
        myJob.AIMoveArray = new KeyValuePair<int, int>[Moves.Count];
		myJob.AIBoard = currentBoard;
		myJob.UseEasyAI = EasyAI;
        for (var i = 0; i < Moves.Count; i++)
        {
            myJob.AIMoveArray[i] = Moves[i];
        }
		myJob.Start();
        
    }


    public void ShowValidMoves()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                if (isValidMove(i, j) && turn == "black")
                {
					Cannonballs[i][j].renderer.enabled = OptionsMenuTT.isAssistanceChecked;
					if (OptionsMenuTT.isAssistanceChecked)
                    {
						Cannonballs[i][j].renderer.material = solid;
						Cannonballs[i][j].renderer.material.color = Color.white;
                    }
                }
            }
        }
		if (turnsLeft < 56 && OptionsMenuTT.isAssistanceChecked)
		{
			Cannonballs[blackLastRow][blackLastCol].renderer.material = BlackLastPiece;
			//Cannonballs[blackLastRow][blackLastCol].renderer.enabled = true;
			if(turnsLeft < 55)
			{				
				Cannonballs[redLastRow][redLastCol].renderer.material = RedLastPiece;
				//Cannonballs[redLastRow][redLastCol].renderer.enabled = true;			
			}
		}
    }

    public void HideValidMoves()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
				if (Cannonballs[i][j].tag == "Untagged")
                {
					Cannonballs[i][j].renderer.enabled = false;
					Cannonballs[i][j].renderer.material = solid;
					Cannonballs[i][j].renderer.material.color = Color.white;
                }
            }
        }
		if (turnsLeft < 56) 
		{
			Cannonballs[blackLastRow][blackLastCol].renderer.material = BlackPiece;
			//Cannonballs[blackLastRow][blackLastCol].renderer.enabled = true;			
			if (turnsLeft < 55) 
			{
				Cannonballs[redLastRow][redLastCol].renderer.material = RedPiece;
				//Cannonballs[redLastRow][redLastCol].renderer.enabled = true;				
			}
		}

    }

	public void ToggleClickability () 
	{

		if (isClickable == true) {
			isClickable = false;
			Debug.Log("Board not clickable.");
		} 
		else 
		{
			isClickable = true;
			Debug.Log("Board is clickable");
		}
		
	}
}
