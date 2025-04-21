using UnityEngine;

public class ExitDoorLogic : MonoBehaviour
{
    [SerializeField] private GameObject WinManager;
    [SerializeField] private GameObject MenacingBall;
    [SerializeField] private Transform MenacingBallSpawnPosition;
    private bool hasWon = false;
    private bool alreadyDone = false;

    [SerializeField] private GameObject ToDelete;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        hasWon = WinManager.GetComponent<DoWeWin>().win;

        if (hasWon && !alreadyDone) {
            Destroy(ToDelete);
            Instantiate(MenacingBall, MenacingBallSpawnPosition);

            alreadyDone = true;
        }
    }
}
