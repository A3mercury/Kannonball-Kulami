using UnityEngine;
using System.Collections;
using System;

public class Network_Manager : MonoBehaviour {

    /// <summary>
    /// //////////////////////////////////////////////////////Variable Declaration////////////////////////////////////////////////////////////
    /// -isOnline- is recieved from the menu screen when the player chooses to play single player or multiplayer.  
    /// -sendrequest- is used to call a client function that sends a connection request to the host player.  
    /// -prompt- is used to activate the Connection Request that the player on the receiving end will see.  
    /// -beingconnectedto- tells the receiving player that a connection request is incoming.
    /// -waitingforresponse- tells the requesting player when the request has been answered. 
    /// -isconnected- is used only for testing purposes.
    /// -userName- is your name when you sign in.
    /// -maxPlayers- determines the limit of players online.
    /// -port- is the port number that the game is listening through.
    /// -userwantingtoconnect- retreives the name of the requesting player.
    /// -userwantingtoconnectfromserver- sends the requesting player back the receiving player's name.
    /// -myskin- is used for testing GUI at this point in development 2/24/2015
    /// -windowRect- defines a new window. 
    /// </summary>

    public static bool chat = false;
    public bool isOnline;
    public bool isInGame = false;
    public static bool fromtransition;
    public string serverName;
    public string clientName;
    public static int networkplayer;
    bool sendrequest = false, prompt = false, beingconnectedto = false, respondtorequest = false, waitingforresponse = false, isconnected = false;
    public string userName = "", maxPlayers = "10", port = "21212", userwantingtoconnect = "", userwantingtoconnectfromserver = "";
    public GUISkin myskin;
    private Rect windowRect = new Rect(0, 43, 200, 200);
    private GameCore gameCore;
    public Vector2 scrollPosition;

    public NetworkLoginInterface login;

    void Start()
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        isOnline = fromtransition;
    }

	
    /// <summary>
    /// ///////////////////////////////////////////////////Server initialization and connection////////////////////////////////////////////////
    /// StartServer() establishes Unity's security methods and creates the server using the maxPlayers, port number, and setting an address.
    /// It will also create a connection to the reserved game on the server which will be named "KannonBall_Kulami_HU_Softdev_Team1_2015."
    /// OnServerInitialized() and OnMasterServerEvent() are both used for testing to show that a player was connected and that their userName
    /// was successfully registered. 
    /// </summary>

    public void StartServer()
    {
        //Network.InitializeSecurity();
        Network.InitializeServer(int.Parse(maxPlayers), int.Parse(port), !Network.HavePublicAddress());
        MasterServer.RegisterHost("testkannonball", userName);
    }

    void OnServerInitialized()
    {
        Debug.Log(userName + " joined as Server.");
    }

    void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            Debug.Log("Registration was successful.");
    }

    /*
    private void OnGUI()
    {
        if (isOnline)
        {
            if(Network.peerType == NetworkPeerType.Disconnected)
            {
                GUILayout.Label("Please enter your User Name:");
                userName = GUILayout.TextField(userName);

                if(GUILayout.Button("Connect to Kannonball Kulami!"))
                {
                    try
                    {
                        StartServer();
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
                windowRect = GUI.Window(0, windowRect, windowFunc, "Players");
            }

            if(Network.isServer)
            {
                //if (isconnected)
                    //chat = true;
                gameCore.playerColor = "black";
                networkplayer = 1;
            }
            if(Network.isClient && !isInGame)
            {
                networkView.RPC("OnChallenge", RPCMode.All, userwantingtoconnectfromserver, userName);
           }

        }
        else
            return;
    }

    
   public void windowFunc(int id)
    {
        if (GUILayout.Button("Refresh"))
        {
            MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
        }
        

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.Box("Player Name");

        GUILayout.EndScrollView();

        if (MasterServer.PollHostList().Length != 0)
        {
            HostData[] data = MasterServer.PollHostList();
            foreach (HostData c in data)
            {
                GUILayout.BeginHorizontal();
                //if (c.gameName != userName)
                //{
                    GUILayout.Box(c.gameName);
                    if (GUILayout.Button("Connect"))
                    {
                        Network.Connect(c);
                        sendrequest = true;
                        userwantingtoconnectfromserver = c.gameName;
                        isconnected = true;
                        networkView.RPC("OnChallenge", RPCMode.All);
                        gameCore.playerColor = "red";
                        
                    }
                //}
                GUILayout.EndHorizontal();

            }
        }
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }*/
	
	[RPC]
    private void SendConnectionRequest(string userName, bool request, bool message)
    { }

    [RPC]
    private void RespondtoRequest(bool response)
    {
        isconnected = response;
    }

   [RPC]
   public void SendMove(int row, int col)
   {
       gameCore.PlacePiece(row, col);
   }

    [RPC]
    public void OnChallenge(string sName, string cName)
   {
       Debug.Log("OnChallenge");
       serverName = sName;
       clientName = cName;
       isInGame = true;
   }
}
