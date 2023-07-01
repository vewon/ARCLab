using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARTrackedImageManager), typeof(ARRaycastManager))]
public class PlaceTrackedImageOnTouch : MonoBehaviour
{
    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA

    [Serializable]
    public struct PrefabDetails
    {
        public GameObject Prefab;
        public string Details;
    }

    [SerializeField]
    private PrefabDetails[] ArPrefabsWithDetails;

    //BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB

    // referencing ARTrackedImageManager and ARRaycastManager Components
    private ARTrackedImageManager _trackedImagesManager;
    private ARRaycastManager _raycastManager;

    //array of gameobjects to be instantiated when a tracked image is touched
    public GameObject[] ArPrefabs;

    //list to store ARRaycast hits and tracked images
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<ARTrackedImage> trackedImages = new List<ARTrackedImage>();

    // Define the threshold distance
    public float thresholdDistance = 0.1f;

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
        foreach (var trackedImage in eventArgs.added)
        {
            trackedImages.Add(trackedImage);
        }

        //handles removed tracked images
        foreach (var trackedImage in eventArgs.removed)
        {
            trackedImages.Remove(trackedImage);
        }
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        //check if finger index is 0 (first finger)
        if (finger.index != 0) return;

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
                        var imageName = trackedImage.referenceImage.name;
                        GameObject curPrefab = Array.Find(ArPrefabs, prefab => prefab.name == imageName);
                        if (curPrefab != null)
                        {
                            var newPrefab = Instantiate(curPrefab, pose.position, pose.rotation);
                        }
                    }
                }
            }
        }
        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA

        foreach (ARRaycastHit hit in hits)
        {
            Pose pose = hit.pose;
            foreach (var trackedImage in trackedImages)
            {
                if (Vector3.Distance(pose.position, trackedImage.transform.position) < thresholdDistance)
                {
                    var imageName = trackedImage.referenceImage.name;
                    PrefabDetails curPrefabDetails = Array.Find(ArPrefabsWithDetails, prefabDetails => prefabDetails.Prefab.name == imageName);
                    if (curPrefabDetails.Prefab != null)
                    {
                        var newPrefab = Instantiate(curPrefabDetails.Prefab, pose.position, pose.rotation);
                        var hoverInfo = newPrefab.AddComponent<HoverInfo>();
                        hoverInfo.SetDetails(curPrefabDetails.Details);
                    }
                }
            }
        }

        //BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB
    }
}
