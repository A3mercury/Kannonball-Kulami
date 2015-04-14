using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Chat_Script : MonoBehaviour
{
    Network_Manager networkManager;

    public Vector2 scrollPosition = Vector2.zero;
    public GUISkin myskin;
    private GUIStyle myStyle;
    private GUIStyle myOtherStyle;
    private bool firstConnection = true;
    public bool clearbox = false;
    public static bool ChatOpen = false;
    private Rect windowRect = new Rect(
                (Screen.width * 2f) / 100,
                (Screen.height * 34f) / 100,
                (Screen.width * 24f) / 100,
                (Screen.height * 50f) / 100
                );
    public string messBox = "", messageToSend = "", user = "";

    // Chat canvas stuff
    public Canvas GameSceneCanvas;
    public GameObject ChatBoxPanel;
    public Button sendButton;
    public InputField messageField;
    public Button pulloutTab;
    public GameCore gameCore;

    public Animator ChatBoxShowHide;

    public GUISkin ChatSkin;

    void Awake()
    {
        networkManager = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        sendButton.onClick.AddListener(SendButtonClick);
        pulloutTab.onClick.AddListener(OpenCloseChat);
    }

    void Update()
    {
        
    }

    public void OpenCloseChat()
    {
        if (ChatBoxShowHide.GetBool("isChatOpen"))
        {
            ChatOpen = false;
            ChatBoxShowHide.SetBool("isChatOpen", false);
        }
        else
        {
            ChatOpen = true;
            ChatBoxShowHide.SetBool("isChatOpen", true);
        }
    }

    private void OnGUI()
    {
        if (networkManager.ingame && !gameCore.GameIsOver)
        {
            ChatBoxPanel.gameObject.SetActive(true);
            GUI.skin = ChatSkin;
            //GUI.Label(windowRect, messBox);
            if (ChatBoxShowHide.GetBool("isChatOpen"))
                GUI.Window(120, windowRect, windowFunc, "");
        }
        else
        {
            ChatBoxPanel.gameObject.SetActive(false);
            GUI.skin = ChatSkin;
            messBox = "";
            if (ChatBoxShowHide.GetBool("isChatOpen"))
                GUI.Window(120, windowRect, windowFunc, "");
        }
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
        GUI.skin = ChatSkin;

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.customStyles[0]);
        GUILayout.Box(messBox, GUI.skin.box);
        GUILayout.EndScrollView();

    }

    [RPC]
    public void SendMyMessage(string mess)
    {
        Debug.Log(mess);
        messBox += mess;
    }
}