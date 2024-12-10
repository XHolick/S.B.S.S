using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class RenderTextureSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;         // The VideoPlayer component
    public RenderTexture renderTexture;    // The RenderTexture for video playback
    public VideoClip loopingVideo;         // The background looping video
    public VideoClip transitionVideo;      // The transition video
    public string nextSceneName;           // The name of the next scene

    private bool isTransitioning = false;  // Flag to prevent multiple triggers

    void Start()
    {
        if (videoPlayer == null || renderTexture == null)
        {
            Debug.LogError("VideoPlayer or RenderTexture not assigned!");
            return;
        }

        // Configure VideoPlayer to use the RenderTexture
        videoPlayer.targetTexture = renderTexture;

        // Set the initial video to the looping video
        videoPlayer.clip = loopingVideo;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

    public void PlayTransitionVideoAndLoadScene()
    {
        if (isTransitioning)
            return; // Prevent multiple clicks

        isTransitioning = true;

        // Stop looping and play the transition video
        videoPlayer.isLooping = false;
        videoPlayer.clip = transitionVideo;
        videoPlayer.Play();

        // Subscribe to the event when the video finishes
        videoPlayer.loopPointReached += OnTransitionVideoEnd;
    }

    private void OnTransitionVideoEnd(VideoPlayer vp)
    {
        // Load the next scene when the transition video ends
        SceneManager.LoadScene(nextSceneName);
    }
}
