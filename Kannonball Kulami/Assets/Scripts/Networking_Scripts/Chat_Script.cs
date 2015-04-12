using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Chat_Script : MonoBehaviour
{
    Network_Manager networkManager;
    void Awake()
    {
        networkManager = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();        
    }

    public Vector2 scrollPosition = Vector2.zero;
    public GUISkin myskin;
    private GUIStyle myStyle;
    private GUIStyle myOtherStyle;
    private bool firstConnection = true;
    private Rect windowRect;
    private string messBox = "", messageToSend = "", user = "";

    // Chat canvas stuff
    public Canvas GameSceneCanvas;
    public GameObject ChatBoxPanel;
    public Button sendButton;
    public InputField messageField;
    public Button pulloutTab;

    public Animator ChatBoxShowHide;

    public GUISkin ChatSkin;

    void Start()
    {
        

        sendButton.onClick.AddListener(SendButtonClick);
        pulloutTab.onClick.AddListener(OpenCloseChat);
    }

    void Update()
    {
        
    }

    public void OpenCloseChat()
    {
        if (ChatBoxShowHide.GetBool("isChatOpen"))
            ChatBoxShowHide.SetBool("isChatOpen", false);
        else
            ChatBoxShowHide.SetBool("isChatOpen", true);
    }

    private void OnGUI()
    {
        GUI.skin = ChatSkin;

        windowRect = new Rect(
            (Screen.width * 2f) / 100, 
            (Screen.height * 34f) / 100, 
            (Screen.width * 24f) / 100, 
            (Screen.height * 50f) / 100
            );

        //if (networkManager.isInGame)
        //{
            //if (firstConnection)
            //{
            //    if (Network.isServer)
            //    {
            //        messBox = networkManager.clientName + " has challenged you to a game! Do you accept?\n";
            //    }
            //    else
            //    {
            //        messBox = "You have connected to " + networkManager.serverName + ". You will go first, and play as red.\n";
            //    }
            //    firstConnection = false;
            //}
            if(ChatBoxShowHide.GetBool("isChatOpen"))
                GUI.Window(120, windowRect, windowFunc, "");
        //}

    }

    void SendButtonClick()
    {
        if (messageField.text != "" && messageField.text.Length < 140)
        {
            networkView.RPC("SendMyMessage", RPCMode.All, networkManager.userName + ": " + messageField.text + "\n");
            messageField.text = "";
        }
    }

    private void windowFunc(int id)
    {
        //myStyle = GUI.skin.box;
        //myStyle.alignment = TextAnchor.UpperLeft;
        //myStyle.wordWrap = true;
        //myStyle.stretchHeight = true;
        //myOtherStyle = GUI.skin.textField;
        //myOtherStyle.alignment = TextAnchor.MiddleLeft;
        //myOtherStyle.clipping = TextClipping.Clip;
        //myOtherStyle.wordWrap = false;
        //myOtherStyle.fixedWidth = 200;
        //GUILayout.Box(messBox, myStyle, GUILayout.Height(350));

        GUI.skin = ChatSkin;

        scrollPosition = GUILayout.BeginScrollView(scrollPosition,GUI.skin.customStyles[0]);
        GUILayout.Box(messBox, GUI.skin.box);
        GUILayout.EndScrollView();
        //GUILayout.BeginHorizontal();

        //GUILayout.EndHorizontal();
        //GUILayout.BeginHorizontal();
        ////GUILayout.Label("User:");
        ////user = GUILayout.TextField(user);
        //GUILayout.EndHorizontal();
        //GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

   [RPC]
   public void SendMyMessage(string mess)
   {
       Debug.Log(mess);
       messBox += mess;
   }

}