using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    [Header("Container Settings")]
    // List the required item IDs (for example, "red", "blue", "green")
    public string[] requiredItemIDs;

    [Header("Door Reference")]
    // Reference to your door (or door handle) script that opens the door
    public DoorHandle doorHandle;

    // Internal list to keep track of which items have been inserted
    private List<string> insertedItems = new List<string>();

    // When an item enters the container's trigger, try to process it.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the incoming object has an Item component.
        Item item = other.GetComponent<Item>();
        if (item == null)
            return; // Not a valid item, ignore.

        // Verify that this item is one of the required ones.
        if (System.Array.IndexOf(requiredItemIDs, item.itemID) < 0)
        {
            Debug.Log("Item " + item.itemID + " is not accepted in this container.");
            return;
        }

        // If the item type is already in the container, ignore it.
        if (insertedItems.Contains(item.itemID))
        {
            Debug.Log("Item " + item.itemID + " has already been placed in the container.");
            return;
        }

        // "Accept" the item:
        // Snap the item to the container's position (or a specific attach point if desired).
        other.transform.position = transform.position;
        other.transform.SetParent(transform);
        // Disable physics to keep it in place.
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Record that this item has been inserted.
        insertedItems.Add(item.itemID);
        Debug.Log("Inserted item: " + item.itemID);

        // Optionally, you can add a visual effect or animation here.

        // Check if all required items have been inserted.
        if (AllItemsInserted())
        {
            OpenDoor();
        }
    }

    // Helper method to check if every required item is present.
    private bool AllItemsInserted()
    {
        foreach (string requiredID in requiredItemIDs)
        {
            if (!insertedItems.Contains(requiredID))
                return false;
        }
        return true;
    }

    // Method to trigger door opening.
    private void OpenDoor()
    {
        if (doorHandle != null)
        {
            doorHandle.OpenDoorAutomatically(); // Example door opening method.
            Debug.Log("All required items have been placed in the container. Door is now open!");
        }
        else
        {
            Debug.LogWarning("Door handle reference is missing!");
        }
    }
}
