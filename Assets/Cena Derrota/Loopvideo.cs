using UnityEngine;
using UnityEngine.Video;

public class Loopvideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer
    public float loopDuration = 1.0f; // Duration of the loop (in seconds)

    private double loopStartTime; // Time to start the loop

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned!");
            return;
        }

        // Ensure the VideoPlayer is set to not loop automatically
        videoPlayer.isLooping = false;

        // Calculate the start time of the loop
        loopStartTime = videoPlayer.clip.length - loopDuration;

        // Subscribe to the VideoPlayer's loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video ended. Starting the loop...");

        // Set the VideoPlayer's time to the loop start time
        vp.time = loopStartTime;
        vp.Play();
    }
}
