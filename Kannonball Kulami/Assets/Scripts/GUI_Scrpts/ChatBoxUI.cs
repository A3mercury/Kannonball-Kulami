using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatBoxUI : MonoBehaviour 
{
    public Animator[] chatboxAnimators;
    public Animator ChatOpenCloseAnimator;
    public Animator SendButtonAnimator;

    public Button[] chatboxButtons;
    public Button sendChatButton;
    public Button pulloutTab;

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

        // assigning the buttons
        chatboxButtons = GetComponentsInChildren<Button>();
        foreach(Button button in chatboxButtons)
        {
            if (button.name == "send_button")
                sendChatButton = button;
            if (button.name == "pullout_tab")
                pulloutTab = button;
        }

        // event listeners for the buttons
        sendChatButton.onClick.AddListener(SendMessage);
        pulloutTab.onClick.AddListener(OpenCloseChat);
	}

    // Called when the pulloutTab is clicked
    public void OpenCloseChat()
    {
        if (ChatOpenCloseAnimator.GetBool("isChatOpen"))
            ChatOpenCloseAnimator.SetBool("isChatOpen", false);
        else
            ChatOpenCloseAnimator.SetBool("isChatOpen", true);
    }

    public void SendMessage()
    {
        // access the animator to change the button when pressed
        SendButtonAnimator.SetBool("isSend", true);
        
        // will send message to chat
    }
}
