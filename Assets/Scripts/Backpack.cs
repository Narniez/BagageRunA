using UnityEngine;

public class Backpack : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is a package (use a tag or layer)
        if (collision.gameObject.CompareTag("Package"))
        {
            // Parent the package to the backpack to move with it
            collision.transform.SetParent(transform);

            // Optionally, adjust Rigidbody2D properties
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Reduce gravity or make kinematic to stabilize
                rb.gravityScale = 0; // Prevent further falling
                rb.linearVelocity = Vector2.zero; // Stop movement
                rb.isKinematic = true; // Optional: Make kinematic to prevent physics interactions
            }
        }
    }
}
