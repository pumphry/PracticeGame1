using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{

    private string GAMEPLAY_PREFAB_PATH = "Prefabs/3D/GameplayPrefab";

    // Use this for initialization
    void Start()
    {
        Debug.Log("GameplayManager is active.");
    }

    public void CreateGameplayInstance()
    {
        Debug.Log("Gameplay instance is being created.");

        UIManager.Instance.ToggleFrontendUI(false);

        CreateGameplayPrefab();
    }

    private void CreateGameplayPrefab()
    {
        GameObject gameplayPrefab = Instantiate(Resources.Load(GAMEPLAY_PREFAB_PATH, typeof(GameObject))) as GameObject;

        if (gameplayPrefab != null)
        {
            Debug.LogFormat("Gameplay prefab instanced.");

            gameplayPrefab.transform.position = Vector3.zero;
        }
        else
        {
            Debug.LogErrorFormat("Gameplay prefab failed to instance!");
        }
    }

}
