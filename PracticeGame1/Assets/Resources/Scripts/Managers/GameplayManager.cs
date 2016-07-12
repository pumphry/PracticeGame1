using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager> {

    private GameObject _GameplayRoot;

    // Use this for initialization
    void Start()
    {
        Debug.Log("GameplayManager is active.");

        CreateGameplayRoot();
    }

    private void CreateGameplayRoot()
    {
        // Create a UIRoot that will host the two layers of the UI (screen layer and popup layer).
        GameObject _GameplayRoot = new GameObject();
        _GameplayRoot.name = "GameplayRoot";
    }

    public void CreateGameplayInstance()
    {
        Debug.Log("Gameplay instance is being created.");

        UIManager.Instance.ToggleFrontendUI(false);
    }

}
