using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatBoxUI : MonoBehaviour 
{
    GameObject ChatPanel;

    Animator[] chatboxAnimators;
    Animator ChatOpenCloseAnimator;
    Animator SendButtonAnimator;

    InputField messageField;

    Button[] chatboxButtons;
    Button sendChatButton;
    Button pulloutTab;

    Network_Manager networkManager;
    Chat_Script chatManager;

	// Use this for initialization
	void Start () 
    {
        // assigning the animators
        chatboxAnimators = GetComponentsInChildren<Animator>();
        foreach(Animator anim in chatboxAnimators)
        {
            if(anim.runtimeAnimatorController.name == "ChatOpenCloseController")
            {
                ChatOpenCloseAnimator = anim;
                ChatOpenCloseAnimator.SetBool("isChatOpen", false);
            }
            else if(anim.runtimeAnimatorController.name == "SendButtonController")
            {
                SendButtonAnimator = anim;
                SendButtonAnimator.SetBool("isSend", false);
            }
        }

        
        // only shows the chatbox in online play
        networkManager = GameObject.Find("Network_Manager").GetComponent<Network_Manager>();

        messageField = GameObject.Find("chatbox_input").GetComponent<InputField>() as InputField;
        chatManager = networkManager

        // assigning the buttons
        chatboxButtons = GetComponentsInChildren<Button>();
        foreach(Button button in chatboxButtons)
        {
            if (button.name == "send_button")
            {
                sendChatButton = button;
            }
            if (button.name == "pullout_tab")
                pulloutTab = button;
        }

        // event listeners for the buttons
        sendChatButton.onClick.AddListener(SendMessage);
        pulloutTab.onClick.AddListener(OpenCloseChat);

        ChatPanel = GameObject.Find("ChatBoxPanel");
	}

    // Called when the pulloutTab is clicked
    public void OpenCloseChat()
    {
        Debug.Log("Clicked on the pulltab");
        if (ChatOpenCloseAnimator.GetBool("isChatOpen"))
            ChatOpenCloseAnimator.SetBool("isChatOpen", false);
        else
            ChatOpenCloseAnimator.SetBool("isChatOpen", true);
    }

    public void SendMessage()
    {
        if (messageField.text != "")
        {
            // access the animator to change the button when pressed
            SendButtonAnimator.SetBool("isSend", true);

            // will send message to chat
            .SendMyMesssage(messageField.text);
        }
    }
}
