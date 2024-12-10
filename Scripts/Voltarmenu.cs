using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // For UI components

public class ReturnToMenuButton : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; // Name of the main menu scene
    public GameObject confirmationDialog; // The dialog UI for confirmation
    public Button yesButton; // Button to confirm the return to menu
    public Button cancelButton; // Button to cancel the action

    void Start()
    {
        // Ensure the confirmation dialog is hidden initially
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false);
        }

        // Assign button functionalities
        if (yesButton != null)
        {
            yesButton.onClick.AddListener(ReturnToMainMenu);
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

    private void ReturnToMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName); // Load the main menu scene
        }
        else
        {
            Debug.LogError("Main menu scene name is not set!");
        }
    }
}
