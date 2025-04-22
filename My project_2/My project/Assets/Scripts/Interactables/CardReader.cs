using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Normal.Realtime;
using UnityEngine.XR.Interaction.Toolkit.Interactors;  // Normcore namespace

public class CardReader : XRSocketInteractor
{
    [Header("CardReader ReaderOptions Data")]
    public float allowedUprightErrorRange = 0.99999f;

    [Header("Accepted Keycard IDs")]
    [Tooltip("Only these card.cardID values will unlock the door on a valid swipe.")]
    public string[] acceptedCardIDs;

    [Header("Success References")]
    public GameObject visualLockToHide;
    public DoorController doorController;  // Networked door controller

    private Vector3 m_HoverEntry;
    private bool m_SwipIsValid;
    private Transform m_KeycardTransform;

    public override bool CanSelect(IXRSelectInteractable interactable) => false;

    public override bool CanHover(IXRHoverInteractable interactable) => interactable is Keycard;

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        m_KeycardTransform = args.interactableObject.transform;
        m_HoverEntry = m_KeycardTransform.position;
        m_SwipIsValid = true;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        // First, get the Keycard component and check its ID
        var keycardGO = args.interactableObject.transform.gameObject;
        var keycard = keycardGO.GetComponent<Keycard>();
        if (keycard == null || !acceptedCardIDs.Contains(keycard.cardID))
        {
            Debug.Log($"CardReader: Rejected card ID '{keycard?.cardID ?? "null"}'");
            m_KeycardTransform = null;
            return;
        }

        // Next, check swipe validity
        Vector3 entryToExit = keycardGO.transform.position - m_HoverEntry;
        Debug.Log($"Swipe exit delta: {entryToExit} (y delta: {entryToExit.y})");

        if (m_SwipIsValid && entryToExit.y < -0.15f)
        {
            Debug.Log("Swipe valid AND card ID accepted! Unlocking door for everyone.");
            visualLockToHide.SetActive(false);

            if (doorController != null)
                doorController.OpenDoorForEveryone();
            else
                Debug.LogWarning("CardReader: DoorController reference not set.");
        }
        else
        {
            Debug.Log($"Swipe invalid or too shallow. Valid? {m_SwipIsValid}, yDelta: {entryToExit.y}");
        }

        m_KeycardTransform = null;
    }

    private void Update()
    {
        if (m_KeycardTransform != null)
        {
            float dot = Vector3.Dot(m_KeycardTransform.forward, Vector3.up);
            if (dot < 1 - allowedUprightErrorRange)
                m_SwipIsValid = false;
        }
    }
}
