using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manager that dictates what obstacles are spawned on certain ObstaclesSpawnPoints for each row that gets created.
/// </summary>
public class ObstacleSpawnerManager : MonoBehaviour
{
    private bool _ObstacleSpawnerManagerInitialized = false;

    private const string EMPTY_OBSTACLE_ASSET_PATH = "Prefabs/3D/Obstacles/EmptyObstacle";
    private const string PIKE_ASSET_PATH = "Prefabs/3D/Obstacles/Pike";
    private const string HILL_ASSET_PATH = "Prefabs/3D/Obstacles/Hill";
    private const string TEST_ENEMY_AI_ASSET_PATH = "Prefabs/3D/Obstacles/TestEnemyAIObstacle";
    private const string ARCHER_ENEMY_AI_ASSET_PATH = "Prefabs/3D/Obstacles/TestArcherAIObstacle";

    private const int EMPTY_OBSTACLE_INDEX_NUM = 0;

    public int MaxBuildingsPerSpawnRow = 4;
    public int MinBuildingsPerSpawnRow = 0;

    public List<Transform> RowBuildingSpawnPoints = new List<Transform>();

    public List<string> ObstacleObjectPaths = new List<string>();

    private static float BASE_ROW_SPAWN_TIME_INTERVAL = 2.0f;

    private float _TrackSpeed = 0.001f;

    private float _SpawnTimeFullInterval = 2.0f;

    private bool _SpawnIntervalNotFinished = true;
    private float _TimeRemainingInSpawnInterval = 1.0f;

	// Use this for initialization
	void Start () {
        this.transform.localPosition.Set(0f, 0f, 150f);

        if (GameplayManager.Instance != null)
        {
            _TrackSpeed = GameplayManager.Instance.GameplayTrackSpeed;
        }

        _SpawnTimeFullInterval = BASE_ROW_SPAWN_TIME_INTERVAL;

        // Add all the obstacle location paths to the ObstacleObjectPaths list before we begin spawning obstacles.
        AddObstaclesToObstaclePathList();

        // Do everything necessary for initialization prior to this being true.
        _ObstacleSpawnerManagerInitialized = true;
    }

    private void AddObstaclesToObstaclePathList()
    {
        ObstacleObjectPaths.Add(EMPTY_OBSTACLE_ASSET_PATH);
        ObstacleObjectPaths.Add(PIKE_ASSET_PATH);
        ObstacleObjectPaths.Add(HILL_ASSET_PATH);
        ObstacleObjectPaths.Add(TEST_ENEMY_AI_ASSET_PATH);
        ObstacleObjectPaths.Add(ARCHER_ENEMY_AI_ASSET_PATH);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If this script is initialized and the gameplay isn't paused, then generate obstacles!
        if (_ObstacleSpawnerManagerInitialized && !GameplayManager.Instance.GameplayPaused)
        {
            if(_SpawnIntervalNotFinished)
            {
                _TimeRemainingInSpawnInterval -= Time.deltaTime;

                if(_TimeRemainingInSpawnInterval < 0)
                {
                    _SpawnIntervalNotFinished = false;
                }
            }
            else
            {
                // TODO Spawn the buildings now in a row.
                SpawnObstacles();

                _TimeRemainingInSpawnInterval = _SpawnTimeFullInterval;

                _SpawnIntervalNotFinished = true;
            }
        }    
	}

    /// <summary>
    /// Spawn obstacles in a row.
    /// </summary>
    private void SpawnObstacles()
    {
        int guaranteedEmptyObstacleSpawnPoint = UnityEngine.Random.Range(0, RowBuildingSpawnPoints.Count);

        for (int spawnPointNum = 0; spawnPointNum < RowBuildingSpawnPoints.Count; spawnPointNum++)
        {
            int obstacleToSpawnIndex = UnityEngine.Random.Range(0, ObstacleObjectPaths.Count);

            GameObject obstacle = null;

            // If the spawnPointNum is not the same as the guaranteed empty spawn point num, then spawn an obstacle at random. Else, spawn an empty obstacle to give the player a guaranteed escape route.
            if (spawnPointNum != guaranteedEmptyObstacleSpawnPoint)
            {
                // Spawn a random obstacle at one of the spawn points while not spawning multiples at that spawn point.
                obstacle = Instantiate(Resources.Load(ObstacleObjectPaths[obstacleToSpawnIndex], typeof(GameObject))) as GameObject;
            }
            else
            {
                // Spawn a random obstacle at one of the spawn points while not spawning multiples at that spawn point.
                obstacle = Instantiate(Resources.Load(ObstacleObjectPaths[EMPTY_OBSTACLE_INDEX_NUM], typeof(GameObject))) as GameObject;
            }

            if (obstacle != null)
            {
                obstacle.transform.SetParent(RowBuildingSpawnPoints[spawnPointNum].transform);
                obstacle.transform.localPosition = Vector3.zero;

                Debug.LogFormat("Spawn a {0} at the spawn location #{1}.", ObstacleObjectPaths[obstacleToSpawnIndex], spawnPointNum);

                InitSpawnedObstacleScript(obstacle);
            }
            else
            {
                Debug.LogError("Obstacle to spawn is null!");
            }
        }
    }

    private void InitSpawnedObstacleScript(GameObject obstacle)
    {
        if (obstacle.GetComponent<GenericObstacleManager>() != null)
        {
            GenericObstacleManager genericObstacleScript = obstacle.GetComponent<GenericObstacleManager>();
            if (genericObstacleScript != null)
            {
                genericObstacleScript.Init(_TrackSpeed);
            }
            else
            {
                DestroyImmediate(obstacle);
            }

            return;
        }
        else if (obstacle.GetComponent<ArcherObstacleManager>() != null)
        {
            ArcherObstacleManager archerObstacleScript = obstacle.GetComponent<ArcherObstacleManager>();
            if (archerObstacleScript != null)
            {
                archerObstacleScript.Init(_TrackSpeed);
            }
            else
            {
                DestroyImmediate(obstacle);
            }

            return;
        }
        else
        {
            return;
        }
    }
}
