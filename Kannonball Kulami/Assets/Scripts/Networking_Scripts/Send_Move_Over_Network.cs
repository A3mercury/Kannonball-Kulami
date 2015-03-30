using UnityEngine;
using System.Collections;

public class Send_Move_Over_Network : MonoBehaviour {

    public Vector2 scrollPosition;
    private GUIStyle myStyle;
    public GUISkin myskin;
    private GUIStyle myOtherStyle;
    Network_Manager get;
    private Rect windowRect = new Rect((Screen.width - 250)/2, (Screen.height - 100) / 2, 250, 100);
    private string messBox = "", messageToSend = "", user = "";
    private bool firstConnection = true;
    void Awake()
    {
        get = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
    }
	// Update is called once per frame
    private void OnGUI()
    {
        GUI.skin = myskin;
        if(get.isInGame)
        {
            if(firstConnection)
            {
                if(Network.isServer)
                {
                    messBox = get.clientName + " has challenged you to a game! Do you accept?\n";
                }
                else
                {
                    messBox = "You have challenged " + get.serverName + " to a game. Awaiting response...\n";
                }
                firstConnection = false;
            }
            windowRect = GUI.Window(1, windowRect, windowFunc, "");
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

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(280));
        GUILayout.Box(messBox, myStyle);
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        //messageToSend = GUILayout.TextField(messageToSend, myOtherStyle);
        if (GUILayout.Button("Accept", GUILayout.Width(75)))
        {

            //networkView.RPC("SendMyMessage", RPCMode.All, get.userName + ": " + messageToSend + "\n");
            //Debug.Log(messBox);
            //messageToSend = "";
        }
        if(GUILayout.Button("Deny", GUILayout.Width(75)))
        {
        
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();
        
    }
    [RPC]
    private void SendMyMessage(string mess)
    {
        Debug.Log(mess);
        messBox += mess;
    }
   
}
