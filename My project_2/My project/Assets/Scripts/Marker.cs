using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Marker : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip;
    public Material drawingMat;
    public Material tipMat;
    public float penWidth;
    public Color[] penColors;

    [Header("Hands & Grabbables")]
    // Add grabbers and grabbable

    // Something about keeping track of all of our drawings
    private LineRenderer currentDrawing_1;
    private GameObject currentDrawing;
    private int index;  // count amount of drawings we have?
    private int currColorIndex;


    private void Start()
    {
        currColorIndex = 0;
        tipMat.color = penColors[currColorIndex];

        var grab = GetComponent<XRGrabInteractable>();
        grab.activated.AddListener(XRGrabInteractable_Activated);
        grab.deactivated.AddListener(XRGrabInteractable_Deactivated);
    }

    private void Update()
    {

        // bool isRightHandDraw = grabInteractable.IsSelectedByRight() && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        // bool isLeftHandDraw = grabInteractable.IsSelectedByLeft() && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        // bool isRightHandDraw = isGrabbed && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        // bool isLeftHandDraw = isGrabbed && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        // bool isRightHandDraw = isGrabbed;
        // bool isLeftHandDraw = isGrabbed;
        // bool isRightHandDraw = doBeSelected;
        // bool isLeftHandDraw = doBeSelected;

        // if (isRightHandDraw || isLeftHandDraw) {
        //     Draw();
        // }
        // else if (currentDrawing != null) {
        //     currentDrawing = null;
        // } 
        // else if (OVRInput.GetDown(OVRInput.Button.One)) {
        //     SwitchColor();
        // }

        // else if (OVRInput.GetDown(OVRInput.Button.Two)) {
        //     Erase();
        // }
    }

    private void XRGrabInteractable_Activated(ActivateEventArgs eventArgs) {
        BeginDrawing();
    }

    private void XRGrabInteractable_Deactivated(DeactivateEventArgs eventArgs) {
        EndDrawing();
    }

    public void Draw() {
        // if there is no drawing, start a new One
        if (currentDrawing_1 == null) {
            index = 0;
            currentDrawing_1 = new GameObject().AddComponent<LineRenderer>();
            currentDrawing_1.material = drawingMat;
            currentDrawing_1.startColor = currentDrawing_1.endColor = penColors[currColorIndex];
            currentDrawing_1.startWidth = currentDrawing_1.endWidth = penWidth;
            currentDrawing_1.positionCount = index + 1; // or, just 1
            currentDrawing_1.SetPosition(0, tip.transform.position);
        }

        // If there is a drawing, continue drawing
        else {
            var currentPosition = currentDrawing_1.GetPosition(index);
            if (Vector3.Distance(currentPosition, tip.transform.position) > 0.01f) {
                index++;
                currentDrawing_1.positionCount = index + 1;
                currentDrawing_1.SetPosition(index, tip.transform.position);
            }
        }
    }

    private void SwitchColor() {
        if (currColorIndex == penColors.Length - 1) {
            currColorIndex = 0;
        } else {
            currColorIndex++;
        }

        tipMat.color = penColors[currColorIndex];
    }

    private void Erase() {
        Debug.Log("Erasing");
        index = 0;
        currentDrawing_1.positionCount = index + 1;
        while (currentDrawing_1 != null) {
            currentDrawing_1 = null;
            index = index + 1;
            currentDrawing_1.positionCount = index + 1;
        }

        index = 0;
        currentDrawing_1.positionCount = index + 1;
    }

    private void BeginDrawing()
    {
        currentDrawing = new GameObject("Drawing");
        var trail = currentDrawing.AddComponent<TrailRenderer>();
        trail.time = Mathf.Infinity;
        trail.material = drawingMat;
        trail.startWidth = .05f;
        trail.endWidth = .05f;
        trail.minVertexDistance = .02f;

        currentDrawing.transform.parent = tip.transform;
        currentDrawing.transform.localPosition = Vector3.zero;
        currentDrawing.transform.localRotation = Quaternion.identity;
    }

    private void EndDrawing()
    {
        currentDrawing.transform.parent = null;
        currentDrawing.GetComponent<TrailRenderer>().emitting = false;
        currentDrawing = null;
    }
}
