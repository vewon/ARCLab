using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public string InfoText;

    private void Start()
    {
        // Set the info text when the panel starts
        var textComponent = GetComponentInChildren<Text>();
        if (textComponent != null)
        {
            textComponent.text = InfoText;
        }
        else
        {
            Debug.LogWarning("No Text component found in children of InfoPanel");
        }
    }
}
