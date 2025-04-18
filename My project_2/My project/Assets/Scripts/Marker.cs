using UnityEditor;
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

    // [Header("Hands & Grabbables")]
    // public Grabbable oculusGrabbable;
    // public GrabInteractable grabInteractable;

    // Something about keeping track of all of our drawings
    private LineRenderer currentDrawing_beta;
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
        grab.selectEntered.AddListener(XRGrabInteractable_Selected);
    }

    private void Update()
    {
        // else if (currentDrawing != null) {
        //     currentDrawing = null;
        // } 
        // else if (OVRInput.GetDown(OVRInput.Button.One)) {
        //     SwitchColor();
        // }
    }

    private void XRGrabInteractable_Activated(ActivateEventArgs eventArgs)
    {
        BeginDrawing();
    }

    private void XRGrabInteractable_Deactivated(DeactivateEventArgs eventArgs)
    {
        EndDrawing();
    }

    private void XRGrabInteractable_Selected(SelectEnterEventArgs eventArgs)
    {
        if(OVRInput.GetDown(OVRInput.Button.One)) {
            SwitchColor();
        }
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

        currentDrawing_beta.transform.parent = tip.transform;
        currentDrawing_beta.transform.localPosition = Vector3.zero;
        currentDrawing_beta.transform.localRotation = Quaternion.identity;
    }

    private void EndDrawing()
    {
        currentDrawing.transform.parent = null;
        currentDrawing.GetComponent<TrailRenderer>().emitting = false;
        currentDrawing = null;
    }

    private void SwitchColor() {
        if (currColorIndex == penColors.Length - 1) {
            currColorIndex = 0;
        } else {
            currColorIndex++;
        }

        tipMat.color = penColors[currColorIndex];
    }

    // private void Erase() {
    //     Debug.Log("Erasing");
    //     index = 0;
    //     currentDrawing_beta.positionCount = index + 1;
    //     while (currentDrawing_beta != null) {
    //         currentDrawing_beta = null;
    //         index = index + 1;
    //         currentDrawing_beta.positionCount = index + 1;
    //     }

    //     index = 0;
    //     currentDrawing_beta.positionCount = index + 1;
    // }

    
}
