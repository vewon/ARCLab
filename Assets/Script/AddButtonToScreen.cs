using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonToScreen : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject canvas; // The Canvas to which the button will be added
    public GameObject menuPrefab;

    private GameObject currentMenu;
    private bool isMenuVisible;

    void Start()
    {
        // Instantiate the button as a child of the Canvas
        GameObject instantiatedButton = Instantiate(buttonPrefab, canvas.transform);

        // Move the button to the top of the screen
        RectTransform rectTransform = instantiatedButton.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(50, -50); // Move button down by 150 pixels

        // Set up the button's click event
        Button buttonComponent = instantiatedButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        if (isMenuVisible) // checks if menu is visible; if yes, then it destroys the menu
        {
            Destroy(currentMenu);
            isMenuVisible = false;
            currentMenu.SetActive(false);
        }
        else
        {
            currentMenu = Instantiate(menuPrefab, canvas.transform);
            RectTransform menuTransform = currentMenu.GetComponent<RectTransform>();
            menuTransform.anchorMin = new Vector2(0.5f, 0.5f);
            menuTransform.anchorMax = new Vector2(0.5f, 0.5f);
            menuTransform.anchoredPosition = new Vector2(0, 0);
            isMenuVisible = true;
            currentMenu.SetActive(true);
            currentMenu.tag = "Menu";
        }
    }
}