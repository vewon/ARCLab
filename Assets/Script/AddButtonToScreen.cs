using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class AddButtonToScreen : MonoBehaviour
{
    public GameObject buttonPrefab;
    public ARRaycastManager raycastManager;
    public Camera arCamera;

    private List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    private GameObject instantiatedButton;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);

            if (raycastManager.Raycast(touch.position, s_Hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = s_Hits[0].pose;

                Vector3 screenPosition = new Vector3(Screen.width / 2f, Screen.height, 0f);
                Vector3 worldPosition = arCamera.ScreenToWorldPoint(screenPosition);

                instantiatedButton = Instantiate(buttonPrefab, worldPosition, hitPose.rotation);
                instantiatedButton.transform.SetParent(transform);

                Button buttonComponent = instantiatedButton.GetComponent<Button>();
                buttonComponent.onClick.AddListener(OnButtonClicked);
            }
        }
    }

    void OnButtonClicked()
    {
        Debug.Log("Button clicked!");
    }
}
