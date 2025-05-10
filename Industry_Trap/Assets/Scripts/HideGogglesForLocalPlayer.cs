using UnityEngine;
using Normal.Realtime;

public class HideGogglesForLocalPlayer : MonoBehaviour
{
    private RealtimeAvatar _realtimeAvatar;

    void Start()
    {
        _realtimeAvatar = GetComponentInParent<RealtimeAvatar>();

        if (_realtimeAvatar != null && _realtimeAvatar.realtime != null)
        {
            int localClientID = _realtimeAvatar.realtime.clientID;
            if (_realtimeAvatar.ownerIDInHierarchy == localClientID)
            {
                // Hide goggles only for the local player
                gameObject.SetActive(false);
            }
        }
    }
}

