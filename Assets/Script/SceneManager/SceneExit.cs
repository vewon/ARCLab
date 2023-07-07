using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour
{
    public Button switchButton;

    void Start()
    {
        switchButton.onClick.AddListener(Switch);
    }

    private void Switch()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
