using UnityEngine;

/*
public class HoverInfo : MonoBehaviour
{
    // String to hold the details about this object
    [SerializeField]
    private string details = "Default detail information";

    // Function to allow other scripts to get the details
    public string GetDetails()
    {
        return details;
    }
}
*/

public class HoverInfo : MonoBehaviour
{
    [SerializeField]
    private string details = "Default detail information";

    public void SetDetails(string newDetails)
    {
        details = newDetails;
    }

    public string GetDetails()
    {
        return details;
    }
}