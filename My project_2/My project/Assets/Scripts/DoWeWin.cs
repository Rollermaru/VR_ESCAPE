using TMPro;
using UnityEngine;

public class DoWeWin : MonoBehaviour
{

    [SerializeField] private GameObject inputField;
    [SerializeField] private string code;
    private string text;

    public bool win = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = inputField.GetComponent<TMP_InputField>().text;
    }

    // Update is called once per frame
    void Update()
    {
        text = inputField.GetComponent<TMP_InputField>().text;
        CheckWin();

        if (win) Debug.Log("YOU WIN");
    }

    // If text = code, then activate win sequence
    void CheckWin() {
        if (win) return;

        if (text.Equals(code)) {
            win = true;
            return;
        }
    }
}
