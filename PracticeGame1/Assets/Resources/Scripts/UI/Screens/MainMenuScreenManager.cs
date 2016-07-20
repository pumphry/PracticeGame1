using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScreenManager : MonoBehaviour {

    public List<Button> ButtonsList = new List<Button>();

    public Text BestRunTimeText;

    private string RUNTIME_TEXT_PREFIX = "Best Run Time: ";
    private string RUNTIME_TEXT_SUFFIX = " Seconds";

	// Use this for initialization
	void Start ()
    {
        // Grab the best run time and populate the BestRunTimeText with it.
        SetBestRunTimeTextField();
    }

    public void StartButtonPressed()
    {
        Debug.Log("Start button has been pressed. Begin loading gameplay!");

        // Load gameplay prefab.
        GameplayManager.Instance.CreateGameplayInstance();
    }

    public void OptionsButtonPressed()
    {
        Debug.Log("Options button has been pressed. Begin loading options popup!");

        // Load popup using utility function from UIManager script.
        UIManager.Instance.LoadAndShowUniquePopup(UIManager.UIPopups.OptionsPopup);
    }

    private void SetBestRunTimeTextField()
    {
        BestRunTimeText.text = RUNTIME_TEXT_PREFIX
            + PlayerPrefsManager.Instance.GetPlayerPrefFloatVal(PlayerPrefsManager.PlayerPrefKeyNames.BestTimeFloatVal).ToString("F2")
            + RUNTIME_TEXT_SUFFIX;
    }
}
