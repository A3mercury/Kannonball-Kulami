using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreCard : MonoBehaviour {
    public static int redScore;
    public static int blackScore;
    public Text scoreText;
	// Use this for initialization
	void Start () {
        redScore = 0;
        blackScore = 0;
	}

    void OnLevelWasLoaded(int index)
    {
        if (index == 5 || index == 6)
        {
            scoreText.text = "Red Score: " + redScore + "\n\nBlack Score: " + blackScore; 
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
