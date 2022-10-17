using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    float frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;  // 4 updates per sec.
    private void Start()
    {
        if (Debug.isDebugBuild)
        {
            Application.targetFrameRate = 10000;    
            Debug.developerConsoleVisible = true;
        }
    }

    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0/updateRate)
            {
                fps = frameCount / dt ;
                frameCount = 0;
                dt -= 1.0f /updateRate;
            }
        }
    }

    private void OnGUI()
    {
        // If it is a debug build, show the debug overlay
        if (Debug.isDebugBuild)
        {
            GUI.skin.label.fontSize = 20;
            GUI.skin.label.normal.textColor = Color.yellow;
            // GUI.Label(new Rect(10, 10, 200, 40), "FPS: " + (1.0f / Time.deltaTime).ToString("F2"));
            // Calculate framerate smoothly
            GUI.Label(new Rect(10, 10, 200, 40), "FPS: " + fps.ToString("F2"));
            
            
            
        }
    }
}