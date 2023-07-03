using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARTrackedImageManager), typeof(ARRaycastManager))]
public class PlaceTrackedImageOnTouch : MonoBehaviour
{
    private ARTrackedImageManager _trackedImagesManager;
    private ARRaycastManager _raycastManager;
    public GameObject[] ArPrefabs;
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    private List<ARTrackedImage> trackedImages = new List<ARTrackedImage>();
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public float thresholdDistance = 0.1f;

    void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
        _raycastManager = GetComponent<ARRaycastManager>();

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

        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            trackedImages.Add(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            trackedImages.Remove(trackedImage);
        }
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0) return;

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
                        if (curPrefab != null && !spawnedObjects.ContainsKey(imageName))
                        {
                            var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                            spawnedObjects[imageName] = newPrefab;
                        }
                    }
                }
            }
        }
    }
}
