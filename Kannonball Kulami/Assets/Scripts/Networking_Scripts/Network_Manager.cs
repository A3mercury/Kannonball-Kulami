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
    public bool invoked, disconnected, sentRequest, ingame = false, calledgamescene;
    public static bool chat = false;
    public bool isOnline;
    public bool detecteddisconnect;
    public static bool fromtransition;
    public string serverName;
    public string clientName;
    private int randomBoard = 0;
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
    public GUISkin PopupSkin;

    public GUIStyle OpponentRect;

    // Invitation GUI stuff
    Rect InviteWrapperRect;
    Rect InviteBackgroundRect;

    bool popuptrue = false;

    void Start()
    {
        if(Application.loadedLevelName == "GameScene")
            gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        isOnline = fromtransition;

        ConnectionRequestRect = new Rect((Screen.width - 100) / 2, (Screen.height - 100) / 2, 100, 100);
        randomBoard = UnityEngine.Random.Range(1, 8);
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
        ingame = false;
        invoked = true;
        disconnected = false;
        sentRequest = false;
        calledgamescene = false;
        detecteddisconnect = false;
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
        else if (masterServerEvent == MasterServerEvent.RegistrationFailedNoServer)
            Debug.Log("Rgistration failed because no server is running");
        else if (masterServerEvent == MasterServerEvent.RegistrationFailedGameType)
            Debug.Log("Registration failed because an empty game type was given");
        else if (masterServerEvent == MasterServerEvent.RegistrationFailedGameName)
            Debug.Log("Registration failed because an empty game name was given.");
        else
            Debug.Log("Registration failed");
    }

    void Update()
    {
        if(Network.isServer)
           MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");

        //if (detecteddisconnect)
        //{
        //    Debug.Log("Made it to disconnect.");
        //    messBox = "Unfortunately your opponent has been disconnected.\n You will be taken back to the player list.";
        //    windowRect = GUI.Window(1, windowRect, DisconnectpopUp, "");
        //}
    }
    
    private void OnGUI()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    popuptrue = true;
        //}

        //// FOR TESTING THE POPUP 
        //if (popuptrue)
        //{
        //    GUI.skin = PopupSkin;

        //    InviteWrapperRect = GUI.Window(1, InviteWrapperRect, InvitationPopupWindow, "");

        //}

        // If we are in an online game
        if (isOnline)
        {
            if (!ingame)
            {
                GUI.skin = ServerSkin;

                // If we are not currently connected
                if (Network.peerType == NetworkPeerType.Disconnected)
                {
                    Debug.Log("It has restarted.");
                    ServerWrapperRect = GUI.Window(0, ServerWrapperRect, ServerWindowBeforeConnection, "");
                }
                else
                {
                    // open the server window
                    ServerWrapperRect = GUI.Window(0, ServerWrapperRect, ServerWindowBeforeConnection, "");
                    ServerWrapperRect = GUI.Window(0, ServerWrapperRect, ServerWindowAfterConnection, "");

                    // if we are the server
                    if (Network.isServer)
                    {
                        gameCore.playerColor = "black";
                        networkplayer = 1;
                    }
                    // if we are the client
                    if (Network.isClient && !sentRequest && !disconnected)
                    {
                        Debug.Log("It's here");
                        networkView.RPC("SendConnectionRequest", RPCMode.All, userName, true);
                    }
                    // sent or recieving a invite
                    if (sentRequest && !disconnected)
                    {
                        // if we have been sent an invite
                        if (Network.isServer)
                        {
                            //messBox = clientName + " has challenged you to a game! Do you accept?\n";
                            InviteWrapperRect = GUI.Window(200, InviteWrapperRect, InvitationPopupWindow, "");

                        }
                        else // if we are sending an invite
                        {
                            messBox = "You have challenged " + serverName + " to a game. Awaiting response...\n";
                            windowRect = GUI.Window(1, windowRect, AwaitingResponse, "");
                        }
                        //GUI.skin = PopupSkin;
                        //InviteWrapperRect = GUI.Window(1, InviteWrapperRect, InvitationPopupWindow, "");
                        //windowRect = GUI.Window(1, windowRect, popUp, "");
                    }
                }

                // if game was denied
                if (disconnected && invoked)
                {
                    //Evoke();
                    messBox = "Request has been denied.\n";
                    windowRect = GUI.Window(1, windowRect, popUp, "");
                   // Invoke("Evoke", 3);
                    //Invoke("Disconnect", 3);
                }
            }
            else if(!detecteddisconnect)
            {
                Debug.Log("detecteddisconnect = " + detecteddisconnect);
                if(!calledgamescene)
                {
                    GameObject.FindObjectOfType<CameraGameSceneMovement>().SelectCameraPosition();
                    calledgamescene = true;
                }

                //if (detecteddisconnect)
                //{
                //    Debug.Log("Made it to disconnect.");
                //    messBox = "Unfortunately your opponent has been disconnected.\n You will be taken back to the player list.";
                //    windowRect = GUI.Window(1, windowRect, DisconnectpopUp, "");
                //}
                //if(!Network.isClient && !Network.isServer)
                //{
                //    messBox = "Unfortunately your opponent has been disconnected.\n You will be taken back to the player list.";
                //    windowRect = GUI.Window(1, windowRect, DisconnectpopUp, "");
                //}
            }
            else
            {
                Debug.Log("Made it to disconnect.");
                messBox = "Unfortunately your opponent has been disconnected.\n You will be taken back to the player list.";
                windowRect = GUI.Window(1, windowRect, DisconnectpopUp, "");
            }
        }
        else
            return;
    } 

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        detecteddisconnect = true;
        Debug.Log(player.ipAddress + " has disconnected.");
        //messBox = "Unfortunately your opponent has been disconnected.\n You will be taken back to the player list.";
        //windowRect = GUI.Window(1, windowRect, DisconnectpopUp, "");
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        Debug.Log("Disconnected from server");

        if (Network.isServer)
        { }
        else
        {
            Debug.Log("Made it to nested if");
            Debug.Log(ingame);
            if (ingame)
            {
                detecteddisconnect = true;
                //messBox = "Unfortunately your opponent has been disconnected.\n You will be taken back to the player list.";
                //windowRect = GUI.Window(1, windowRect, DisconnectpopUp, "");
            }
        }
            
        

    }

    public void InvitationPopupWindow(int id)
    {
        GUI.skin = PopupSkin;

        // Wrapper
        InviteWrapperRect = new Rect(
            (Screen.width * (1 - (432f / 1440f))) / 2,
            (Screen.height * (1 - (241f / 900f))) / 2,
            Screen.width * (432f / 1440f), 
            Screen.height * (241f / 900f)
            );

        InviteBackgroundRect = new Rect(0, 0, InviteWrapperRect.width, InviteWrapperRect.height);

        GUILayout.BeginArea(InviteBackgroundRect, GUI.skin.customStyles[0]);

        // Opponent's Name
        Rect OpponentNameRect = new Rect(
            0, 
            (InviteBackgroundRect.height * 5f) / 100, 
            InviteBackgroundRect.width, 
            (InviteBackgroundRect.height * 22f) / 100
            );

        // Wants to battle
        Rect InviteLabel = new Rect(
            0,
            0,
            InviteBackgroundRect.width,
            (InviteBackgroundRect.height * 22f) / 100
            );

        Rect HoldVertical = new Rect(
            0,
            0,
            InviteBackgroundRect.width,
            (InviteBackgroundRect.height * 50f) / 100
            );

        GUILayout.BeginArea(HoldVertical);
        GUILayout.BeginVertical(GUI.skin.customStyles[1]);

        GUILayout.Label(clientName, GUI.skin.customStyles[2]);
        GUILayout.Label("wants to battle!", GUI.skin.customStyles[3]);

        GUILayout.EndVertical();
        GUILayout.EndArea();

        Rect HoldHorizontal = new Rect(
            (InviteBackgroundRect.width * 10f) / 100,
            (InviteBackgroundRect.height * 60f) / 100,
            (InviteBackgroundRect.width * 80f) / 100,
            (InviteBackgroundRect.height * 25f) / 100
            );

        Rect AcceptButton = new Rect(
            (HoldHorizontal.width * 2F) / 100,
            0,
            (HoldHorizontal.width * 45F) / 100,
            (HoldHorizontal.height)
            );

        Rect DenyButton = new Rect(
            (HoldHorizontal.width * 52f) / 100,
            0,
            (HoldHorizontal.width * 45F) / 100,
            (HoldHorizontal.height)
            );

        GUILayout.BeginArea(HoldHorizontal);
        GUILayout.BeginHorizontal(GUI.skin.customStyles[4]);

        if (Network.isServer)
        {
            GUILayout.BeginArea(AcceptButton);
            if(GUILayout.Button("", GUI.skin.customStyles[5]))
            {
                networkView.RPC("RespondtoRequest", RPCMode.All, true, randomBoard);
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(DenyButton);
            if(GUILayout.Button("", GUI.skin.customStyles[6]))
            {
                networkView.RPC("RespondtoRequest", RPCMode.All, false, 0);
            }
            GUILayout.EndArea();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUILayout.EndArea();
    }

    public void Evoke()
    {
        //invoked = false;
        Network.Disconnect();
        MasterServer.UnregisterHost();
        MasterServer.ClearHostList();
    }
    public void Disconnect()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
        ingame = false;
        invoked = true;
        disconnected = false;
        sentRequest = false;
        Debug.Log("Disconnected from masterserver");
        Invoke("Restart", 4);
    }
    public void Restart()
    {
        Network.InitializeServer(int.Parse(maxPlayers), int.Parse(port), !Network.HavePublicAddress());
        MasterServer.RegisterHost("KannonBall_Kulami_HU_Softdev_Team1_2015", userName);
    }
    //public void RequestPopup(int id)
    //{
    //    GUILayout.TextField(userwantingtoconnect);
    //}

    #region server list 

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
                if (c.gameName != userName)
                {

                    if (GUILayout.Button("", GUI.skin.customStyles[11]))
                    {
                        Network.Connect(c);
                        serverName = c.gameName;
                        userwantingtoconnectfromserver = c.gameName;
                        gameCore.playerColor = "red";

                    }
                }
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
        if (!hitConnected)
            userName = GUILayout.TextField(userName, 15, GUI.skin.customStyles[2]).Replace("\n", "");
        else
        {
            GUILayout.TextField(userName, 15, GUI.skin.customStyles[2]).Replace("\n", "");
            
        }
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
                MasterServer.UnregisterHost();
                Debug.Log("You have been disconnected");
            }
            GUILayout.EndArea();
        }
               
        GUILayout.EndArea();
        /////////////////////////////////////// End of Header
    }

    #endregion

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


        GUILayout.Width(300);
        GUILayout.Box(messBox, myStyle);
        GUILayout.BeginHorizontal();


        if (GUILayout.Button("Ok", GUILayout.Width(75)))
        {
            invoked = false;
            StartServer();
            GameObject.FindObjectOfType<CameraGameSceneMovement>().SelectCameraPosition();
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();

    }

    private void DisconnectpopUp(int id)
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

        Debug.Log("disconnection popup is up");
        if (GUILayout.Button("Ok", GUILayout.Width(75)))
        {
            //invoked = false;
            StartServer();
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();

    }

    private void AwaitingResponse(int id)
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
    public void RespondtoRequest(bool response, int board)
    {
        if (response)
        {
            sentRequest = response;
            ingame = true;
            gameCore.MakeGameboard(board);
        }
        else
        {
            sentRequest = response;
            disconnected = true;
        }
    }

    [RPC]
    public void SendMove(int row, int col)
    {
        gameCore.PlacePiece(row, col);
    }
}
