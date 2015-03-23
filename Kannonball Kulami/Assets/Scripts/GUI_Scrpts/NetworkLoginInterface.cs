using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NetworkLoginInterface : MonoBehaviour 
{
    Canvas serverCanvas;

    InputField usernameField;

    Button[] serverButtons;
    Button connectButton;
    Button disconnectButton;
    Button inviteToGameButton;

    //Image opponentPanel;

    Text[] textObjects;
    Text opponentName;

    GameObject opponentsListParent;
    GameObject[] opponentsList;
    public GameObject opponentsPanel;

    Network_Manager network;
    GameCore gameCore;

    Vector2 scrollPosition;

    private Rect windowRect = new Rect(0, 43, 200, 200);

	// Use this for initialization
	void Start () 
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        serverCanvas = GetComponent<Canvas>();
        serverCanvas.enabled = true;

        // get username field
        usernameField = GetComponentInChildren<InputField>();

        // assign all the buttons in the server list
        serverButtons = GetComponentsInChildren<Button>();
        foreach(Button b in serverButtons)
        {
            if (b.name == "connect_button")
                connectButton = b;
            else if (b.name == "disconnect_button")
                disconnectButton = b;
            else if (b.name == "invite_button")
                inviteToGameButton = b;
        }

        // get the panel that will be copied to make the list of opponents
        //images = GetComponentsInChildren<Image>();
        //foreach(Image i in images)
        //{
        //    if (i.name == "opponent_panel")
        //        opponentPanel = i;
        //}

        // get the opponent's name out of all the text objects in the children
        textObjects = GetComponentsInChildren<Text>();
        foreach(Text t in textObjects)
        {
            if (t.name == "opponent_name")
                opponentName = t;
        }

        // set the necessary properties
        //opponentsListParent = GameObject.Find("opponents_list");
        //opponentsListParent.SetActive(false);

        opponentsPanel = GameObject.Find("opponents_panel");

        connectButton.enabled = false;
        disconnectButton.enabled = false;
        connectButton.onClick.AddListener(ConnectToServer);
        disconnectButton.onClick.AddListener(DisconnectFromServer);
        //inviteToGameButton.onClick.AddListener(InviteToGame);

        network = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();

    }
    
	// Update is called once per frame
    private void OnGUI()
    {
        if (network.isOnline)
        {
            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                network.userName = usernameField.text;

                if (network.userName != "")
                {
                    connectButton.enabled = true;
                    disconnectButton.enabled = true;
                }
                else
                {
                    connectButton.enabled = false;
                    disconnectButton.enabled = false;
                }
            }
            if (Network.isServer)
            {
                gameCore.playerColor = "black";
            }
            if (Network.isClient && !network.isInGame)
            {
                network.networkView.RPC("OnChallenge", RPCMode.Server, network.userwantingtoconnectfromserver, network.userName);
            }
        }
    }

    void ConnectToServer()
    {
        //try
        //{
           network.StartServer();
           ListOpponents();
        //}
        //catch(Exception ex)
        //{
        //    //print("Exception " + ex);
        //    Debug.Log("Exception " + ex);
        //}
    }

    void DisconnectFromServer()
    {
        usernameField.text = "";
        Network.Disconnect();

        if (!Network.isServer)
            Debug.Log("Disconnected from server");
    }

    void ListOpponents()
    {
        MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");



        if (MasterServer.PollHostList().Length != 0)
        {
            HostData[] data = MasterServer.PollHostList();
            foreach (HostData c in data)
            {
                GameObject newPanel = Instantiate(opponentsPanel) as GameObject;
                Image newPanelBackground = newPanel.GetComponent<Image>();
                newPanel.SetActive(true);

            }
        }
    }

    void InviteToGame()
    {

    }
}
