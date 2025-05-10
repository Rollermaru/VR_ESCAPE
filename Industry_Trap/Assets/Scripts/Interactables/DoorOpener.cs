using Normal.Realtime;
using UnityEngine;

public class DoorOpener : RealtimeComponent<DoorOpenerModel>
{
    [SerializeField] private DoorHandle _handle;
    private RealtimeView _realtimeView;

    private void Awake()
    {
        // Cache the RealtimeView on this same GameObject
        _realtimeView = GetComponent<RealtimeView>();
    }

    protected override void OnRealtimeModelReplaced(DoorOpenerModel previousModel, DoorOpenerModel currentModel)
    {
        if (previousModel != null) previousModel.openDoorDidChange -= DidOpenDoor;
        if (currentModel != null) currentModel.openDoorDidChange += DidOpenDoor;
    }

    private void DidOpenDoor(DoorOpenerModel m, bool value)
    {
        if (value) _handle.OpenDoorAutomatically();
    }

    public void RequestOpen()
    {
        // 1) If we aren’t already the owner, take ownership
        if (_realtimeView != null && !_realtimeView.isOwnedLocallySelf)
            _realtimeView.RequestOwnership();

        // 2) Now that we own it, set the model to true and it will sync
        model.openDoor = true;
    }
}