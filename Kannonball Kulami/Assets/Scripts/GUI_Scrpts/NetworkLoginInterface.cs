using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NetworkLoginInterface : MonoBehaviour
{
    public Canvas MPSceneCanvas;

    public InputField UsernameField;

    public Button ConnectButton;
    public Button DisconnectButton;
    public Button RefreshListButton;
    public Button InviteTogameButton;

    public GameObject OpponentsListContent;
    public ScrollRect OpponentsListRect;
    public Scrollbar OpponentsListScrollbar;

    public Network_Manager network;

    public GameObject OpponentsPanel;
    public Text OpponentsName;

    float opponentListFloatIncrease;

    void Start()
    {
        //// get the opponent's name out of all the text objects in the children
        //foreach (Text t in GetComponentsInChildren<Text>())
        //{
        //    if (t.name == "opponent_name")
        //        OpponentsName = t;
        //}
        // for spacing out the opponents when polled from the server
        opponentListFloatIncrease = 0;
    }

    void Update()
    {

    }

    public void ConnectToServer()
    {
        if (network.isOnline && UsernameField.text != "")
        {
            network.userName = UsernameField.text;

            try
            {
                network.StartServer();
                //******************************DO NOT CALL LIST OPPONENTS FROM HERE!!!!!
            }
            catch (Exception ex)
            {
                print("Exception " + ex);
                Debug.Log("Exception " + ex);
            }
        }
    }

    public void DisconnectFromServer()
    {
        // clear text field
        UsernameField.text = "";
        Network.Disconnect();

        // clear opponents_list


        // disconnect from server
        if (!Network.isServer)
            Debug.Log("Disconnected from server");
    }

    public void RefreshList()
    {
        //if (GUILayout.Button("Refresh"))
            //for (int i = 0; i < 1; i++ )
            //MasterServer.ClearHostList();

        MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");

        OpponentsListContent = new GameObject("opponent_content");
        OpponentsListContent.transform.parent = OpponentsListRect.transform;
        OpponentsListContent.transform.position = OpponentsListRect.transform.position;

        RectTransform RT = OpponentsListContent.AddComponent<RectTransform>();
        RectTransform PRT = OpponentsListContent.GetComponentInParent<RectTransform>();

        RT.sizeDelta = new Vector2(0, 600f);
        RT.position = new Vector3(PRT.position.x, PRT.position.y - 500, PRT.position.z);

        OpponentsListContent.AddComponent<Image>().color = Color.green;

        Debug.Log(MasterServer.PollHostList().Length);
        if (MasterServer.PollHostList().Length != 0)
        {
            HostData[] data = MasterServer.PollHostList();
            foreach (HostData c in data)
            //for (int i = 0; i < 10; i++)
            {
                Debug.Log(c.gameName);
                GameObject newGO = Instantiate(
                    OpponentsPanel,
                    new Vector3(0, 0, 0),
                    Quaternion.identity
                ) as GameObject;

                newGO.transform.parent = OpponentsListContent.transform;
                newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(260f, 54f);
                newGO.GetComponent<RectTransform>().transform.position = new Vector3(
                    newGO.transform.parent.position.x,
                    (newGO.GetComponent<RectTransform>().sizeDelta.y / 2) - ((newGO.GetComponent<RectTransform>().sizeDelta.y + 10) * ++opponentListFloatIncrease),
                    0
                );

                OpponentsListRect.GetComponent<ScrollRect>().content = OpponentsListContent.GetComponent<RectTransform>();
                OpponentsListRect.GetComponent<ScrollRect>().verticalScrollbar = GameObject.Find("opponent_scrollbar").GetComponent<Scrollbar>();

                OpponentsName = newGO.GetComponentInChildren<Text>();
                Debug.Log("Showing " + c.gameName);
                OpponentsName.text = c.gameName;
            }
        }
    }

    void ClearOpponentsList()
    {

    }

    public void InviteOppontent()
    {

    }
}
//    Canvas serverCanvas;

//    InputField usernameField;

//    Button[] serverButtons;
//    Button connectButton;
//    Button disconnectButton;
//    Button inviteToGameButton;

//    Text[] textObjects;
//    Text opponentName;

//    GameObject opponentsListParent;
//    GameObject[] opponentsList;
//    public GameObject opponentsPanel;

//    Network_Manager network;
//    GameCore gameCore;

//    Vector2 scrollPosition;
//    public GameObject opponentListRect;
//    GameObject opponentListContent;

//    private Rect windowRect = new Rect(0, 43, 200, 200);
//    float opponentListFloatIncrease;

//    int frames = 0;
//    bool timeToCheck = false;

//    // Use this for initialization
//    void Start()
//    {
//        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
//        serverCanvas = GetComponent<Canvas>();
//        serverCanvas.enabled = true;

//        // get username field
//        usernameField = GetComponentInChildren<InputField>();

//        // assign all the buttons in the server list
//        serverButtons = GetComponentsInChildren<Button>();
//        foreach (Button b in serverButtons)
//        {
//            if (b.name == "connect_button")
//                connectButton = b;
//            else if (b.name == "disconnect_button")
//                disconnectButton = b;
//            else if (b.name == "invite_button")
//                inviteToGameButton = b;
//        }

//        //// get the opponent's name out of all the text objects in the children
//        //textObjects = GetComponentsInChildren<Text>();
//        //foreach(Text t in textObjects)
//        //{
//        //    if (t.name == "opponent_name")
//        //        opponentName = t;
//        //}

//        // set the necessary properties
//        connectButton.enabled = false;
//        disconnectButton.enabled = false;
//        connectButton.onClick.AddListener(ConnectToServer);
//        disconnectButton.onClick.AddListener(DisconnectFromServer);
//        //inviteToGameButton.onClick.AddListener(InviteToGame);

//        network = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
//        opponentListFloatIncrease = 0;
//    }

//    // Update is called once per frame
//    private void Update()
//    {
//        frames++;
//        if (network.isOnline)
//        {
//            if (Network.peerType == NetworkPeerType.Disconnected)
//            {
//                network.userName = usernameField.text;

//                if (network.userName != "")
//                {
//                    connectButton.enabled = true;
//                    disconnectButton.enabled = true;
//                }
//                else
//                {
//                    connectButton.enabled = false;
//                    disconnectButton.enabled = false;
//                }
//            }
//            else
//            {
//                ListOpponents();
//            }
//            if (Network.isServer)
//            {
//                gameCore.playerColor = "black";
//                //network.networkplayer = 1;
//            }
//            if (Network.isClient && !network.isInGame)
//            {
//                //network.networkView.RPC("OnChallenge", RPCMode.All, network.userwantingtoconnectfromserver, network.userName);
//            }
//        }
//    }

//    void ConnectToServer()
//    {
//        try
//        {
//            network.StartServer();
//            //******************************DO NOT CALL LIST OPPONENTS FROM HERE!!!!!
//        }
//        catch (Exception ex)
//        {
//            //print("Exception " + ex);
//            Debug.Log("Exception " + ex);
//        }
//    }

//    void DisconnectFromServer()
//    {
//        // clear text field
//        usernameField.text = "";
//        Network.Disconnect();

//        // clear opponents_list


//        // disconnect from server
//        if (!Network.isServer)
//            Debug.Log("Disconnected from server");
//    }

//    public void ListOpponents()
//    {
//        //if(GUILayout.Button("Refresh"))
//        //for (int i = 0; i < 1; i++ )
//        MasterServer.ClearHostList();
//        MasterServer.RequestHostList("testkannonball");

//        opponentListContent = new GameObject("opponent_content");
//        opponentListContent.transform.parent = opponentListRect.transform;
//        opponentListContent.transform.position = opponentListRect.transform.position;

//        RectTransform RT = opponentListContent.AddComponent<RectTransform>();
//        RectTransform PRT = opponentListContent.GetComponentInParent<RectTransform>();

//        RT.sizeDelta = new Vector2(0, 600f);
//        RT.position = new Vector3(PRT.position.x, PRT.position.y - 500, PRT.position.z);

//        opponentListContent.AddComponent<Image>().color = Color.green;

//        if (MasterServer.PollHostList().Length != 0)
//        {
//            HostData[] data = MasterServer.PollHostList();
//            foreach (HostData c in data)
//            //for (int i = 0; i < 10; i++)
//            {
//                Debug.Log(c.gameName);
//                GameObject newGO = Instantiate(
//                    opponentsPanel,
//                    new Vector3(0, 0, 0),
//                    Quaternion.identity
//                ) as GameObject;

//                newGO.transform.parent = opponentListContent.transform;
//                newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(275f, 54f);
//                newGO.GetComponent<RectTransform>().transform.position = new Vector3(
//                    newGO.transform.parent.position.x,
//                    (newGO.GetComponent<RectTransform>().sizeDelta.y / 2) - ((newGO.GetComponent<RectTransform>().sizeDelta.y + 10) * opponentListFloatIncrease++),
//                    0
//                );

//                opponentListRect.GetComponent<ScrollRect>().content = opponentListContent.GetComponent<RectTransform>();
//                opponentListRect.GetComponent<ScrollRect>().verticalScrollbar = GameObject.Find("opponent_scrollbar").GetComponent<Scrollbar>();

//                opponentName = newGO.GetComponentInChildren<Text>();
//                opponentName.text = c.gameName;


//            }
//        }
//    }

//    void InviteToGame()
//    {
//        //Network.Connect(c);
//        //network.sendrequest = true;
//        //network.userwantingtoconnectfromserver = c.gameName;
//        //network.isconnected = true;
//        //networkView.RPC("OnChallenge", RPCMode.All);
//        gameCore.playerColor = "red";
//    }
//}
