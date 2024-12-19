using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] leavePoints;
    [SerializeField] private ShelfWrapper[] shelfLocations;
    [SerializeField] private Transform cashierPosition;

    private void Start()
    {
        shelfLocations = GetShelfLocations(); // Raf konumlarını al
        StartCoroutine(SpawnNPCWithDelay(10f));
    }

    private IEnumerator SpawnNPCWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnNPC();
    }

    private void SpawnNPC()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        Transform leavePoint = leavePoints[randomIndex];

        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);

        NPC npcScript = npc.GetComponent<NPC>();
        npcScript.shelves = shelfLocations;
        npcScript.cashierPosition = cashierPosition;
        npcScript.leavePoint = leavePoint;

        StartCoroutine(SpawnNPCWithDelay(15f));
    }

    private ShelfWrapper[] GetShelfLocations()
    {
        GameObject[] shelfWrappers = GameObject.FindGameObjectsWithTag("ShelfWrapper");
        List<ShelfWrapper> shelfTransforms = new List<ShelfWrapper>();

        foreach (GameObject wrapper in shelfWrappers)
        {
            ShelfWrapper shelfWrapper = wrapper.GetComponent<ShelfWrapper>();
            if (shelfWrapper != null)
            {
                shelfTransforms.Add(shelfWrapper);
            }
        }

        return shelfTransforms.ToArray();
    }
}
