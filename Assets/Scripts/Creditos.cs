using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [Header("Hover Settings")]
    public string hoverMessage;
    public TextMeshProUGUI hoverText1;
    public TextMeshProUGUI hoverText2;
    public Material materialOriginal;
    public Material materialHighlight;
    private Renderer renderer;

    [Header("Blink Effect")]
    public float blinkInterval = 0.5f; // Time between blinks
    private bool isBlinking = false;  // Tracks blinking state
    private bool useBrightMaterial = false; // Tracks material state for blinking

    [Header("Credits Canvas")]
    public GameObject creditsCanvas;
    public float animationDuration = 0.5f;
    public Vector3 maxScale = Vector3.one;
    private Vector3 initialScale = Vector3.zero;
    private CanvasGroup canvasGroup;

    private bool creditsVisible = false; // Tracks if the credits canvas is visible
    private bool hoverEnabled = true;   // Controls hover behavior

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            materialOriginal = renderer.material;
        }

        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(false);
            creditsCanvas.transform.localScale = initialScale;
            canvasGroup = creditsCanvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = creditsCanvas.AddComponent<CanvasGroup>();
            }
            canvasGroup.blocksRaycasts = false;
        }

        // Start the blinking effect automatically
        Debug.Log("Starting automatic blinking effect...");
        StartCoroutine(BlinkMaterial());
    }

    private IEnumerator BlinkMaterial()
    {
        isBlinking = true;

        while (isBlinking)
        {
            if (renderer != null)
            {
                // Alternate materials based on the state
                if (useBrightMaterial)
                {
                    Debug.Log("Switching to original material");
                    renderer.material = materialOriginal;
                }
                else
                {
                    Debug.Log("Switching to highlight material");
                    renderer.material = materialHighlight;
                }

                // Toggle the state
                useBrightMaterial = !useBrightMaterial;
            }
            else
            {
                Debug.LogError("Renderer is null! Stopping blinking.");
                yield break;
            }

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void OnMouseEnter()
    {
        if (hoverEnabled)
        {
            if (renderer != null && materialHighlight != null)
            {
                renderer.material = materialHighlight;
            }

            if (hoverText1 != null) hoverText1.text = hoverMessage;
            if (hoverText2 != null) hoverText2.text = hoverMessage;
        }
    }

    private void OnMouseExit()
    {
        // If hover is enabled, reset the material
        if (hoverEnabled)
        {
            if (renderer != null)
            {
                renderer.material = materialOriginal;
            }
        }

        // Clear hover messages regardless of hoverEnabled state
        if (hoverText1 != null) hoverText1.text = "";
        if (hoverText2 != null) hoverText2.text = "";
    }

    private void OnMouseDown()
    {
        if (!creditsVisible)
        {
            OpenCredits();
        }
        else
        {
            CloseCredits();
        }
    }

    private void OpenCredits()
    {
        if (creditsCanvas != null)
        {
            creditsCanvas.SetActive(true);
            canvasGroup.blocksRaycasts = true;
            StartCoroutine(AnimateCanvas(creditsCanvas.transform, initialScale, maxScale, animationDuration));
            creditsVisible = true;

            // Disable hover effects and ensure the highlight material remains
            hoverEnabled = false;
            if (renderer != null && materialHighlight != null)
            {
                renderer.material = materialHighlight;
            }
        }
    }

    private void CloseCredits()
    {
        if (creditsCanvas != null)
        {
            StartCoroutine(AnimateCanvas(creditsCanvas.transform, maxScale, initialScale, animationDuration, () =>
            {
                creditsCanvas.SetActive(false);
                canvasGroup.blocksRaycasts = false;
                creditsVisible = false;

                // Re-enable hover effects and reset material
                hoverEnabled = true;
                if (renderer != null)
                {
                    renderer.material = materialOriginal;
                }
            }));
        }
    }

    private IEnumerator AnimateCanvas(Transform target, Vector3 startScale, Vector3 endScale, float duration, System.Action onComplete = null)
    {
        float elapsed = 0f;
        target.localScale = startScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            target.localScale = Vector3.Lerp(startScale, endScale, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        target.localScale = endScale;
        onComplete?.Invoke();
    }
}
