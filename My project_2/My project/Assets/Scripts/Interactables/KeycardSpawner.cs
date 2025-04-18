using Normal.Realtime;
using UnityEngine;
using static Normal.Realtime.Realtime;

public class KeycardSpawner : MonoBehaviour
{

    public GameObject keycardPrefab;

    public Transform spawnPoint;

    void Start()
    {
        // You don't need to SetActive(false) on the prefab asset.
    }

    public void SpawnKeyCard()
    {
        // 1) Make sure your Realtime component on the scene has registered
        //    “keycardPrefab.name” in its Sync Prefabs slot (so Normcore can spawn it),
        // 2) Then call Instantiate and hold the returned clone:
        var spawnedCard = Realtime.Instantiate(
            keycardPrefab.name,
            spawnPoint.position,
            spawnPoint.rotation,
            new InstantiateOptions()
        );

        // (optional) initialize your Keycard component on the clone:
        var cardComp = spawnedCard.GetComponent<Keycard>();
        if (cardComp != null)
            cardComp.cardID = System.Guid.NewGuid().ToString();

        // no need to SetActive(true)—the clone comes in active by default.
    }
}