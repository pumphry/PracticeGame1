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

		UIManager.OnUIFrameworkFinishedBuilding += OpenFrontEndUIComponents;
	}

	void OpenFrontEndUIComponents()
	{
		// Call the UIManager to open the startup FrontEnd UI elements.
		if (UIManager.Instance != null) {
			UIManager.Instance.LoadStartupScreen ();
			UIManager.Instance.LoadFrontEndHudOverlay ();
		} 
		else 
		{
			Debug.LogError ("UIManager is null!");
		}
	}
}
