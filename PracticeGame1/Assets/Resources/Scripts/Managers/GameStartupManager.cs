using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// Creates instances of all the Manager classes that need to be running on startup of game.
/// </summary>
public class GameStartupManager : MonoBehaviourSingleton<GameStartupManager> 
{

    public const string MANAGER_SCRIPTS_PATH = "Prefabs/Managers/";

    // List of all Manager script names for startup initialization.
    public const string SCREEN_MANAGER = "ScreenManager";
	public const string UI_MANAGER = "UIManager";
    public const string GAMEPLAY_MANAGER = "GameplayManager";
    public const string PLAYER_PREFS_MANAGER = "PlayerPrefsManager";

    // List of manager name strings for each manager that will get instanced on startup.
    public List<string> ManagerNames = new List<string>();

	// Use this for initialization
	void Start ()
	{
        Debug.Log("StartupManager active.");

        ManagerNames.Clear();

        EstablishManagers();
    }

    void EstablishManagers()
    {
        // Go through and add all manager names to the list.
		ManagerNames.Add (SCREEN_MANAGER);
		ManagerNames.Add (UI_MANAGER);
        ManagerNames.Add (GAMEPLAY_MANAGER);
        ManagerNames.Add(PLAYER_PREFS_MANAGER);

        // Iterate through all scenes in the list and create an instance of each as a child under the GameStartupManager GameObject.
        if (ManagerNames.Count > 0)
        {
            foreach (string managerName in ManagerNames)
            {
                CreateManagerChild(managerName);
            }
        }

        Debug.Log("All startup scene managers are now instanced.");
    }

    void CreateManagerChild(string managerName)
    {
        if (!string.IsNullOrEmpty(managerName))
        {
            GameObject managerInstance = Instantiate(Resources.Load(MANAGER_SCRIPTS_PATH + managerName, typeof(GameObject))) as GameObject;

            if (managerInstance != null)
            {
                managerInstance.transform.parent = this.transform;

                Debug.LogFormat("{0} manager instanced on startup.", managerName);
            }
            else
            {
                Debug.LogErrorFormat("GameStartupManager: CreateManagerChild: {0} is null or not valid!", managerName);
            }
        }
    }
}
