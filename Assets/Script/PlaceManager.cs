using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    private TargetIndicator targetIndicator;
    public GameObject ObjectToPlace;

    private GameObject newTargetObject;

    // Start is called before the first frame update
    void Start()
    {
        targetIndicator = FindObjectOfType<TargetIndicator>(); 
    }

    public void ClickToPlace()
    {
        newTargetObject = Instantiate(ObjectToPlace, targetIndicator.transform.position, targetIndicator.transform.rotation);
    }
}
