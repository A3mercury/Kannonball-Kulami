﻿using UnityEngine;
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

    private bool gameOver;

	static bool isClickable;

	// Use this for initialization
	void Start () 
    {
		firstMove = true;
        gameOver = false;

		row = int.Parse(gameObject.name[10].ToString());
		col = int.Parse(gameObject.name[11].ToString());
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        network = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();

		isClickable = true;
	}
	
	// Update is called once per frame
	void Update () {
		isClickable = gameCore.isClickable;
	}

    void OnMouseDown()
    {
		if (isClickable) {
			gameCore.currentRow = row;
			gameCore.currentCol = col;
			//Debug.Log(gameCore.playerColor);
			if (!gameCore.GameIsOver && ("black" == gameCore.turn)) {
				if (firstMove && OptionsMenuTT.PlayerGoesFirst) {
					if (network.isOnline) {
						network.networkView.RPC ("SendMove", RPCMode.All, row, col);
						firstMove = false;
					} else {
						gameCore.PlacePiece (row, col);
						firstMove = false;
						gameCore.MakeAIMove ();
					}
				} else {
					if (gameCore.isValidMove (row, col)) {
						if (network.isOnline) {
							network.networkView.RPC ("SendMove", RPCMode.All, row, col);
							if (gameCore.GameIsOver)
								gameOver = true;
						} else {
							gameCore.PlacePiece (row, col);
							if (gameCore.GameIsOver)
								gameOver = true;
							else {
								gameCore.MakeAIMove ();
							}
						}
					}
				}
			}

			// if game is over
			// do game over stuff
			// (call a method in GameCore.cs)

			if (gameOver) {
				Debug.Log ("Game Over!");
			}

			//Debug.Log("gameOver: " + gameOver);
		} 
    }

	public void ToggleClickability () 
	{
		if (isClickable == true) 
		{
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
