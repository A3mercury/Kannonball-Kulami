using UnityEngine;
using System.Collections;
using System;

public class Network_Manager : MonoBehaviour
{
    bool isOnline = false;
    bool single = false;
    bool beingconnected = false;
    bool playerresponse = false;
    public GUISkin myskin;
    private Rect windowRect = new Rect(0, 50, 200, 400);
    public string userName = "", maxPlayers = "2", port = "21212";
    string playerwantingtoconnect = "";

    private void OnGUI()
    {
        GUI.skin = myskin;
        //windowRect = GUI.Window(0, windowRect, windowFunc, "Players");

        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            if (!isOnline && !single)
            {
                GUILayout.Label("Select type of play:");
                if (GUILayout.Button("Single Player!"))
                {
                    single = true;
                }

                if (GUILayout.Button("Online Multiplayer!"))
                {
                    isOnline = true;
                }
            }
            else if (!isOnline && single)
            {
                GUILayout.Label("Single Player play now started.");
            }
            else if (isOnline && !single)
            {
                GUILayout.Label("Please enter a username:");
                userName = GUILayout.TextField(userName);

                if (GUILayout.Button("Connect to Kannonball Kulami!"))
                {
                    try
                    {
                        Network.InitializeSecurity();
                        Network.InitializeServer(int.Parse(maxPlayers), int.Parse(port), !Network.HavePublicAddress());
                        MasterServer.RegisterHost("Testing KK_Chat", userName);
                    }
                    catch (Exception)
                    {
                        print("Please type in numbers for port and max players");

                    }
                }
            }
        }
        else
        {
            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect();
                isOnline = false;
                single = false;
            }



            windowRect = GUI.Window(0, windowRect, windowFunc, "Players");

        }

    }

    private void windowFunc(int id)
    {
        if (GUILayout.Button("Refresh"))
        {
            MasterServer.RequestHostList("Testing KK_Chat");
        }

        GUILayout.BeginHorizontal();

        GUILayout.Box("Player Name");

        GUILayout.EndHorizontal();

        if (MasterServer.PollHostList().Length != 0)
        {
            HostData[] data = MasterServer.PollHostList();
            foreach (HostData c in data)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(c.gameName);
                if (GUILayout.Button("Connect"))
                {
                    Network.Connect(c);
                    //networkView.RPC("ConnectionRequest", RPCMode.All, userName, true);
                }
                GUILayout.EndHorizontal();
            }
        }
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

    [RPC]
    private void ConnectionRequest(string userName, bool connect)
    {
        Debug.Log(userName);
        GUILayout.Label(userName + "wants to play kulami.  Do you accept?");
        if (GUILayout.Button("Yes"))
        {
            playerresponse = true;
        }
        else if (GUILayout.Button("No"))
        {
            playerresponse = false;
        }

        networkView.RPC("ConnectionResponse", RPCMode.Others, playerresponse);
        playerwantingtoconnect = userName;
        beingconnected = connect;
    }

    [RPC]
    private void ConnectionResponse(bool response)
    {
        playerresponse = response;
    }

}
