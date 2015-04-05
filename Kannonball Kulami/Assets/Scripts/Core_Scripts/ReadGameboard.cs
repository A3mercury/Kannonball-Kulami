using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class ReadGameboard
{
    public GamePlace[,] gamePlaces;
    public int boardSize = 8;

    string[] pieceNumbers;

    private int gameBoardNum;

    public ReadGameboard(GamePlace[,] gamePlaces, int gbNum)
    {
        gameBoardNum = gbNum;

        //StreamReader reader = new StreamReader("gameBoard"+gbNum+".txt");
        //pieceNumbers = reader.ReadToEnd().Split(',');
		pieceNumbers = GetGameBoardPieces (gbNum).Split (',');
        //gamePlaces = new GamePlace[boardSize, boardSize];
        this.gamePlaces = gamePlaces;

        PrepGameboard();

        gamePlaces = this.gamePlaces;
    }

    public void PrepGameboard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                GamePlace gp = new GamePlace();

                gp.pieceNum = int.Parse(pieceNumbers[(boardSize * i) + j]);
                gp.owner = "open";

                gamePlaces[i, j] = gp;
            }
        }
    }

    public void Output()
    {
        for (int i = 0; i < boardSize; i++)
            for (int j = 0; j < boardSize; j++)
                Debug.Log("pieceNum: " + gamePlaces[i, j].pieceNum + " owner: " + gamePlaces[i, j].owner);
    }

	public string GetGameBoardPieces(int gb)
	{
		string gbPieces = "";
		switch (gb) 
		{
			case 1:
				gbPieces ="1,1,1,2,2,2,3,4,5,5,6,7,8,9,3,4,5,5,6,7,8,9,3,4,10,10,11,11,12,12,13,13,10,10,11,11,12,12,13,13,14,14,15,15,16,16,17,17,14,14,15,15,16,16,17,17,14,14,15,15,16,16,17,17";
				break;
			case 2:
				gbPieces ="1,1,1,2,2,3,3,3,4,4,5,6,6,6,10,10,4,4,5,7,8,9,10,10,4,4,5,7,8,9,10,10,11,11,12,12,13,13,14,14,11,11,12,12,13,13,14,14,15,15,16,16,16,17,17,17,15,15,16,16,16,17,17,17";
				break;
			case 3:
				gbPieces ="1,1,2,3,3,4,5,5,1,1,2,3,3,4,6,6,1,1,2,9,9,4,6,6,7,7,8,9,9,10,10,11,7,7,8,9,9,10,10,11,12,12,12,14,15,15,17,17,13,13,13,14,15,15,17,17,13,13,13,16,16,16,17,17";
				break;
			case 4:
				gbPieces ="1,1,1,2,3,3,4,4,1,1,1,2,3,3,4,4,5,6,6,2,7,7,4,4,5,6,6,9,7,7,11,12,8,8,8,9,10,10,11,12,8,8,8,15,10,10,11,17,13,13,14,15,16,16,16,17,13,13,14,15,16,16,16,17";
				break;
			case 5:
				gbPieces ="1,1,1,2,2,2,3,3,4,4,5,5,6,6,3,3,4,4,8,8,6,6,9,9,7,7,8,8,12,12,9,9,10,10,10,11,12,12,9,9,10,10,10,11,12,12,17,17,13,13,14,11,15,15,17,17,13,13,14,16,16,16,17,17";
				break;
			case 6:
				gbPieces ="1,1,1,2,2,3,3,7,4,5,5,2,2,6,6,7,4,5,5,2,2,6,6,7,8,8,8,9,9,10,10,10,8,8,8,9,9,10,10,10,11,12,12,14,14,15,15,16,11,12,12,14,14,15,15,16,11,13,13,14,14,17,17,17";
				break;
			case 7:
				gbPieces ="1,1,2,2,2,3,4,4,1,1,2,2,2,3,5,5,6,6,7,8,8,3,5,5,6,6,7,8,8,10,10,10,6,6,9,9,9,10,10,10,11,11,13,13,14,15,16,16,12,12,13,13,14,15,17,17,12,12,13,13,14,15,17,17";
				break;
		}
		return gbPieces;
	}
}