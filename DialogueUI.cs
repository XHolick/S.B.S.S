using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] public TextMeshProUGUI dialogueText;

    public void Enable() {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Disable() {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void SetDialogue(string text) {
        dialogueText.text = text;
    }
}
