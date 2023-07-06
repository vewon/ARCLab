using UnityEngine;
using TMPro; // Include the TextMeshPro namespace
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown; // Use TMP_Dropdown instead of Dropdown
    public Button button1;
    public Button button2;

    void Start()
    {
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                // Code to change UI for the first option
                button1.gameObject.SetActive(true);
                button2.gameObject.SetActive(false);
                break;
            case 1:
                // Code to change UI for the second option
                button1.gameObject.SetActive(false);
                button2.gameObject.SetActive(true);
                break;
            default:
                // Optional default case
                break;
        }
    }
}
