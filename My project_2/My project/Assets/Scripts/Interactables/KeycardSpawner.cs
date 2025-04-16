using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using static Normal.Realtime.Realtime; // Make sure to include Normcore's namespace
public class KeycardSpawner : MonoBehaviour
{
    public GameObject keycardPrefab;

    private void Start()
    {
        // Ensure the keycard is hidden when the scene starts
        keycardPrefab.SetActive(false);
    }


    public void SpawnKeyCard()
    {
        // You need to use the prefab's name as it’s registered in Normcore’s Sync Prefabs list.
        string prefabName = keycardPrefab.name;

        // Create or use default instantiation options. You can create a new instance of InstantiateOptions,
        // or if you want the default behavior, you can pass a new empty options object.
        InstantiateOptions options = new InstantiateOptions();
        // Optionally you can set properties on options here, for example:
        // options.destroyWhenOwnerLeaves = true;
        // options.useInstance = true; // if you have multiple Normcore instances in your scene

        // Now call the new overload of Realtime.Instantiate.
        keycardPrefab.SetActive(true);
        Realtime.Instantiate(prefabName, transform.position, transform.rotation, options);
    }
}