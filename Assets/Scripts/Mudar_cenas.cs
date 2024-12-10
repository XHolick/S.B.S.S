using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class ChangeSceneButton : MonoBehaviour
{
    public string targetSceneName; 
    public GameObject confirmationDialog; 
    public Button yesButton; 
    public Button cancelButton; 

    void Start()
    {

        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false);
        }

        if (yesButton != null)
        {
            yesButton.onClick.AddListener(ChangeScene);
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
            confirmationDialog.SetActive(true); 
        }
    }

    private void HideConfirmationDialog()
    {
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false); 
        }
    }

    private void ChangeScene()
    {

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("Target scene name is not set!");
        }
    }
}
