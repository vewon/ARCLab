using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

[RequireComponent(typeof(ARTrackedImageManager), typeof(ARRaycastManager))]
public class ShowDetailsOnTouch : MonoBehaviour
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

    private Dictionary<string, string> imageDetails = new Dictionary<string, string>();

    // container class to hold keys and values for JSON serialization
    [Serializable]
    public class ImageDetailsContainer
    {
        public List<string> keys = new List<string>();
        public List<string> values = new List<string>();
    }

    // expose imageDetails to be accessed in OpenAIController
    public Dictionary<string, string> ImageDetails
    {
        get { return imageDetails; }
    }

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
        StartCoroutine(LoadImageDetailsFromJSON());
        detailPanel.SetActive(false);
    }

    public IEnumerator LoadImageDetailsFromJSON()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "imageDetails.json");

        using (UnityWebRequest www = UnityWebRequest.Get(path))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"Unable to load image details: {www.error}");
            }
            else
            {
                ImageDetailsContainer imageDetailsContainer = JsonUtility.FromJson<ImageDetailsContainer>(www.downloadHandler.text);

                imageDetails.Clear();

                for (int i = 0; i < imageDetailsContainer.keys.Count; i++)
                {
                    imageDetails.Add(imageDetailsContainer.keys[i], imageDetailsContainer.values[i]);
                }
            }
        }
    }

    private void OnEnable()
    {
        if (_trackedImagesManager != null)
        {
            _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        //enables touch fucntionalities
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.TouchSimulation.Enable();
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
    }
}