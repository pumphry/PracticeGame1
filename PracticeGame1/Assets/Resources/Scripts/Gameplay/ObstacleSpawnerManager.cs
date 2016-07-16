using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawnerManager : MonoBehaviour
{
    private const string PIKE_ASSET_PATH = "Prefabs/3D/Obstacles/Pike";

    public int MaxBuildingsPerSpawnRow = 4;
    public int MinBuildingsPerSpawnRow = 0;

    public List<Transform> RowBuildingSpawnPoints = new List<Transform>();

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
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameplayManager.Instance.GameplayPaused)
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
                SpawnBuildings();

                _TimeRemainingInSpawnInterval = _SpawnTimeFullInterval;

                _SpawnIntervalNotFinished = true;
            }
        }    
	}

    /// <summary>
    /// 
    /// </summary>
    private void SpawnBuildings()
    {
        // TODO Replace this with actualy range min/max values I have above when they work.
        int numOfBuildingsToSpawn = UnityEngine.Random.Range(0, 4);

        int spawnLocation = UnityEngine.Random.Range(1, 4);

        for(int i = 0; i < numOfBuildingsToSpawn; i++)
        {
            /*if(spawnLocation >= 3)
            {
                spawnLocation = 0;
            }
            else
            {
                spawnLocation++;
            }*/

            // Spawn a building at one of the spawn points while not spawning multiples at that spawn point.
            GameObject building = Instantiate(Resources.Load(PIKE_ASSET_PATH, typeof(GameObject))) as GameObject;

            building.transform.SetParent(RowBuildingSpawnPoints[i].transform);
            building.transform.localPosition = Vector3.zero;
            //building.transform.rotation = Quaternion.Euler(Vector3.zero);

            Debug.LogFormat("Spawn a building at the {0} spawn row.", i);

            BuildingManager buildingScript = building.GetComponent<BuildingManager>();
            if(buildingScript != null)
            {
                buildingScript.Init(_TrackSpeed);
            }
            else
            {
                DestroyImmediate(building);
            }
        }
    }
}
