using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Reference to the character's transform.
    public float smoothSpeed = 0.125f;  // The speed at which the camera follows the character.

    // Offset to control the camera's position relative to the character.
    public Vector3 offset = new Vector3(0, 0, -10);

    public float verticalOffset = 2.0f; // Adjust this value to control the vertical camera offset.

    void LateUpdate()
    {
        // Calculate the desired position for the camera.
        Vector3 desiredPosition = target.position + offset;

        // Check for 'W' key input to move the camera up.
        if (Input.GetKey("w"))
        {
            desiredPosition.y += verticalOffset;
        }
        // Check for 'S' key input to move the camera down.
        else if (Input.GetKey("s"))
        {
            desiredPosition.y -= verticalOffset;
        }

        // Use SmoothDamp to gradually move the camera to the desired position.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position.
        transform.position = smoothedPosition;
    }
}
