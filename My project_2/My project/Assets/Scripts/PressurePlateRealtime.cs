using UnityEngine;
using Normal.Realtime;
using UnityEngine.Events;

[RequireComponent(typeof(RealtimeView))]
public class PressurePlateRealtime : RealtimeComponent<PressurePlateModel>
{
    public LayerMask valid;
    public GameObject plate;
    public Vector3 goDown;
    public Material activatedMat, unactivatedMat;
    public UnityEvent WhenPressed, WhenReleased;

    private Vector3 inactivePos, activePos;
    private MeshRenderer rend;

    void Awake()
    {
        rend = plate.GetComponent<MeshRenderer>();
        inactivePos = plate.transform.position;
        activePos = inactivePos + goDown;
    }

    protected override void OnRealtimeModelReplaced(PressurePlateModel prev, PressurePlateModel curr)
    {
        if (prev != null) prev.pressedDidChange -= OnPressedChanged;
        if (curr != null)
        {
            curr.pressedDidChange += OnPressedChanged;
            OnPressedChanged(curr, curr.pressed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & valid.value) == 0) return;

        // 1) Request ownership so you can write model below
        if (!realtimeView.isOwnedLocallySelf)
        {
            realtimeView.RequestOwnership();
            Debug.Log("[Plate] Requested ownership");
        }

        // 2) Flip the shared flag
        if (!model.pressed)
        {
            model.pressed = true;
            Debug.Log("[Plate] model.pressed → true");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & valid.value) == 0) return;

        if (model.pressed)
        {
            model.pressed = false;
            Debug.Log("[Plate] model.pressed → false");

            // optional: give up ownership so others can press immediately
            // realtimeView.ClearOwnership();
            // Debug.Log("[Plate] Cleared ownership");
        }
    }

    private void OnPressedChanged(PressurePlateModel m, bool isPressed)
    {
        Debug.Log($"[Plate] OnPressedChanged: {isPressed}");
        rend.material = isPressed ? activatedMat : unactivatedMat;
        plate.transform.position = isPressed ? activePos : inactivePos;
        if (isPressed) WhenPressed?.Invoke();
        else WhenReleased?.Invoke();
    }
    public PressurePlateModel PlateModel
    {
        get { return model; }
    }
}
