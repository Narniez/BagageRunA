using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageHandler : MonoBehaviour
{

    public static PackageHandler Instance { get; private set; }


    [SerializeField] private GameObject[] packagePrefabs;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Transform backpackTransform;
    [SerializeField] private Transform spawnTransform;

    [SerializeField] private float trackingStrength = 2f;
    [SerializeField] private float maxTrackingSpeed = 5f;
    [SerializeField] private float targetUpdateInterval = 0.5f;

    [SerializeField] private List<Rigidbody2D> spawnedPackages = new List<Rigidbody2D>();

    [SerializeField] private bool facingRight = true;
    [SerializeField] private float throwSpeed = 5f;
    [SerializeField] private float throwHeight = 3f;
    [SerializeField] private float throwOffsetY = 1f;

    private void Start()
    {
        //InvokeRepeating(nameof(SpawnPackage), 0f, spawnInterval);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPackage();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            PopLastPackage();
        }
    }


    public void SpawnPackage()
    {
        // Randomly select a package prefab
        GameObject package = packagePrefabs[Random.Range(0, packagePrefabs.Length)];

        // Spawn at spawnTransform position
        Vector2 spawnPosition = spawnTransform.position;
        spawnPosition.x += Random.Range(-0.5f, 0.5f);

        Quaternion spawnRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        GameObject instantiatedPackage = Instantiate(package, spawnPosition, spawnRotation);
        Rigidbody2D rb = instantiatedPackage.GetComponent<Rigidbody2D>();
        spawnedPackages.Add(rb);
        PackageCollisionHandler collisionHandler = instantiatedPackage.GetComponent<PackageCollisionHandler>();
        if (collisionHandler != null)
        {
            collisionHandler.SetPackageHandler(this, rb);
        }
    }

    public void OnPackageCollision(Rigidbody2D rb, Transform packageTransform)
    {
        // Stop tracking and physics
        if (rb != null)
        {
            StartCoroutine(DelayKinematic(rb, packageTransform));
        }
    }

    private IEnumerator DelayKinematic(Rigidbody2D rb, Transform packageTransform)
    {
        // Wait for 0.5 seconds to allow settling
        yield return new WaitForSeconds(0.5f);

        // Stop movement and set kinematic
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop movement
            rb.isKinematic = true; // Disable physics updates
            packageTransform.SetParent(backpackTransform); // Parent to backpack
        }
    }

    private void PopLastPackage()
    {
        if (spawnedPackages.Count == 0)
            return;

        // Get the last package
        Rigidbody2D rb = spawnedPackages[spawnedPackages.Count - 1];
        if (rb == null)
        {
            spawnedPackages.RemoveAt(spawnedPackages.Count - 1);
            return;
        }

        // Unparent and re-enable physics
        Transform packageTransform = rb.transform;
        packageTransform.SetParent(null);
        rb.isKinematic = false;

        // Position package above backpack
        Vector3 backpackPos = backpackTransform.position;
        Vector2 throwStartPos = new Vector2(backpackPos.x, backpackPos.y + throwOffsetY);
        packageTransform.position = throwStartPos;

        // Calculate throw direction based on player facing
        float throwDirection = facingRight ? 1f : -1f;
        Vector2 throwVelocity = new Vector2(throwDirection * throwSpeed, throwHeight);
        rb.velocity = throwVelocity;

        // Remove from tracking lists
        spawnedPackages.Remove(rb);
    }

}

