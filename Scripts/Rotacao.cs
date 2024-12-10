using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object on its Y-axis (vertical axis)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}