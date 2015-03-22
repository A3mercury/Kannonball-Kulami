using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkLoginInterface : MonoBehaviour 
{
    Canvas serverCanvas;

    InputField usernameField;

    Button[] serverButtons;
    Button connectButton;
    Button disconnectButton;
    Button inviteToGameButton;

	// Use this for initialization
	void Start () 
    {
        serverCanvas = GetComponent<Canvas>();
        serverCanvas.enabled = true;

        // get username field
        usernameField = GetComponentInChildren<InputField>();

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

        connectButton.onClick.AddListener(ConnectToServer);
        disconnectButton.onClick.AddListener(DisconnectFromServer);
        //inviteToGameButton.onClick.AddListener(InviteToGame);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void ConnectToServer()
    {

    }

    void DisconnectFromServer()
    {

    }

    void InviteToGame()
    {

    }
}
