using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material material1; // Assign the first material in the inspector
    public Material material2; // Assign the second material in the inspector
    public float switchInterval = 1f; // Time in seconds between switches

    private Renderer objRenderer;
    private bool isMaterial1Active = true;
    private float timer;

    void Start()
    {
        // Get the Renderer component attached to the object
        objRenderer = GetComponent<Renderer>();

        // Set the initial material
        if (objRenderer != null && material1 != null)
        {
            objRenderer.material = material1;
        }
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Switch materials if the interval has elapsed
        if (timer >= switchInterval)
        {
            timer = 0f; // Reset the timer
            ToggleMaterial();
        }
    }

    void ToggleMaterial()
    {
        if (objRenderer != null)
        {
            // Switch between the two materials
            objRenderer.material = isMaterial1Active ? material2 : material1;
            isMaterial1Active = !isMaterial1Active;
        }
    }
}
