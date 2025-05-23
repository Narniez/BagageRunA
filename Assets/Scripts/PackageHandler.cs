using UnityEngine;

public class PackageHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] packagePrefabs;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Transform backpackTransform;
    [SerializeField] private Transform spawnTransform;

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
    }

    private void SpawnPackage()
    {
        GameObject package = packagePrefabs[Random.Range(0, packagePrefabs.Length)];

        Vector2 spawnPosition = spawnTransform.transform.position;
        Instantiate(package, spawnPosition, Quaternion.identity);
    }

}

