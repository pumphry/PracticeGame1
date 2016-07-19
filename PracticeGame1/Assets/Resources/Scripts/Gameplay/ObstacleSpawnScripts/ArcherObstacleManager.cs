using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A custom Archer Obstacle manager script that inherits from the base class GenericObstacleManager. Archer's shoot arrows!
/// </summary>
public class ArcherObstacleManager : GenericObstacleManager
{
    public float ProjectileFireCountdownStartTime = 3.0f;

    private float _CurrentProjectileFireCountdownTime = 3.0f;

    public const string ARROW_PROJECTILE_ASSET_PATH = "Prefabs/3D/Projectiles/TestProjectile";

    private Vector3 _StartingArrowPosition = new Vector3(0f, 1f, 0f);

    void Update()
    {
        UpdateObstacle();

        CountdownToProjectileFire();
    }
    
    public void CountdownToProjectileFire()
    {
        if(StartMovingOnTrack && !GameplayManager.Instance.GameplayPaused)
        {
            _CurrentProjectileFireCountdownTime -= Time.deltaTime;

            if(_CurrentProjectileFireCountdownTime <= 0f)
            {
                _CurrentProjectileFireCountdownTime = ProjectileFireCountdownStartTime;

                FireArrow();
            }
        }
    }

    private void FireArrow()
    {
        GameObject projectile = null;

        projectile = Instantiate(Resources.Load(ARROW_PROJECTILE_ASSET_PATH, typeof(GameObject))) as GameObject;

        BaseProjectileManager projectileManager = null;

        if(projectile != null)
        {
            projectile.transform.parent = this.transform;
            projectile.transform.localPosition = _StartingArrowPosition;

            projectileManager = projectile.GetComponent<BaseProjectileManager>();

            if(projectileManager != null)
            {
                projectileManager.Init(0.001f);

                Debug.Log("Arrow projectile fired!");
            }
            else
            {
                DestroyImmediate(projectile.gameObject);
            }
        }
        else
        {
            DestroyImmediate(projectile.gameObject);
        }
    }
}
