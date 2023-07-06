using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    public Button switchButton;

    void Start()
    {
        switchButton.onClick.AddListener(Switch);
    }

    private void Switch()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "ObjectIdentification")
        {
            SceneManager.LoadScene("CircuitSimulator");
        }
        else
        {
            SceneManager.LoadScene("ObjectIdentification");
        }
    }
}
