using UnityEngine;

public class PlayerMovementLimiter : MonoBehaviour
{
    // Define the movement boundaries
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    void Update()
    {
        // Clamp the player's position within the boundaries
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

        // Apply the clamped position back to the player
        transform.position = clampedPosition;
    }
}
