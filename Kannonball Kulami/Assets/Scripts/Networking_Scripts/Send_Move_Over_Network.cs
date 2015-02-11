using UnityEngine;
using System.Collections;

public class Send_Move_Over_Network : MonoBehaviour 
{
    private int x, y;

    [RPC]
    private void SendMove(int movex, int movey)
    {
        x = movex;
        y = movey;
    }
}
