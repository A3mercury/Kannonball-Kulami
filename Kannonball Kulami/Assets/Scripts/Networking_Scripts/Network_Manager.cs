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
    /// 
    public bool invoked, disconnected, sentRequest;
    public static bool chat = false;
    public bool isOnline;
    public static bool fromtransition;
    public string serverName;
    public string clientName;
    public static int networkplayer;
    public string userName = "", maxPlayers = "10", port = "21212", userwantingtoconnect = "", userwantingtoconnectfromserver = "";
    private string messBox = "", messageToSend = "", user = "";
    public GUISkin myskin;
 
    private GameCore gameCore;
    public Vector2 scrollPosition = Vector2.zero;

    //GUI for popup box
    private Rect windowRect = new Rect((Screen.width - 250) / 2, (Screen.height - 100) / 2, 250, 100);
    private GUIStyle myStyle;
    private GUIStyle myOtherStyle;

    // Austin's Gui crap
    float serverWindowWidth = 700f / 1440f;
    float serverWindowHeight = 650f / 900f;
    Rect ServerWrapperRect;
    Rect ServerBackground;
    Rect UsernameRect;
    Rect ConnectionRequestRect;
    public GUISkin ServerSkin;
    public GUIStyle connectButton;
    public GUIStyle disconnectButton;
    public GUIStyle inviteButton;
    public GUIStyle OpponentRect;

    void Start()
    {
        if(Application.loadedLevelName == "GameScene")
            gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        isOnline = fromtransition;

        ConnectionRequestRect = new Rect((Screen.width - 100) / 2, (Screen.height - 100) / 2, 100, 100);
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
        //MasterServer.updateRate = 1;
        MasterServer.RegisterHost("KannonBall_Kulami_HU_Softdev_Team1_2015", userName);
        Debug.Log("Restarted");
        invoked = true;
        disconnected = false;
        sentRequest = false;   
    }

    void OnServerInitialized()
    {
        Debug.Log(userName + " joined as Server.");
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
    }

    void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            Debug.Log("Registration was successful.");
    }

    void Update()
    {
        //if(Network.isServer)
           // MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");        
    }
    
    private void OnGUI()
    {
        if (isOnline)
        {
            GUI.skin = ServerSkin;

            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                Debug.Log("It has restarted.");
                ServerWrapperRect = GUI.Window(0, ServerWrapperRect, ServerWindowBeforeConnection, "");
            }
            else
            {
                ServerWrapperRect = GUI.Window(0, ServerWrapperRect, ServerWindowBeforeConnection, "");
                ServerWrapperRect = GUI.Window(0, ServerWrapperRect, ServerWindowAfterConnection, "");


                if (Network.isServer)
                {
                    gameCore.playerColor = "black";
                    networkplayer = 1;
                }
                if (Network.isClient && !sentRequest && !disconnected)
                {
                    Debug.Log("It's here");
                    networkView.RPC("SendConnectionRequest", RPCMode.All, userName, true);
                }
                if (sentRequest && !disconnected)
                {
                    if (Network.isServer)
                    {
                        messBox = clientName + " has challenged you to a game! Do you accept?\n";
                    }
                    else
                    {
                        messBox = "You have challenged " + serverName + " to a game. Awaiting response...\n";
                    }
                    windowRect = GUI.Window(1, windowRect, popUp, "");
                }
                if (disconnected && invoked)
                {
                    messBox = "Request has been denied.\n";
                    windowRect = GUI.Window(1, windowRect, popUp, "");
                    Invoke("Evoke", 3);
                    Invoke("Restart", 4);

                }
            }
        }
        else
            return;
    } 

    public void Evoke()
    {
        invoked = false;
        Network.Disconnect();
        MasterServer.UnregisterHost();
        MasterServer.ClearHostList();
    }
    public void Restart()
    {
        StartServer();
    }
    public void RequestPopup(int id)
    {
        GUILayout.TextField(userwantingtoconnect);
    }

    public void ServerWindowBeforeConnection(int id)
    {
        // Assign the GUI skin
        GUI.skin = ServerSkin;

        // Window Wrapper
        ServerWrapperRect = new Rect(
            (Screen.width * (1 - serverWindowWidth)) / 2,
            (Screen.height * (1 - serverWindowHeight)) / 2,
            Screen.width * serverWindowWidth,
            Screen.height * serverWindowHeight
            );

        // Background image stretched to the wrapper
        ServerBackground = new Rect(0, 0, ServerWrapperRect.width, ServerWrapperRect.height);        

        // background image in customSyles[0]
        GUILayout.BeginArea(ServerBackground, GUI.skin.customStyles[0]);

        InsertHeader(false);

        Rect OpponentListRect = new Rect(
            (ServerBackground.width * 6f) / 100f,
            (ServerBackground.height * 25f) / 100f,
            (ServerBackground.width * 88f) / 100f,
            (ServerBackground.height * 68f) / 100f
            );

        GUILayout.BeginArea(OpponentListRect, GUI.skin.customStyles[9]);
        GUILayout.EndArea();
        GUILayout.EndArea();
    }

    public void ServerWindowAfterConnection(int id)
    {
        // Assign the GUI skin
        GUI.skin = ServerSkin;

        /////////////////////////////////////// Window Wrapper
        ServerWrapperRect = new Rect(
            (Screen.width * (1 - serverWindowWidth)) / 2,
            (Screen.height * (1 - serverWindowHeight)) / 2,
            Screen.width * serverWindowWidth,
            Screen.height * serverWindowHeight
            );

        // Background image stretched to the wrapper
        ServerBackground = new Rect(0, 0, ServerWrapperRect.width, ServerWrapperRect.height);

        // background image in customSyles[0]
        GUILayout.BeginArea(ServerBackground, GUI.skin.customStyles[0]);

        InsertHeader(true);

        /////////////////////////////////////// Opponent's list

        Rect OpponentListRect = new Rect(
            (ServerBackground.width * 6f) / 100f,
            (ServerBackground.height * 25f) / 100f,
            (ServerBackground.width * 88f) / 100f,
            (ServerBackground.height * 68f) / 100f
            );

        GUILayout.BeginArea(OpponentListRect, GUI.skin.customStyles[8]);
        if (GUILayout.Button("Refresh")) // temp button placement
        {
            MasterServer.ClearHostList();
            MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
        }
        //else
        //InvokeRepeating("GetHostList", 0, 60);

        if (MasterServer.PollHostList().Length != 0)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            HostData[] data = MasterServer.PollHostList();
            foreach (HostData c in data)
            //for (int i = 0; i < 5; i++)
            {
                GUILayout.BeginHorizontal(GUI.skin.customStyles[10]);
                GUILayout.Box(c.gameName, GUI.skin.customStyles[10]);
                //if (c.gameName != userName)
                //{

                    if (GUILayout.Button("", GUI.skin.customStyles[11]))
                    {
                        Network.Connect(c);
                        serverName = c.gameName;
                        userwantingtoconnectfromserver = c.gameName;
                        gameCore.playerColor = "red";

                    }
                //}
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        GUILayout.EndArea();
        GUILayout.EndArea();
    }

    void InsertHeader(bool hitConnected)
    {
        /////////////////////////////////////// Header of the server window
        Rect HeaderRect = new Rect(
            (ServerBackground.width * 6f) / 100f,
            (ServerBackground.height * 7f) / 100f,
            (ServerBackground.width * 88f) / 100f,
            (ServerBackground.height * 10f) / 100f
            );
        GUILayout.BeginArea(HeaderRect, GUI.skin.customStyles[1]);
        UsernameRect = new Rect(
            0,
            0,
            (HeaderRect.width * 40f) / 100,
            HeaderRect.height
            );

        // Actual username
        GUILayout.BeginArea(UsernameRect);
        userName = GUILayout.TextField(userName, 15, GUI.skin.customStyles[2]);
        GUILayout.EndArea();



        // connect button
        Rect ConnectButtonRect = new Rect(
            (HeaderRect.width * 48f) / 100,
            0,
            (HeaderRect.width * 25f) / 100,
            HeaderRect.height
            );
        // disconnect button
        Rect DisconnectButtonRect = new Rect(
            (HeaderRect.width * 75f) / 100,
            0,
            ConnectButtonRect.width,
            ConnectButtonRect.height
            );
        
        // dependent on if the user has clicked to start the server
        if (!hitConnected)
        {
            GUILayout.BeginArea(ConnectButtonRect);
            if (GUILayout.Button("", GUI.skin.customStyles[4]))
            {
                if (userName != "")
                {
                    try
                    {
                        StartServer();
                    }
                    catch (Exception)
                    {
                        print("Server could not be started");
                    }
                }
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(DisconnectButtonRect);
            GUILayout.Button("", GUI.skin.customStyles[7]);
            GUILayout.EndArea();
        }
        else
        {
            // Disabled connect button
            GUILayout.BeginArea(ConnectButtonRect);
            GUILayout.Button("", GUI.skin.customStyles[6]);
            GUILayout.EndArea();

            GUILayout.BeginArea(DisconnectButtonRect);
            if (GUILayout.Button("", GUI.skin.customStyles[5]))
            {
                Network.Disconnect();
            }
            GUILayout.EndArea();
        }
               
        GUILayout.EndArea();
        /////////////////////////////////////// End of Header
    }

    public void GetHostList()
    {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
    }

    private void popUp(int id)
    {
        myStyle = GUI.skin.box;
        myStyle.alignment = TextAnchor.UpperLeft;
        myStyle.wordWrap = true;
        myStyle.stretchHeight = true;
        myOtherStyle = GUI.skin.textField;
        myOtherStyle.alignment = TextAnchor.MiddleLeft;
        myOtherStyle.clipping = TextClipping.Clip;
        myOtherStyle.wordWrap = false;
        myOtherStyle.fixedWidth = 200;


        GUILayout.Width(250);
        GUILayout.Box(messBox, myStyle);
        GUILayout.BeginHorizontal();

        if (Network.isServer)
        {
            if (GUILayout.Button("Accept", GUILayout.Width(75)))
            {
                networkView.RPC("RespondtoRequest", RPCMode.All, true);
            }
            if (GUILayout.Button("Deny", GUILayout.Width(75)))
            {
                networkView.RPC("RespondtoRequest", RPCMode.All, false);
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();

    }

   
	
    [RPC]
    private void SendConnectionRequest(string userName, bool request)
    {
        sentRequest = request;
        clientName = userName;
    }

    [RPC]
    public void RespondtoRequest(bool response)
    {
        if(response)
            sentRequest = response;
        else
            sentRequest = response;
            disconnected = true;        
    }

    [RPC]
    public void SendMove(int row, int col)
    {
        gameCore.PlacePiece(row, col);
    }
}
