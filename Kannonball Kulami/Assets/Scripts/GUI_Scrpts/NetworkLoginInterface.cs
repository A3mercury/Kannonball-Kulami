//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System;

//public class NetworkLoginInterface : MonoBehaviour
//{
//    bool requested = true;
//    int listlength = 0;
//    public Canvas MPSceneCanvas;

//    public InputField UsernameField;

//    public Button ConnectButton;
//    public Button DisconnectButton;
//    public Button RefreshListButton;
//    public Button InviteTogameButton;

//    public GameObject OpponentsListContent;
//    public ScrollRect OpponentsListRect;
//    public Scrollbar OpponentsListScrollbar;

//    public Network_Manager network;

//    public GameObject OpponentsPanel;
//    public Text OpponentsName;

//    private HostData[] oldList;
//    private HostData[] newList;

//    GameObject[] GList;
//    float opponentListFloatIncrease;

//    void Start()
//    {
//        OpponentsListContent = new GameObject("opponent_content");
//        OpponentsListContent.transform.parent = OpponentsListRect.transform;
//        OpponentsListContent.transform.position = OpponentsListRect.transform.position;

//        RectTransform RT = OpponentsListContent.AddComponent<RectTransform>();
//        RectTransform PRT = OpponentsListContent.GetComponentInParent<RectTransform>();

//        RT.sizeDelta = new Vector2(0, 600f);
//        RT.position = new Vector3(PRT.position.x, PRT.position.y - 500, PRT.position.z);

//        OpponentsListContent.AddComponent<Image>().color = Color.green;

//        // for spacing out the opponents when polled from the server
//        opponentListFloatIncrease = 0;
//    }

//    void Update()
//    {
//        //if (GUILayout.Button("Refresh"))
//        //for (int i = 0; i < 1; i++ )
//        //MasterServer.ClearHostList();

        

//        if (Network.isServer)
//        {
//            if (oldList == null || oldList.Length == 0)
//            {
//                //oldList = MasterServer.PollHostList();
//            }

//            newList = MasterServer.PollHostList();
//            if (Input.GetMouseButtonDown(1))
//            {
//                //MasterServer.ClearHostList();
//                newList = MasterServer.PollHostList();
//                foreach (HostData item in newList)
//                {
//                    Debug.Log(item.gameName);
//                }
//            }
//        }
        

//        if (Network.isServer && listsAreDifferent(oldList, newList))//listlength != MasterServer.PollHostList().Length)
//        {
//            Debug.Log(MasterServer.PollHostList().Length);
//            if (MasterServer.PollHostList().Length != 0)
//            {
//                HostData[] data = MasterServer.PollHostList();
//                foreach (HostData c in data)
//                //for (int i = 0; i < 10; i++)
//                {
//                    Debug.Log(c.gameName);
//                    GameObject newGO = Instantiate(
//                        OpponentsPanel,
//                        new Vector3(0, 0, 0),
//                        Quaternion.identity
//                    ) as GameObject;

//                    newGO.transform.parent = OpponentsListContent.transform;
//                    newGO.GetComponent<RectTransform>().sizeDelta = new Vector2(260f, 54f);
//                    newGO.GetComponent<RectTransform>().transform.position = new Vector3(
//                        newGO.transform.parent.position.x,
//                        (newGO.GetComponent<RectTransform>().sizeDelta.y / 2) - ((newGO.GetComponent<RectTransform>().sizeDelta.y + 10) * ++opponentListFloatIncrease),
//                        0
//                    );

//                    OpponentsListRect.GetComponent<ScrollRect>().content = OpponentsListContent.GetComponent<RectTransform>();
//                    OpponentsListRect.GetComponent<ScrollRect>().verticalScrollbar = GameObject.Find("opponent_scrollbar").GetComponent<Scrollbar>();

//                    OpponentsName = newGO.GetComponentInChildren<Text>();
//                    Debug.Log("Showing " + c.gameName);
//                    OpponentsName.text = c.gameName;


//                    listlength = MasterServer.PollHostList().Length;
//                }

                
//                Debug.Log(listlength);

//            }
//            oldList = newList;
//        }

//    }

//    bool listsAreDifferent(HostData[] old, HostData[] newList)
//    {
//        bool result = false;
//        if (old == null || old.Length != newList.Length)
//        {
//            result = true;
//        }
//        else
//        {
//            for (int i = 0; i < old.Length && !result; i++)
//            {
//                if (old[i].gameName != newList[i].gameName)
//                {
//                    result = true;
//                }
//            }
//        }
//        return result;
//    }

//    //bool HasBeenUpdated()
//    //{
//    //    GList = GameObject.FindObjectsOfType<GameObject>();
//    //    GameObject[] Opponentlist;
//    //    foreach(GameObject G in GList)
//    //    {
//    //        if(G.name == "opponent_panel(Clone)")
//    //            Opponentlist.
//    //        //if(MasterServer.PollHostList().Length != GList.Length)
//    //    }
//    //    return true;
//    //}

//    public void ConnectToServer()
//    {
//        if (network.isOnline && UsernameField.text != "")
//        {
//            if (Network.peerType == NetworkPeerType.Disconnected)
//            {
//                network.userName = UsernameField.text;
//                try
//                {
//                    network.StartServer();
//                    //MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
//                    //******************************DO NOT CALL LIST OPPONENTS FROM HERE!!!!!
//                }
//                catch (Exception ex)
//                {
//                    print("Exception " + ex);
//                    Debug.Log("Exception " + ex);
//                }
//            }
//            else
//            {

//            }
            
//        }
//    }

//    public void DisconnectFromServer()
//    {
//        // clear text field
//        UsernameField.text = "";
//        Network.Disconnect();

//        // clear opponents_list


//        // disconnect from server
//        if (!Network.isServer)
//            Debug.Log("Disconnected from server");
//    }

//    public void RefreshList()
//    {
//        MasterServer.RequestHostList("KannonBall_Kulami_HU_Softdev_Team1_2015");
//        Debug.Log(MasterServer.PollHostList().Length);
//        //oldList = MasterServer.PollHostList();
//    }

//    void ClearOpponentsList()
//    {

//    }

//    public void InviteOppontent()
//    {

//    }
//}
