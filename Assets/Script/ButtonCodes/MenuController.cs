using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        // Fetch all the buttons in the scene
        Button[] buttons = FindObjectsOfType<Button>();

        // Assign a click event to each button
        foreach (var button in buttons)
        {
            button.onClick.AddListener(delegate { OnButtonClick(button.name); });
        }
    }

    void OnButtonClick(string buttonName)
    {
        // Depending on the button's name, perform an action
        switch (buttonName)
        {
            case "Simulator":
                // Open Circuit Simulator Mode
                SceneManager.LoadScene("CircuitSimulator");
                break;
            case "Identifier":
                // Open Object Identifier Mode
                SceneManager.LoadScene("ObjectIdentification");
                break;
            case "Exit":
                // Quit the game
                Application.Quit();
                break;
        }
    }
}
