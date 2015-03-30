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
    public bool isConnected = false;
    public bool isOnline;
    public bool isInGame = false;
    public static bool fromtransition;
    public string serverName;
    public string clientName;
    public static int networkplayer;
    bool sendrequest = false, prompt = false, beingconnectedto = false, respondtorequest = false, waitingforresponse = false;
    public string userName = "", maxPlayers = "10", port = "21212", userwantingtoconnect = "", userwantingtoconnectfromserver = "";
    public GUISkin myskin;
    //private Rect windowRect = new Rect(0, 43, 200, 200);
    private GameCore gameCore;
    public Vector2 scrollPosition = Vector2.zero;

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

    //public NetworkLoginInterface login;

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
        
    }

    void OnServerInitialized()
    {
        Debug.Log(userName + " joined as Server.");
        MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
    }

    void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            Debug.Log("Registration was successful.");
    }

    void Update()
    {
        if(Network.isServer)
            MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");

        // updates the window to the size of the screen on every update
        
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
            ServerRect = GUI.Window(0, ServerRect, ServerWindow, "");
            if(Network.peerType == NetworkPeerType.Disconnected)
            {
            }
            else
            {
            //    if (GUILayout.Button("Disconnect"))
            //    {
            //        Network.Disconnect();
            //    }

                    ServerRect = GUI.Window(0, ServerRect, windowFunc, "");
                
            }

            if(beingconnectedto)
            {
                ConnectionRequestRect = GUI.Window(0, ConnectionRequestRect, RequestPopup, "");
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
        userName = GUILayout.TextField(userName, GUI.skin.textField);
        GUILayout.EndArea();

        if (GUILayout.Button("Connect", connectButton))
        {
            try
            {
                StartServer();
                isConnected = true;                
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
        MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
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
                    sendrequest = true;
                    userwantingtoconnectfromserver = c.gameName;
                    isConnected = true;
                    //networkView.RPC("SendConnectionRequest", RPCMode.Server, userName, true);
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
        userwantingtoconnect = userName;
        beingconnectedto = request;
    }

    [RPC]
    private void RespondtoRequest(bool response)
    {
        //isconnected = response;
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
