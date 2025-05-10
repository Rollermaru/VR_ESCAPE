using Normal.Realtime;

[RealtimeModel]
public partial class DoorStateModel
{
    // property #1, initial value = false, reliable = true
    [RealtimeProperty(1, false, true)]
    private bool _isOpen;
}