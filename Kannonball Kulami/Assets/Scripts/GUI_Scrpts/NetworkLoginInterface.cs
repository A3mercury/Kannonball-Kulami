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

    Text[] textObjects;
    Text opponentName;

    GameObject opponentsListParent;
    GameObject[] opponentsList;
    public GameObject opponentsPanel;

    Network_Manager network;
    GameCore gameCore;

    Vector2 scrollPosition;
    public GameObject opponentListRect;
    GameObject opponentListContent;

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

        // get the opponent's name out of all the text objects in the children
        textObjects = GetComponentsInChildren<Text>();
        foreach(Text t in textObjects)
        {
            if (t.name == "opponent_name")
                opponentName = t;
        }

        // set the necessary properties
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
        try
        {
            network.StartServer();
            ListOpponents();
        }
        catch (Exception ex)
        {
            //print("Exception " + ex);
            Debug.Log("Exception " + ex);
        }
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

        opponentListContent = new GameObject("opponent_content");
        opponentListContent.transform.parent = opponentListRect.transform;
        opponentListContent.transform.position = opponentListRect.transform.position;
       
        RectTransform RT = opponentListContent.AddComponent<RectTransform>();
        RectTransform PRT = opponentListContent.GetComponentInParent<RectTransform>();

        RT.sizeDelta = new Vector2(0, 500f);
        RT.position = new Vector3(PRT.position.x, PRT.position.y - 400, PRT.position.z);

        opponentListContent.AddComponent<Image>().color = Color.green;

        Debug.Log("get here");
        if (MasterServer.PollHostList().Length != 0)
        {
            
            HostData[] data = MasterServer.PollHostList();
            //foreach (HostData c in data)
            for (int i = 0; i < 10; i++)
            {
                GameObject newGO = Instantiate(
                    opponentsPanel,
                    new Vector3(0, 0, 0),
                    Quaternion.identity
                ) as GameObject;

                newGO.transform.parent = opponentListContent.transform;
                newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(275f, 54f);
                newGO.GetComponent<RectTransform>().transform.position = new Vector3(
                    newGO.transform.parent.position.x,
                    (newGO.GetComponent<RectTransform>().sizeDelta.y / 2),
                    0
                );

                opponentListRect.GetComponent<ScrollRect>().content = opponentListContent.GetComponent<RectTransform>();
                opponentListRect.GetComponent<ScrollRect>().verticalScrollbar = GameObject.Find("opponent_scrollbar").GetComponent<Scrollbar>();
                opponentListRect.GetComponent<Scrollbar>().size = 1f;
            }
        }
    }

    void InviteToGame()
    {

    }
}
