using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class DoorOpenerModel
{
    //   propertyID   reliable?   generateDidChangeEvent?
    [RealtimeProperty(1, true, true)]
    private bool _openDoor;
}
