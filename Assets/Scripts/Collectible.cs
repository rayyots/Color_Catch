using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Adjustable values in the Inspector
    public float rotationSpeed = 45f;   // Rotation speed in degrees per second
    public float hoverHeight = 0.5f;    // How high the collectible will hover above its start position
    public float hoverSpeed = 2f;       // Speed of the hovering (controls how fast the object moves up and down)

    private Vector3 startPosition;      // Store the original position of the collectible

    void Start()
    {
        // Store the original position of the collectible when the game starts
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotation: Apply continuous rotation to the collectible
        RotateCollectible();

        // Hovering: Apply a sinusoidal motion to make the collectible move up and down smoothly
        HoverCollectible();
    }

    // Function to rotate the collectible around its y-axis (vertical axis)
    void RotateCollectible()
    {
        // Rotate the object around the y-axis at the specified speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    // Function to make the collectible hover up and down
    void HoverCollectible()
    {
        // Use a sine wave function to create smooth up and down motion
        // Mathf.Sin creates a wave pattern, and we multiply by hoverHeight to control the movement range
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Apply the hover offset to the object's position, keeping it in the x and z dimensions
        transform.position = startPosition + new Vector3(0, hoverOffset, 0);
    }
}
