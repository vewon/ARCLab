using UnityEngine;
using UnityEngine.UI;

public class AddButtonToScreen : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject canvas; // The Canvas to which the button will be added

    void Start()
    {
        // Instantiate the button as a child of the Canvas
        GameObject instantiatedButton = Instantiate(buttonPrefab, canvas.transform);

        // Move the button to the top of the screen
        RectTransform rectTransform = instantiatedButton.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1);
        rectTransform.anchorMax = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = new Vector2(0, -150); // Move button down by 150 pixels

        // Set up the button's click event
        Button buttonComponent = instantiatedButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        Debug.Log("Button clicked!");
    }
}