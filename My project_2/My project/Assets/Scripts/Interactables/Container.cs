using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collects specified item pieces and, when complete, spawns the assembled computer prefab.
/// Icons for each required item change color upon detection.
/// </summary>
public class ItemContainer : MonoBehaviour
{
    [Header("Container Settings")]
    [Tooltip("List the required item IDs (e.g., \"CPU\", \"GPU\", \"RAM\").")]
    public string[] requiredItemIDs;

    [Header("Icon References")]
    [Tooltip("Icon GameObjects corresponding to each required item, in the same order as requiredItemIDs.")]
    public GameObject[] itemIcons;

    [Header("Assembly Settings")]
    [Tooltip("Prefab of the fully assembled computer to spawn when all pieces are inserted.")]
    public GameObject assembledComputerPrefab;

    [Tooltip("Optional transform at which to spawn the assembled computer. Uses container's transform if null.")]
    public Transform spawnPoint;

    // Internal tracking of inserted piece IDs
    private HashSet<string> insertedItems = new HashSet<string>();
    private bool hasAssembled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasAssembled)
            return;

        // Check for matching Item component
        Item item = other.GetComponent<Item>();
        if (item == null)
            return;

        string id = item.itemID;
        int index = System.Array.IndexOf(requiredItemIDs, id);
        if (index < 0 || insertedItems.Contains(id))
            return;

        // Mark as inserted
        insertedItems.Add(id);
        Debug.Log($"Item detected: {id}");

        // Change icon color to indicate presence
        if (itemIcons != null && index < itemIcons.Length)
        {
            var renderer = itemIcons[index].GetComponent<Renderer>();
            if (renderer != null)
            {
                // Example: set to green to show collected
                renderer.material.color = Color.green;
            }
            else
            {
                var img = itemIcons[index].GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                    img.color = Color.green;
            }
        }

        // If all required pieces are present, assemble
        if (insertedItems.Count == requiredItemIDs.Length)
            AssembleComputer();
    }

    private void AssembleComputer()
    {
        if (assembledComputerPrefab == null)
        {
            Debug.LogWarning($"AssembledComputerPrefab not set on {name}.");
            return;
        }

        Vector3 pos = (spawnPoint != null ? spawnPoint.position : transform.position);
        Quaternion rot = (spawnPoint != null ? spawnPoint.rotation : transform.rotation);
        Instantiate(assembledComputerPrefab, pos, rot);
        Debug.Log("Assembled computer spawned.");

        hasAssembled = true;
    }
}
