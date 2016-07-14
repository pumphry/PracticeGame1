using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameOverPopup : MonoBehaviour
{
    public Text MessageText;

    /// <summary>
    /// Init with a message to tell the user how they lost.
    /// </summary>
    /// <param name="message"></param>
    public void Init(string message)
    {
        if (message != null)
        {
            MessageText.text = message;
        }
    }

    public void RestartButtonPressed()
    {
        // TODO create restart functionality. For now will just return to Frontend.
        GameplayManager.Instance.ExitGameAndReturnToFrontend();

        UIManager.Instance.RemoveLastPopupListed();
    }

    public void ExitToFrontEndButtonPressed()
    {
        GameplayManager.Instance.ExitGameAndReturnToFrontend();
        UIManager.Instance.RemoveLastPopupListed();
    }
}
