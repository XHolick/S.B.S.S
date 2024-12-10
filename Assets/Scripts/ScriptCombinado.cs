using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // For video playback
using TMPro; // For TextMeshPro components
using UnityEngine.UI; // For UI components

public class CombinedButtonScript : MonoBehaviour
{
    [Header("Highlight Settings")]
    public string textoMensagem; // Text to display on hover
    public TextMeshProUGUI telaStart1; // Text display 1
    public TextMeshProUGUI telaStart2; // Text display 2
    public Material materialOriginal; // Store the original material
    public Material materialBrilho; // Highlight material

    private Renderer renderer; // Object's renderer
    private bool useBrightMaterial = false; // Tracks material state for blinking

    [Header("Turbine Rotation")]
    public RotacaoTurbina turbina; // Reference to turbine rotation script

    [Header("Scene Loading")]
    public string sceneToLoad; // Name of the scene to load
    public float fadeDuration = 1f; // Duration of the fade effect
    public Image fadeImage; // UI Image for fading

    [Header("Cutscene Settings")]
    public VideoPlayer cutsceneVideo; // Video Player for cutscene
    public Canvas cutsceneCanvas; // Canvas to display the cutscene (optional)

    [Header("Blink Effect")]
    public float blinkInterval = 0.5f; // Time between blinks
    private bool isBlinking = false; // Tracks blinking state

    private void Start()
    {
        // Initialize renderer
        renderer = GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogError("Renderer not found on GameObject! Blinking won't work.");
            return;
        }

        // Ensure materials are assigned
        if (materialOriginal == null)
        {
            Debug.LogWarning("materialOriginal not assigned! Using the current material as default.");
            materialOriginal = renderer.material;
        }

        if (materialBrilho == null)
        {
            Debug.LogError("materialBrilho not assigned! Blinking won't work.");
            return;
        }

        // Ensure fade image starts fully transparent
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }

        // Ensure cutscene canvas is hidden initially
        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.enabled = false;
        }

        // Start blinking effect
        Debug.Log("Starting blinking effect...");
        StartCoroutine(BlinkMaterial());
    }

    private void OnMouseEnter()
    {
        // Display the message on both screens
        if (telaStart1 != null) telaStart1.text = textoMensagem;
        if (telaStart2 != null) telaStart2.text = textoMensagem;
    }

    private void OnMouseExit()
    {
        // Clear the messages
        if (telaStart1 != null) telaStart1.text = "";
        if (telaStart2 != null) telaStart2.text = "";
    }

    private System.Collections.IEnumerator BlinkMaterial()
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
                    Debug.Log("Switching to bright material");
                    renderer.material = materialBrilho;
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

    private void OnMouseDown()
    {
        // Activate turbine rotation if reference exists
        if (turbina != null)
        {
            turbina.IniciarRotacao();
        }

        // Start fade-out, play cutscene, and load the scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log($"Starting sequence for scene: {sceneToLoad}");
            StartCoroutine(FadeCutsceneAndLoadScene());
        }
        else
        {
            Debug.LogWarning("Scene name is not set! Skipping scene load.");
        }
    }

    private System.Collections.IEnumerator FadeCutsceneAndLoadScene()
    {
        // Fade to black
        if (fadeImage != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / fadeDuration));
                yield return null;
            }
        }

        // Play the cutscene
        if (cutsceneVideo != null)
        {
            if (cutsceneCanvas != null) cutsceneCanvas.enabled = true;

            cutsceneVideo.Play();

            // Wait for the video to finish playing
            while (cutsceneVideo.isPlaying)
            {
                yield return null;
            }

            if (cutsceneCanvas != null) cutsceneCanvas.enabled = false;
        }

        // Fade back in (optional)
        if (fadeImage != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer / fadeDuration));
                yield return null;
            }
        }

        // Load the scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
