using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public float spawnInterval;

    List<Transform> spawnPositions = new List<Transform>();
    GameObject currentItem;
    WaypointIndicator waypointIndicator;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndicator = FindObjectOfType<WaypointIndicator>();
        foreach(Transform child in transform)
            spawnPositions.Add(child);

        SpawnItem();
    }

    void SpawnItem()
    {
        // Pick a random position from the list
        int randomIndex = Random.Range(0, spawnPositions.Count);
        Transform spawnPoint = spawnPositions[randomIndex];

        // Spawn the item
        currentItem = Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
        currentItem.GetComponent<Item>().OnItemTaken += HandleItemTaken; // Subscribe to the item taken event

        waypointIndicator.SetTargetExist(true);
        waypointIndicator.SetTargetPosition(spawnPoint.position);
    }

    void HandleItemTaken()
    {
        // Unsubscribe to prevent memory leaks
        currentItem.GetComponent<Item>().OnItemTaken -= HandleItemTaken;

        // Destroy the current item
        Destroy(currentItem);
        waypointIndicator.SetTargetExist(false);

        // Start the respawn timer
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnItem();
    }
}
