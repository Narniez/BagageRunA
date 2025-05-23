using UnityEngine;

public class PackageCollisionHandler : MonoBehaviour
{
    private PackageHandler packageHandler;
    private Rigidbody2D rb;

    public void SetPackageHandler(PackageHandler handler, Rigidbody2D rigidbody)
    {
        packageHandler = handler;
        rb = rigidbody;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Notify PackageHandler if colliding with backpack or another package
        if (collision.gameObject.CompareTag("Backpack") || collision.gameObject.CompareTag("Package"))
        {
            packageHandler?.OnPackageCollision(rb, transform);
        }
    }
}
