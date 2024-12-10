using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TiltShiftController : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;

    public float tiltShiftStrength = 2f;

    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out depthOfField);
    }

    void Update()
    {
        if (depthOfField != null)
        {
            depthOfField.focusDistance.value = Mathf.Lerp(depthOfField.focusDistance.value, tiltShiftStrength, Time.deltaTime);
        }
    }
}
