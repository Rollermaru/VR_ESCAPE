using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Tooltip("LayerMask valid should contain all objects that may interact with this pressure plate")]
    public LayerMask valid;

    [Tooltip("Material to state that this pressure plate has been activated (I dont want to work with animation yet)")]
    public Material ActivatedMat;
    public Material UnactivatedMat;

    [Tooltip("The GameObject corresponding to the plate. For Rendering. So put the visual here.")]
    public GameObject plate;

    [Tooltip("How much pressure plate moves on activation... Please only change Y")]
    public Vector3 goDown;
    private Vector3 ActivePosition;    // The actual world position of pressure plate when activated
    private Vector3 InactivePosition;

    public bool Pressed;
    private MeshRenderer rend;

    public UnityEvent WhenPressed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = plate.GetComponent<MeshRenderer>();
        InactivePosition = plate.transform.position;
        ActivePosition = plate.transform.position + goDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (rend == null) {
            return;
        }
        
        if(Pressed) {
            Debug.Log("On Press Audio Plays");
            WhenPressed?.Invoke();
            OnPress();
        }

        else {
            OnExitPress();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (valid.Equals(other.gameObject.layer)) return;   // do not interact if not layer 7
        Pressed = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (valid.Equals(other.gameObject.layer)) return;   // do not interact if not layer 7
        Pressed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (valid.Equals(other.gameObject.layer)) return;   // do not interact if not layer 7
        Pressed = false;
    }

    // Changes the visuals on Pressure Plate on Activation.
    private void OnPress() {
        rend.material = ActivatedMat;
        plate.transform.position = ActivePosition;
    }

    // Changes visuals on Pressure Plate on deactivation
    private void OnExitPress() {
        rend.material = UnactivatedMat;
        plate.transform.position = InactivePosition;

    }

}
