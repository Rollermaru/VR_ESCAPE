using UnityEngine;
using Normal.Realtime;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(RealtimeView))]
public class DoorHandlerRealtime : MonoBehaviour
{
    [Header("Pressure Plate Reference")]
    public PressurePlateRealtime pressurePlate;

    [Header("Door Objects")]
    public GameObject doorL;
    public GameObject doorR;

    [Header("Movement Settings")]
    public Vector3 moveAmount;
    public float animationTime = 2f;

    [Header("Events")]
    public UnityEvent WhenOpening;
    public UnityEvent WhenClosing;

    private Vector3 closedL, closedR, openL, openR;
    private bool lastPressed;

    private RealtimeTransform rtL, rtR;

    void Start()
    {
        if (pressurePlate == null || doorL == null || doorR == null)
        {
            Debug.LogError("[DoorHandlerRealtime] Missing references.");
            return;
        }

        // Cache door positions
        closedL = doorL.transform.position;
        closedR = doorR.transform.position;
        openL = closedL + moveAmount;
        openR = closedR + moveAmount;

        rtL = doorL.GetComponent<RealtimeTransform>();
        rtR = doorR.GetComponent<RealtimeTransform>();

        if (rtL == null || rtR == null)
            Debug.LogWarning("Door objects missing RealtimeTransform components!");

        lastPressed = pressurePlate.PlateModel.pressed;
        pressurePlate.PlateModel.pressedDidChange += OnPadPressedChanged;

        Debug.Log("[DoorHandlerRealtime] Initialized and subscribed to model.pressedDidChange");
    }

    private void OnPadPressedChanged(PressurePlateModel model, bool isPressed)
    {
        Debug.Log($"[DoorHandler] OnPadPressedChanged: {isPressed}");

        if (isPressed && !lastPressed)
            WhenOpening?.Invoke();
        else if (!isPressed && lastPressed)
            WhenClosing?.Invoke();

        lastPressed = isPressed;

        // Try taking ownership if we're the one who owns the plate (to move the doors)
        if (pressurePlate.isOwnedLocallyInHierarchy)
        {
            if (rtL != null && !rtL.isOwnedLocallySelf)
                rtL.RequestOwnership();

            if (rtR != null && !rtR.isOwnedLocallySelf)
                rtR.RequestOwnership();
        }

        // Run the animation on ALL clients (ownership makes movement persist)
        StopAllCoroutines();
        StartCoroutine(AnimateDoors(isPressed));
    }

    private IEnumerator AnimateDoors(bool open)
    {
        Debug.Log($"[DoorHandler] AnimateDoors: {open}");

        Vector3 startL = doorL.transform.position;
        Vector3 startR = doorR.transform.position;
        Vector3 targetL = open ? openL : closedL;
        Vector3 targetR = open ? openR : closedR;

        float elapsed = 0f;

        while (elapsed < animationTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationTime);

            doorL.transform.position = Vector3.Lerp(startL, targetL, t);
            doorR.transform.position = Vector3.Lerp(startR, targetR, t);

            yield return null;
        }

        doorL.transform.position = targetL;
        doorR.transform.position = targetR;

        Debug.Log("[DoorHandler] Finished animating doors.");
    }
}
