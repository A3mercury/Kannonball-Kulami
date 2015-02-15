using System.Collections;
using System.IO;
using System.Text;

public class ReadGameboard
{
    public GamePlace[,] gamePlaces;
    public int boardSize = 8;

    string[] pieceNumbers;

    private int gameBoardNum;


    public ReadGameboard(GamePlace[,] gamePlaces)
    {
        StreamReader reader = new StreamReader("gameBoard.txt");
        pieceNumbers = reader.ReadToEnd().Split(',');

        //gamePlaces = new GamePlace[boardSize, boardSize];
        this.gamePlaces = gamePlaces;

        readFromFile();

        gamePlaces = this.gamePlaces;
    }

    public void readFromFile()
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
}