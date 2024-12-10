using UnityEngine;

public class PlayScale : MonoBehaviour
{
    public Animator animator; // Reference to the Animator
    private int enterPressCount = 0; // Tracks how many times Enter is pressed
    private bool hasPlayed = false; // Ensures the animation plays only once

    void Update()
    {
        // If the animation has already played, stop checking
        if (hasPlayed) return;

        if (Input.GetKeyDown(KeyCode.Return)) // Listen for Enter key presses
        {
            enterPressCount++;
        }

        if (enterPressCount >= 4) 
        {
            animator.SetTrigger("SlideIn");
            hasPlayed = true; // Mark as played
            Die();
        }
    }
    private void Die()
    { 
     
    
    
    
    }
}
