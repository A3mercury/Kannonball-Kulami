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
    Rect ServerRect;
    Rect UsernameRect;
    Rect ConnectionRequestRect;
    public GUISkin ServerBackground;
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
        // putting this here keeps the aspect ratio the same for the window
        // no matter what the screen size is
        Debug.Log(serverWindowWidth);
        ServerRect = new Rect();
        ServerRect.x = (Screen.width * (1 - serverWindowWidth)) / 2;
        ServerRect.y = (Screen.height * (1 - serverWindowHeight)) / 2;
        ServerRect.width = Screen.width * serverWindowWidth;
        ServerRect.height = Screen.height * serverWindowHeight;

        UsernameRect = new Rect();
        UsernameRect.x = ServerRect.width * (25f / ServerRect.width);
        UsernameRect.y = ServerRect.height * (25f / ServerRect.height);
        UsernameRect.width = ServerRect.width * (242f / ServerRect.width) - 100;
        UsernameRect.height = ServerRect.height * (59f / ServerRect.height) - 15;
        

        if (isOnline)
        {
            GUI.skin = ServerBackground;

            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                Debug.Log("It has restarted.");
                ServerRect = GUI.Window(0, ServerRect, ServerWindow, "");
            }
            else
            {
                ServerRect = GUI.Window(0, ServerRect, ServerWindow, "");
                ServerRect = GUI.Window(0, ServerRect, windowFunc, "");


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

    public void ServerWindow(int id)
    {
        GUI.skin = ServerBackground;
        GUILayout.BeginHorizontal(GUI.skin.box);

        // Styles
        connectButton = new GUIStyle(GUI.skin.button);
        connectButton.margin = new RectOffset(50, 0, 45, 0);

        disconnectButton = new GUIStyle(GUI.skin.button);
        disconnectButton.margin = new RectOffset(50, 0, 45, 0);

        
        GUILayout.BeginArea(UsernameRect, GUI.skin.box);
        userName = GUILayout.TextField(userName, 15, GUI.skin.textField);
        GUILayout.EndArea();

        if (GUILayout.Button("Connect", connectButton))
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

        GUILayout.Button("Disconnect", disconnectButton);

        GUILayout.EndHorizontal();
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

    public void windowFunc(int id)
    {
        GUI.skin = ServerBackground;
        GUILayout.BeginHorizontal();

        // Styles
        connectButton = new GUIStyle(GUI.skin.button);
        connectButton.margin = new RectOffset(50, 0, 45, 0);

        disconnectButton = new GUIStyle(GUI.skin.button);
        disconnectButton.margin = new RectOffset(50, 0, 45, 0);

        inviteButton = new GUIStyle(GUI.skin.button);
        inviteButton.margin = new RectOffset(0, 45, 0, 45);

        //OpponentRect = new GUIStyle(GUI.skin.box);
        //OpponentRect.margin = new RectOffset(0, 0, 0, 50);
        //GUI.skin.scrollView = OpponentRect;

        GUILayout.TextField(userName);

        GUILayout.Button("Connect", connectButton);

        if (GUILayout.Button("Disconnect", disconnectButton))
        {
            Network.Disconnect();
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Refresh"))
        {
            MasterServer.ClearHostList();
            MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
        }
        //else
            //InvokeRepeating("GetHostList", 0, 60);
        

       // scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        //GUILayout.Box("", OpponentRect);

        if (MasterServer.PollHostList().Length != 0)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            HostData[] data = MasterServer.PollHostList();
            foreach (HostData c in data)
            //for (int i = 0; i < 20; i++)
            {
                GUILayout.BeginHorizontal();
                //if (c.gameName != userName)
                //{
                GUILayout.Box(c.gameName);
                if(c.gameName != userName)
                if (GUILayout.Button("Invite", inviteButton))
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
        //GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
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
