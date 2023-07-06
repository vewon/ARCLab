using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenComponent : MonoBehaviour
{
    public GameObject Components;

    public void ComponentsOpen()
    {
        if (Components != null)
        {
            bool isActive = Components.activeSelf;
            Components.SetActive(!isActive);
        }
    }
}
