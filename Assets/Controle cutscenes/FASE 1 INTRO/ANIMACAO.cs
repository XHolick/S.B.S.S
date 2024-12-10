using UnityEngine;

public class StopAnimationAtEnd : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        if (animator != null)
        {
            // Automatically disable Animator after the animation plays
            animator.Play("SlideInAnimation");
            Invoke("DisableAnimator", animator.GetCurrentAnimatorStateInfo(1).length);
        }
    }

    void DisableAnimator()
    {
        animator.enabled = false;
    }
}
