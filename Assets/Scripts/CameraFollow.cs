using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;         // Reference to the player's position
    public Vector3 offset;           // Offset between the camera and player
    public float rotationSpeed = 5f; // Speed of camera rotation
    public float distance = 10f;     // Distance of the camera from the player

    private float currentRotation = 0f; // Current rotation of the camera
    private float currentHeight = 5f;   // Height at which the camera follows the player

    void Start()
    {
        // Set initial offset if it's not provided
        if (offset == Vector3.zero)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        // Get horizontal player input for rotating the camera
        float horizontalInput = Input.GetAxis("Horizontal");

        // Update camera's rotation based on input (left/right movement)
        currentRotation += horizontalInput * rotationSpeed * Time.deltaTime;

        // Ensure camera remains at a set height
        currentHeight = offset.y;

        // Calculate the new position of the camera
        Vector3 direction = new Vector3(0, currentHeight, -distance);
        Quaternion rotation = Quaternion.Euler(0, currentRotation, 0); // Rotate camera around the Y axis
        transform.position = player.position + rotation * direction;  // Apply rotation and offset

        // Make the camera look at the player
        transform.LookAt(player);
    }
}
