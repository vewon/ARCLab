using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

[RequireComponent(typeof(ARTrackedImageManager), typeof(ARRaycastManager))]
public class PlaceTrackedImageOnTouch : MonoBehaviour
{
    // referencing ARTrackedImageManager and ARRaycastManager Components
    private ARTrackedImageManager _trackedImagesManager;
    private ARRaycastManager _raycastManager;

    //array of gameobjects to be instantiated when a tracked image is touched
    public GameObject[] ArPrefabs;

    //list to store ARRaycast hits and tracked images
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<ARTrackedImage> trackedImages = new List<ARTrackedImage>();

    [SerializeField]
    private GameObject detailPanel;

    [SerializeField]
    private TextMeshProUGUI detailText;

    public float thresholdDistance = 0.1f;

    private Dictionary<string, string> imageDetails = new Dictionary<string, string>
    {
        {"CompInfo0", "null"},
        {"CompInfo1", "2k"}
    };

    void Awake()
    {
        //get references to ARTrackedImageManager and ARRaycastManager components
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
        _raycastManager = GetComponent<ARRaycastManager>();

        //check if trackedimages is found
        if (_trackedImagesManager == null)
        {
            Debug.LogError("ARTrackedImageManager component not found.");
        }
        detailPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (_trackedImagesManager != null)
        {
            _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        //enables touch fucntionalities
        Debug.Log("Test Button Work");
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        if (_trackedImagesManager != null)
        {
            _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //handle each tracked image added
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            trackedImages.Add(trackedImage);
            //UpdateImage(trackedImage);
            UpdateInfo(trackedImage);
        }

        //handles removed tracked images
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            trackedImages.Remove(trackedImage);
            //UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateInfo(trackedImage);
        }
    }

    void UpdateInfo(ARTrackedImage trackedImage)
    {
        HoverInfo hoverInfo = trackedImage.GetComponent<HoverInfo>();
        if (hoverInfo != null)
        {
            string newDetails;
            if (imageDetails.TryGetValue(trackedImage.referenceImage.name, out newDetails))
            {
                hoverInfo.SetDetails(newDetails);
            }
            else
            {
                Debug.LogWarning($"Details not found for image {trackedImage.referenceImage.name}");
            }
        }
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        //check if finger index is 0 (first finger)
        if (finger.index != 0) return;

        if (_raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.Image))
        {
            ARRaycastHit closestHit = hits[0];
            foreach (ARRaycastHit hit in hits)
            {
                if (hit.distance < closestHit.distance)
                {
                    closestHit = hit;
                }
            }
            Pose pose = closestHit.pose;

            ARTrackedImage closestTrackedImage = null;
            float minDistance = thresholdDistance;
            foreach (var trackedImage in trackedImages)
            {
                float distance = Vector3.Distance(pose.position, trackedImage.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTrackedImage = trackedImage;
                }
            }

            if (closestTrackedImage != null)
            {
                string details;
                if (imageDetails.TryGetValue(closestTrackedImage.referenceImage.name, out details))
                {
                    detailPanel.SetActive(true);
                    detailText.text = details;
                }
                else
                {
                    Debug.LogWarning($"Details for this image not found {closestTrackedImage.referenceImage.name}");
                    detailPanel.SetActive(false);
                }
            }
        }
        else
        {
            detailPanel.SetActive(false);
        }

        /* OLD CODE
        //perform raycast from finger's current touch position onto tracked images
        if (_raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.Image))
        {
            foreach (ARRaycastHit hit in hits)
            {
                Pose pose = hit.pose;
                foreach (var trackedImage in trackedImages)
                {
                    if (Vector3.Distance(pose.position, trackedImage.transform.position) < thresholdDistance)
                    {
                        string details;
                        if (imageDetails.TryGetValue(trackedImage.referenceImage.name, out details))
                        {
                            detailPanel.SetActive(true);
                            detailText.text = details;
                        }
                        else
                        {
                            Debug.LogWarning($"Details not found for image {trackedImage.referenceImage.name}");
                            detailPanel.SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            detailPanel.SetActive(false);
        }
        */
    }
}
