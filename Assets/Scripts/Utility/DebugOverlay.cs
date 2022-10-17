using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    private void Start()
    {
        if (Debug.isDebugBuild)
        {
            Debug.developerConsoleVisible = true;
            Debug.LogError("Force the build console open...");
        }
    }

    private void OnGUI()
    {
        // If it is a debug build, show the debug overlay
        if (Debug.isDebugBuild)
        {
            // Display FPS in top left corner
            // Set text size and color to yellow
            GUI.skin.label.fontSize = 20;
            GUI.skin.label.normal.textColor = Color.yellow;
            GUI.Label(new Rect(10, 10, 200, 40), "FPS: " + (1.0f / Time.deltaTime).ToString("F2"));
        }
    }
}