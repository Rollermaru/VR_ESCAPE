using UnityEngine;
using UnityEngine.Events;

public class DoorHandler : MonoBehaviour
{
    /**
     *  TRANSLATIONAL MOVEMENT ONLY
     */

    [Tooltip("Object that activates this door. This is hard programmed. Don't change it.")]
    public GameObject Activator;
    private PressurePlate Pressed;

    [Tooltip("The two game objects we want to move")]
    public GameObject DoorL;
    public GameObject DoorR;
    
    [Tooltip("Amount object should move. Only translational.")]
    public Vector3 MoveAmount;
    private Vector3 NewPositionL;
    private Vector3 ResetPositionL;
    private Vector3 NewPositionR; // Doors will move together
    private Vector3 ResetPositionR;

    [Tooltip("Amount of seconds \"Animation\" should take place.")]
    public float AnimationTime;

    [Tooltip("Place sound effects here")]
    public UnityEvent WhenOpening;
    public UnityEvent WhenClosing;

    private bool hasBeenPressed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Pressed = Activator.GetComponent<PressurePlate>();
        ResetPositionL = DoorL.transform.position;
        ResetPositionR = DoorR.transform.position;
        NewPositionL = DoorL.transform.position + MoveAmount;
        NewPositionR = DoorR.transform.position + MoveAmount;
        hasBeenPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (DoorL == null || DoorR == null) return; // No door to move
        if (Activator == null) return;  // Nothing is set to activate this door
        
        
        if(Pressed.Pressed) {
            Debug.Log("On Open Door Audio Plays");
            WhenOpening?.Invoke();
            OpenDoors();
            hasBeenPressed = true;
        }

        else {
            if (hasBeenPressed) {
                Debug.Log("On Door Close Audio Plays");
                WhenClosing?.Invoke();
            }

            CloseDoors();
        }
    }

    // Open door "animation"
    private void OpenDoors() {
        DoorL.transform.position = Vector3.Lerp(DoorL.transform.position, NewPositionL, Time.deltaTime * AnimationTime);
        DoorR.transform.position = Vector3.Lerp(DoorR.transform.position, NewPositionR, Time.deltaTime * AnimationTime);
    }

    // Close door "animation"
    private void CloseDoors() {
        DoorL.transform.position = Vector3.Lerp(DoorL.transform.position, ResetPositionL, Time.deltaTime * AnimationTime);
        DoorR.transform.position = Vector3.Lerp(DoorR.transform.position, ResetPositionR, Time.deltaTime * AnimationTime);
    }
}
