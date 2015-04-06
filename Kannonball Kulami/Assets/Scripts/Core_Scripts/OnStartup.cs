using UnityEngine;
using System.Collections;

public class OnStartup : MonoBehaviour 
{
    void Awake()
    {
        //Resolution[] resolutions = Screen.resolutions;
        //foreach(Resolution res in resolutions)
        //{
        //    print(res.width + "x" + res.height);
        //}

        Screen.SetResolution(1440, 900, true);
    }
}
