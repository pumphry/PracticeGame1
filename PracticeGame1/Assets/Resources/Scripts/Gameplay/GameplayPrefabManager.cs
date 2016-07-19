using UnityEngine;
using System.Collections;

public class GameplayPrefabManager : MonoBehaviour
{
    public bool GameplayPrefabInitialized = false;

    public Transform GameWorldContainer;
    public Transform PlayableCharacter;
    public ObstacleSpawnerManager ObstacleSpawner;

    /// <summary>
    /// This is the Init function that is called at creation of the GameplayPrefab. Pass through whatever flexible settings need to be passed through here.
    /// </summary>
    public void Init()
    {
        // Set whatever needs to be set here.

        GrabAllPublicObjectsForGameplayPrefab();

        // Set GameplayPrefab to initialized.
        GameplayPrefabInitialized = true;
    }

    private void GrabAllPublicObjectsForGameplayPrefab()
    {
        GameWorldContainer = this.transform.FindChild("GameWorldContainer");
        if (GameWorldContainer != null)
        {
            PlayableCharacter = GameWorldContainer.FindChild("ThirdPersonController");

            Transform obstacleSpawnerTransform = GameWorldContainer.FindChild("ObstaclesSpawner");

            if (obstacleSpawnerTransform != null)
            {
                ObstacleSpawner = obstacleSpawnerTransform.GetComponent<ObstacleSpawnerManager>();
            }
        }
    }
}
