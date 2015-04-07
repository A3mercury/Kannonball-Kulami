using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseTexture : MonoBehaviour
{
    private int cursorW = 48;
    private int cursorH = 48;

    public Texture2D cursorTexture;

    void Awake()
    {
        Screen.showCursor = false;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, cursorW, cursorH), cursorTexture);
    }
}