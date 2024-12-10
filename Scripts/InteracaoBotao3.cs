using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // For scene switching

public class InteracaoBotao3 : MonoBehaviour
{
    public string textoMensagem; 
    public TextMeshProUGUI telaCr1;
    public TextMeshProUGUI telaCr2; 
    
    private Material materialOriginal;
    public Material materialBrilho;

    private Renderer renderer;

    [Header("Blink Effect")]
    public float blinkInterval = 0.5f; // Time between blinks
    private bool isBlinking = false;  // Tracks blinking state
    private bool useBrightMaterial = false; // Tracks material state for blinking

    // Scene name for the settings menu
    public string nomeCenaConfiguracoes = "MenuConfiguracoes";

    private void Start()
    {
        renderer = GetComponent<Renderer>();

        // Validate renderer and materials
        if (renderer != null)
        {
            materialOriginal = renderer.material;
        }
        else
        {
            Debug.LogError("Renderer is missing! Blinking effect will not work.");
        }

        if (materialBrilho == null)
        {
            Debug.LogError("MaterialBrilho is not assigned! Blinking effect will not work.");
        }

        // Start the blinking effect
        Debug.Log("Starting automatic blinking effect...");
        StartCoroutine(BlinkMaterial());
    }

    private void OnMouseEnter()
    {
        // Set the bright material
        if (renderer != null && materialBrilho != null)
        {
            renderer.material = materialBrilho;
        }

        // Display message on both screens
        if (telaCr1 != null) telaCr1.text = textoMensagem;
        if (telaCr2 != null) telaCr2.text = textoMensagem;
    }

    private void OnMouseExit()
    {
        // Revert to the original material
        if (renderer != null)
        {
            renderer.material = materialOriginal;
        }

        // Clear the messages
        if (telaCr1 != null) telaCr1.text = "";
        if (telaCr2 != null) telaCr2.text = "";
    }

    private void OnMouseDown()
    {
        // Load the settings menu scene
        if (!string.IsNullOrEmpty(nomeCenaConfiguracoes))
        {
            Debug.Log($"Loading scene: {nomeCenaConfiguracoes}");
            SceneManager.LoadScene(nomeCenaConfiguracoes);
        }
        else
        {
            Debug.LogWarning("Scene name for configurations is empty!");
        }
    }

    private System.Collections.IEnumerator BlinkMaterial()
    {
        isBlinking = true;

        while (isBlinking)
        {
            if (renderer != null)
            {
                // Alternate between original and bright materials
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
}
