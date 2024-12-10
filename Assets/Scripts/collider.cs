using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} entered the destroyer.");

        // Destroy the object
        Destroy(other.gameObject);
    }
}
