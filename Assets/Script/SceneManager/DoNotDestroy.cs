using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    public GameObject[] objectsToKeep;
    void Awake()
    {
        foreach (GameObject obj in objectsToKeep)
        {
            DontDestroyOnLoad(obj);
        }
    }
}
