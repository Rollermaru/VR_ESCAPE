using UnityEngine;
using Normal.Realtime;

[RequireComponent(typeof(RealtimeView))]
public class DoorController : RealtimeComponent<DoorStateModel>
{
    [Header("Hookup")]
    public DoorHandle doorHandle;   // assign in inspector

    // locally track if we've already animated the door open
    private bool hasOpened = false;

    /// <summary>
    /// Called by Normcore whenever the underlying model instance changes (including at startup).
    /// </summary>
    protected override void OnRealtimeModelReplaced(DoorStateModel previousModel, DoorStateModel currentModel)
    {
        // Unsubscribe from old model
        if (previousModel != null)
            previousModel.isOpenDidChange -= IsOpenDidChange;

        // Subscribe to new model
        if (currentModel != null)
        {
            currentModel.isOpenDidChange += IsOpenDidChange;

            // If the door was already marked open before we joined, open locally now
            if (currentModel.isOpen)
                OpenLocally();
        }
    }

    // Matches Normcore's PropertyChangedHandler<bool> signature
    private void IsOpenDidChange(RealtimeModel model, bool isOpen)
    {
        if (isOpen && !hasOpened)
            OpenLocally();
    }

    private void OpenLocally()
    {
        var realtimeTransform = doorHandle.draggedTransform.GetComponent<RealtimeTransform>();
        if (realtimeTransform != null && !realtimeTransform.isOwnedLocallySelf)
        {
            realtimeTransform.RequestOwnership();
        }
        // Animate your door once
        doorHandle.OpenDoorAutomatically();
        hasOpened = true;
    }

    //
    public void OpenDoorForEveryone()
    {
        if (!model.isOpen)
            model.isOpen = true;
    }
}
