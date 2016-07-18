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

    private const string PIKE_ASSET_PATH = "Prefabs/3D/Obstacles/Pike";
    private const string HILL_ASSET_PATH = "Prefabs/3D/Obstacles/Hill";

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
        ObstacleObjectPaths.Add(PIKE_ASSET_PATH);
        ObstacleObjectPaths.Add(HILL_ASSET_PATH);

        // Do everything necessary for initialization prior to this being true.
        _ObstacleSpawnerManagerInitialized = true;
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
    /// Spawn obstacles.
    /// </summary>
    private void SpawnObstacles()
    {
        // TODO Replace this with actualy range min/max values I have above when they work.
        int numOfBuildingsToSpawn = UnityEngine.Random.Range(0, 4);

        for(int i = 0; i < numOfBuildingsToSpawn; i++)
        {
            int obstacleToSpawnIndex = UnityEngine.Random.Range(0, ObstacleObjectPaths.Count);

            // Spawn a random obstacle at one of the spawn points while not spawning multiples at that spawn point.
            GameObject obstacle = Instantiate(Resources.Load(ObstacleObjectPaths[obstacleToSpawnIndex], typeof(GameObject))) as GameObject;

            obstacle.transform.SetParent(RowBuildingSpawnPoints[i].transform);
            obstacle.transform.localPosition = Vector3.zero;

            Debug.LogFormat("Spawn a {0} at the spawn location #{1}.", ObstacleObjectPaths[obstacleToSpawnIndex], i);

            InitSpawnedObstacleScript(obstacle);
        }
    }

    private void InitSpawnedObstacleScript(GameObject obstacle)
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
    }
}
