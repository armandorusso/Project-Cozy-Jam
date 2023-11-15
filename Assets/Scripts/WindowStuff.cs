using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStuff : MonoBehaviour
{

    int LastScreenWidth = 1600;
    int LastScreenHeight = 900;
    float ScreenRatio = 1600f / 900f;

    void Update()
    {

            if ((LastScreenHeight != Screen.height) || (LastScreenWidth != Screen.width))
            {
                Screen.SetResolution(Mathf.RoundToInt((float)Screen.height * ScreenRatio), Screen.height, Screen.fullScreen);
            }
            LastScreenHeight = Screen.height;
            LastScreenWidth = Mathf.RoundToInt(Screen.height * ScreenRatio);

    }
 }
