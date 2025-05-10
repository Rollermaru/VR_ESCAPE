using System;
using TMPro;
using UnityEngine;

public class NumberPad : MonoBehaviour
{
    [Header("Code Sequence")]
    [Tooltip("The correct numeric sequence to unlock the door.")]
    public string sequence;

    [Header("Door Reference")]
    [Tooltip("Drag the DoorHandle component you wish to open when the code is correct.")]
    public DoorHandle doorHandle;

    [Header("UI Display")]
    [Tooltip("TextMeshProUGUI to show input feedback.")]
    public TextMeshProUGUI inputDisplayText;

    private string m_CurrentEnteredCode = string.Empty;

    private void Awake()
    {
        // Initialize display
        if (inputDisplayText != null)
            inputDisplayText.text = "Code Input:\n";
    }

    /// <summary>
    /// Called by each number button, passing its integer value (e.g. via UnityEvent).
    /// </summary>
    public void ButtonPressed(int valuePressed)
    {
        // Prevent extra input once full length reached
        if (m_CurrentEnteredCode.Length >= sequence.Length)
            return;

        // Append digit
        m_CurrentEnteredCode += valuePressed;

        // Update masked display
        if (inputDisplayText != null)
        {
            if (m_CurrentEnteredCode.Length == 1)
            {
                inputDisplayText.text = "*";
                inputDisplayText.color = Color.black;
            }
            else
            {
                inputDisplayText.text += "*";
            }
        }

        // Check once we've entered enough digits
        if (m_CurrentEnteredCode.Length == sequence.Length)
        {
            bool correct = m_CurrentEnteredCode == sequence;
            if (inputDisplayText != null)
            {
                inputDisplayText.color = correct ? Color.green : Color.red;
                inputDisplayText.text = correct ? "Code Valid!" : "Invalid Code!";
            }

            if (correct)
            {
                // Open the door instead of spawning a keycard
                if (doorHandle != null)
                    doorHandle.OpenDoorAutomatically();
                else
                    Debug.LogWarning("NumberPad: DoorHandle reference not set.");
            }
            else
            {
                Debug.Log("NumberPad: Wrong code entered.");
            }

            // Reset for next attempt (with display clear)
            ResetSequence(true);
        }
    }

    /// <summary>
    /// Clears the entered code and optionally resets the UI.
    /// </summary>
    public void ResetSequence(bool clearText)
    {
        m_CurrentEnteredCode = string.Empty;
        if (clearText && inputDisplayText != null)
        {
            inputDisplayText.text = "Code Input:\n";
            inputDisplayText.color = Color.black;
        }
    }
}
