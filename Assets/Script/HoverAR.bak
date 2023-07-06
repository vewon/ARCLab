using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class HoverAR : MonoBehaviour
{
    //ARRaycast manager to handle raycasts in AR space
    ARRaycastManager arRaycastManager;
    //list to hold ar raycast hits
    static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    //reference to ar camera
    [SerializeField]
    private Camera arCamera;

    //reference to panel that displays details
    [SerializeField]
    private GameObject detailPanel;

    //reference to text element that will show details
    [SerializeField]
    private TextMeshProUGUI detailText;

    void Start()
    {
        // Get the ARRaycastManager from the current object (AR Session Origin)
        arRaycastManager = GetComponent<ARRaycastManager>();
        // Initially, set the details panel as inactive
        detailPanel.SetActive(false);
    }

    void Update()
    {
        // Calculate the center of the screen
        Vector2 centerOfScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        // If a raycast from the center of the screen hits an AR trackable
        if (arRaycastManager.Raycast(centerOfScreen, raycastHits, TrackableType.AllTypes))
        {
            // Get the pose where the raycast hit
            Pose hitPose = raycastHits[0].pose;
            // Get the game object that was hit
            var hitObject = raycastHits[0].trackable.transform.gameObject;
            // Try to get the HoverInfo component from the hit object
            var hitInfo = hitObject.GetComponent<HoverInfo>();

            // If a HoverInfo component was found, the object is interactive
            if (hitInfo != null)
            {
                // Show the details panel and set the text to the object's details
                detailPanel.SetActive(true);
                detailText.text = hitInfo.GetDetails();
            }
            // If the object is not interactive
            else
            {
                // Hide the details panel
                detailPanel.SetActive(false);
            }
        }
        // If no AR object was hit by the raycast
        else
        {
            // Hide the details panel
            detailPanel.SetActive(false);
        }
    }
}
