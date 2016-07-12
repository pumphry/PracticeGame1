using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScreenManager : MonoBehaviour {

    public List<Button> ButtonsList = new List<Button>();

	// Use this for initialization
	void Start () {
	    // TODO perform runtime opening functionality here for this screen.
	}

    public void StartButtonPressed()
    {
        Debug.Log("Start button has been pressed. Begin loading gameplay!");

        // Load gameplay prefab.
        GameplayManager.Instance.CreateGameplayInstance();
    }

    public void OptionsButtonPressed()
    {
        Debug.Log("Options button has been pressed. Begin loading options screen!");

        // Load popup using utility function from UIManager script.
        UIManager.Instance.LoadAndShowUniquePopup(UIManager.UIPopups.OptionsPopup);
    }
}
