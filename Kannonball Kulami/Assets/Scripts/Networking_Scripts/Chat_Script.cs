﻿using UnityEngine;
using System.Collections;

public class Chat_Script : MonoBehaviour
{
    Network_Manager get;
    void Awake()
    {
        get = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
        //myStyle = new GUIStyle();
        
        //myOtherStyle = new GUIStyle();
        
    }

    public Vector2 scrollPosition;
    public GUISkin myskin;
    private GUIStyle myStyle;
    private GUIStyle myOtherStyle;
    private bool firstConnection = true;
    private Rect windowRect = new Rect(0, 275, 300, 200);
    private string messBox = "", messageToSend = "", user = "";

    private void OnGUI()
    {
            GUI.skin = myskin;
            if (get.isInGame)
            {
                if (firstConnection)
                {
                    if (Network.isServer)
                    {
                        messBox = get.clientName + " has challenged you to a game! You will go second, and play as black.\n";
                    }
                    else
                    {
                        messBox = "You have connected to " + get.serverName + ". You will go first, and play as red.\n";
                    }
                    firstConnection = false;
                }
                
                windowRect = GUI.Window(1, windowRect, windowFunc, "Chat");
            }
            
    }

    private void windowFunc(int id)
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
        //GUILayout.Box(messBox, myStyle, GUILayout.Height(350));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(280));
        GUILayout.Box(messBox, myStyle);
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        messageToSend = GUILayout.TextField(messageToSend, myOtherStyle);
        if (GUILayout.Button("Send", GUILayout.Width(75)))
        {
            
            networkView.RPC("SendMyMessage", RPCMode.All, get.userName + ": " + messageToSend + "\n");
            Debug.Log(messBox);
            messageToSend = "";
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        //GUILayout.Label("User:");
        //user = GUILayout.TextField(user);
        GUILayout.EndHorizontal();
        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

   [RPC]
   private void SendMyMessage(string mess)
   {
       Debug.Log(mess);
       messBox += mess;
   }

}