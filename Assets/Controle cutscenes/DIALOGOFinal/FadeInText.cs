using UnityEngine;
using UnityEngine.UI; // For UI Text
using TMPro; // Uncomment if using TextMeshPro

public class FadeInText : MonoBehaviour
{
    public float fadeInTime = 2f; // Time it takes to fade in
    private float timer = 0f;
    private Text uiText; // For UI Text
    private TextMeshProUGUI tmpText; // For TextMeshPro
    private bool fadeStarted = false;

    void Start()
    {
        // Get the text component
        uiText = GetComponent<Text>();
        tmpText = GetComponent<TextMeshProUGUI>();
        if (uiText != null)
            uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, 0f);
        if (tmpText != null)
            tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5f && !fadeStarted) // Wait 5 seconds before starting fade
        {
            fadeStarted = true;
            StartCoroutine(FadeIn());
        }
    }

    System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeInTime);

            if (uiText != null)
                uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, alpha);
            if (tmpText != null)
                tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, alpha);

            yield return null;
        }
    }
}
