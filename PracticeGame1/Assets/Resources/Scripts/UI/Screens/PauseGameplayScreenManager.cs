using UnityEngine;
using System.Collections;

public class PauseGameplayScreenManager : ScreenBase
{

	// Use this for initialization
	void Start () {
	
	}
	
    public void ResumeGameButtonPressed()
    {
        Debug.Log("Resume button has been pressed. Toggling game pause to off to resume gameplay!");

        GameplayManager.Instance.TogglePauseGameplay();
    }

    public void OptionsButtonPressed()
    {
        Debug.Log("Options button has been pressed. Begin loading options popup!");

        // Load options popup using utility function from UIManager script.
        UIManager.Instance.LoadAndShowUniquePopup(UIManager.UIPopups.OptionsPopup);
    }

    public void ExitButtonPressed()
    {
        Debug.Log("Exit button has been pressed. Destroy gameplay instance after saving and then return player to front end.");

        GameplayManager.Instance.ExitGameAndReturnToFrontend();
    }
}
