using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypeTextAnimation : MonoBehaviour {
    public event Action TypeFinished;

    [SerializeField] private float typeDelay = 0.05f;
    [SerializeField] private TextMeshProUGUI textObject;

    private string fullText;
    private Coroutine typingCoroutine;

    public void SetFullText(string text) {
        fullText = text;
    }

    public void StartTyping() {
        if (typingCoroutine != null) {
            StopTyping();
        }

        if (string.IsNullOrEmpty(fullText) || textObject == null) {
            Debug.LogWarning("Cannot start typing: Text is null or empty.");
            return;
        }

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText() {
        textObject.text = fullText;
        textObject.maxVisibleCharacters = 0;

        for (int i = 0; i <= fullText.Length; i++) {
            textObject.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typeDelay);
        }

        typingCoroutine = null;
        TypeFinished?.Invoke();
    }

    public void Skip() {
        if (typingCoroutine != null) {
            StopTyping();
            textObject.maxVisibleCharacters = fullText.Length;
            TypeFinished?.Invoke();
        }
    }

    private void StopTyping() {
        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }
}
