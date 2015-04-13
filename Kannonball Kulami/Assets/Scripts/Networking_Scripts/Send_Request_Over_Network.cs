﻿//using UnityEngine;
//using System.Collections;

//public class Send_Request_Over_Network : MonoBehaviour {

//    public Vector2 scrollPosition;
//    private GUIStyle myStyle;
//    public GUISkin myskin;
//    private GUIStyle myOtherStyle;
//    Network_Manager get;
//    private Rect windowRect = new Rect((Screen.width - 250)/2, (Screen.height - 100) / 2, 250, 100);
//    private string messBox = "", messageToSend = "", user = "";
//    private bool firstConnection = true;
//    private bool resolveRequest = false;
//    void Awake()
//    {
//        get = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
//    }
//    // Update is called once per frame
//    private void OnGUI()
//    {
//        GUI.skin = myskin;
//        if (get.sentRequest)
//        {
//            Debug.Log(get.sentRequest);
//            if (firstConnection)
//            {
//                if (Network.isServer)
//                {
//                    messBox = get.clientName + " has challenged you to a game! Do you accept?\n";
//                }
//                else
//                {
//                    messBox = "You have challenged " + get.serverName + " to a game. Awaiting response...\n";
//                }
//                firstConnection = false;
//            }
//            windowRect = GUI.Window(1, windowRect, windowFunc, "");
//        }
//        else
//            return;
//    }

//    private void windowFunc(int id)
//    {
//        myStyle = GUI.skin.box;
//        myStyle.alignment = TextAnchor.UpperLeft;
//        myStyle.wordWrap = true;
//        myStyle.stretchHeight = true;
//        myOtherStyle = GUI.skin.textField;
//        myOtherStyle.alignment = TextAnchor.MiddleLeft;
//        myOtherStyle.clipping = TextClipping.Clip;
//        myOtherStyle.wordWrap = false;
//        myOtherStyle.fixedWidth = 200;

   
//        GUILayout.Width(250);
//        GUILayout.Box(messBox, myStyle);
//        GUILayout.BeginHorizontal();
 
//        if (Network.isServer)
//        {
//            if (GUILayout.Button("Accept", GUILayout.Width(75)))
//            {
//                get.networkView.RPC("RespondtoRequest", RPCMode.All, true);
//            }
//            if (GUILayout.Button("Deny", GUILayout.Width(75)))
//            {
//                get.networkView.RPC("RespondtoRequest", RPCMode.All, false);
//            }
//        }
//        GUILayout.EndHorizontal();
//        GUILayout.BeginHorizontal();
//        GUILayout.EndHorizontal();
        
//    }
//    [RPC]
//    private void SendMyMessage(string mess)
//    {
//        Debug.Log(mess);
//        messBox += mess;
//    }
   
//}
