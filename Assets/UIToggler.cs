using UnityEngine;

public class UIToggle : MonoBehaviour
{
    public GameObject uiToToggle; // The UI to show/hide
    public GameObject uiToToggleBack; // The UI to hide/show

    private bool isVisible = false;

    private void Start()
    {
        // Ensure initial states
        if (uiToToggle != null)
            uiToToggle.SetActive(false);

        if (uiToToggleBack != null)
            uiToToggleBack.SetActive(true);
    }

    public void ToggleUI()
    {
        // Flip once
        isVisible = !isVisible;

        // Apply the same state logic
        if (uiToToggle != null)
            uiToToggle.SetActive(isVisible);

        if (uiToToggleBack != null)
            uiToToggleBack.SetActive(!isVisible);
    }
}
