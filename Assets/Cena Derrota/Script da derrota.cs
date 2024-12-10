using UnityEngine;
using UnityEngine.Video;

public class VideoLooper : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public float loopDuration = 1.0f; // Duration of the loop (in seconds)

    private double loopStartTime;

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer not assigned!");
            return;
        }

        // Calculate the start time of the loop
        loopStartTime = videoPlayer.clip.length - loopDuration;

        // Subscribe to the VideoPlayer's loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Set the VideoPlayer's time to the loop start time
        vp.time = loopStartTime;
        vp.Play();
    }
}
