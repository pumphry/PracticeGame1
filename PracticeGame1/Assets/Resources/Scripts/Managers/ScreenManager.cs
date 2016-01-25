using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages scene transitions and states.
/// </summary>
public class ScreenManager : MonoBehaviourSingleton<ScreenManager> 
{

	public const string SCENES_PATH = "Assets/Resources/Scenes/";

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("SceneManager is active.");

		UIManager.OnUIFrameworkFinishedBuilding += OpenMainMenuScreen;
	}

	void OpenMainMenuScreen()
	{
		// Call the UIManager function to open the main menu.
		Debug.Log("OpenMainMenuScreen call is working.");

		if (UIManager.Instance != null) {
			UIManager.Instance.TransitionToStartupScreen ();
		}

	}
}
