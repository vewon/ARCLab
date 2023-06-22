using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceTrackedImages : MonoBehaviour
{
    private ARTrackedImageManager _trackedImagesManager;

    public GameObject[] ArPrefabs;

    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();

    void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
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
    }

    private void OnDisable()
    {
        if (_trackedImagesManager != null)
        {
            _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.updated)
        {
            var imageName = trackedImage.referenceImage.name;
            GameObject curPrefab = Array.Find(ArPrefabs, prefab => prefab.name == imageName);

            if (curPrefab != null)
            {
                if (!_instantiatedPrefabs.ContainsKey(imageName))
                {
                    var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                    _instantiatedPrefabs[imageName] = newPrefab;
                }

                _instantiatedPrefabs[imageName].SetActive(trackedImage.trackingState == TrackingState.Tracking);
            }
            else
            {
                Debug.LogWarning($"No prefab found for image {imageName}");
            }
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            var imageName = trackedImage.referenceImage.name;
            if (_instantiatedPrefabs.ContainsKey(imageName))
            {
                Destroy(_instantiatedPrefabs[imageName]);
                _instantiatedPrefabs.Remove(imageName);
            }
            else
            {
                Debug.LogWarning($"No instantiated prefab found for image {imageName}");
            }
        }
    }
}
