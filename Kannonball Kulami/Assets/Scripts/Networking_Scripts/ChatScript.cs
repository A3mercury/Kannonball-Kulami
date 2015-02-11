using UnityEngine;
using System.Collections;

public class ChatScript : MonoBehaviour
{
    void Awake()
    {
        get = gameObject.GetComponent<Network_Manager>();
    }

    Network_Manager get;
    public GUISkin myskin;
    private Rect windowRect = new Rect(680, 20, 250, 450);
    private string messBox = "", messageToSend = "", user = "";

    private void OnGUI()
    {
        GUI.skin = myskin;
        windowRect = GUI.Window(1, windowRect, windowFunc, "Chat");
    }

    private void windowFunc(int id)
    {
        GUILayout.Box(messBox, GUILayout.Height(350));
        GUILayout.BeginHorizontal();
        messageToSend = GUILayout.TextField(messageToSend);
        if (GUILayout.Button("Send", GUILayout.Width(75)))
        {

            networkView.RPC("SendMessage", RPCMode.All, get.userName + ": " + messageToSend + "\n");
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
    private void SendMessage(string mess)
    {
        messBox += mess;
    }
}
