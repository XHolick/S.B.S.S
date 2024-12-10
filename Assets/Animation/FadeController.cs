using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private int imageCutscene;
    [SerializeField] private Animator[] animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            try
            {
            animator[imageCutscene].SetTrigger("Fade");
            imageCutscene++;
            }
            catch { }

        }
    }

}
