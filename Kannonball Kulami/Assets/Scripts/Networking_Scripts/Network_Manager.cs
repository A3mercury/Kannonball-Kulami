using UnityEngine;
using System.Collections;
using System;

public class Network_Manager : MonoBehaviour
{
    public GUISkin myskin;
    public string userName = "", maxPlayers = "2", port = "21212";
    private Rect windowRect = new Rect(0, 100, 200, 400);


    private void OnGUI()
    {
        GUI.skin = myskin;
        windowRect = GUI.Window(0, windowRect, windowFunc, "Players");
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            GUILayout.Label("Please enter your User Name:");
            userName = GUILayout.TextField(userName);

            /* GUILayout.Label("Port");
             port = GUILayout.TextField(port);

             GUILayout.Label("Max Player");
             maxPlayers = GUILayout.TextField(maxPlayers);*/

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
        else
        {
            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect();
            }
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
                }
                GUILayout.EndHorizontal();
            }
        }
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

}
