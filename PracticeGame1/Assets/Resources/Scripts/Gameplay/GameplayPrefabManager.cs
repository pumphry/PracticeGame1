using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameplayPrefabManager : MonoBehaviour
{
    public bool GameplayPrefabInitialized = false;

    public Transform GameWorldContainer;
    public Transform PlayableCharacter;
    public InfiniteRunnerPlayerController PlayerController;
    public ObstacleSpawnerManager ObstacleSpawner;
    public Transform PlayerMovementZonesContainer;
    public List<Transform> ListOfPlayerMovementZones = new List<Transform>();

    /// <summary>
    /// This is the Init function that is called at creation of the GameplayPrefab. Pass through whatever flexible settings need to be passed through here.
    /// </summary>
    public void Init()
    {
        // Set whatever needs to be set here.

        GrabAllPublicObjectsForGameplayPrefab();

        SendMovementZonesListToPlayerController();

        // Set GameplayPrefab to initialized.
        GameplayPrefabInitialized = true;
    }

    private void GrabAllPublicObjectsForGameplayPrefab()
    {
        GameWorldContainer = this.transform.FindChild("GameWorldContainer");
        if (GameWorldContainer != null)
        {
            PlayableCharacter = GameWorldContainer.FindChild("ThirdPersonController");

            if(PlayableCharacter != null)
            {
                PlayerController = PlayableCharacter.GetComponent<InfiniteRunnerPlayerController>();
            }

            Transform obstacleSpawnerTransform = GameWorldContainer.FindChild("ObstaclesSpawner");

            if (obstacleSpawnerTransform != null)
            {
                ObstacleSpawner = obstacleSpawnerTransform.GetComponent<ObstacleSpawnerManager>();
            }

            PlayerMovementZonesContainer = GameWorldContainer.FindChild("PlayerMovementZonesContainer");

            if (PlayerMovementZonesContainer != null)
            {
                int count = 0;

                foreach (Transform zone in PlayerMovementZonesContainer.gameObject.GetComponentsInChildren<Transform>())
                {
                    if (count != 0)
                    {
                        ListOfPlayerMovementZones.Add(zone);
                    }

                    count++;
                }
            }
        }
    }

    public void SendMovementZonesListToPlayerController()
    {
        if(PlayerController != null)
        {
            PlayerController.PopulatePlayerMovementZonesList(ListOfPlayerMovementZones);
        }
    }
}
