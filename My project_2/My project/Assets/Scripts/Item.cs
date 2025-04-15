using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Item : XRGrabInteractable
{
    // Set this in the Inspector for each item (e.g., "red", "blue", "green")
    public string itemID;
}