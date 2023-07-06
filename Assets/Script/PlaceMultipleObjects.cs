using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMultipleObjects : MonoBehaviour
{
    private TargetIndicator targetIndicator;
    private GameObject objectToPlace;

    public GameObject objectFirst;
    public GameObject objectSecond;
    public GameObject objectThird;

    // Start is called before the first frame update
    void Start()
    {
        targetIndicator = FindObjectOfType<TargetIndicator>();
    }

    public void InstantiateObject(Quaternion rotation)
    {
        Instantiate(objectToPlace, targetIndicator.transform.position, rotation);
    }

    public void ClickToPlaceFirst()
    {
        objectToPlace = objectFirst;
        Quaternion correctRotation = targetIndicator.transform.rotation * Quaternion.Euler(0, -90, 0); // Adjust this as needed
        InstantiateObject(correctRotation);
    }

    public void ClickToPlaceSecond()
    {
        objectToPlace = objectSecond;
        Quaternion correctRotation = targetIndicator.transform.rotation * Quaternion.Euler(-90, 0, 0);
        InstantiateObject(correctRotation);
    }

    public void ClickToPlaceThird()
    {
        objectToPlace = objectThird;
        InstantiateObject(targetIndicator.transform.rotation);
    }
}
