using UnityEngine;
using UnityEngine.SceneManagement; // For managing scenes
using UnityEngine.UI; // For UI components

public class RestartButton : MonoBehaviour
{
    public GameObject confirmationDialog; // The dialog UI for confirmation
    public Button yesButton; // Button to confirm reset
    public Button cancelButton; // Button to cancel reset

    void Start()
    {
        // Ensure the confirmation dialog is hidden initially
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false);
        }

        // Assign the Yes and Cancel button functionalities
        if (yesButton != null)
        {
            yesButton.onClick.AddListener(RestartScene);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(HideConfirmationDialog);
        }
    }

    public void ShowConfirmationDialog()
    {
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(true); // Show the dialog
        }
    }

    private void HideConfirmationDialog()
    {
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false); // Hide the dialog
        }
    }

    private void RestartScene()
    {
        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
