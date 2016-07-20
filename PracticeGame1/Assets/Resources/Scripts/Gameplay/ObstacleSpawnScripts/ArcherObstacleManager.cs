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

    private bool _RandomizedCountdownTimeSet = false;

    private bool _InRangeOfPlayer = false;

    void Start()
    {
        ProjectileFireCountdownStartTime = UnityEngine.Random.Range(3.0f, 6.0f);

        _RandomizedCountdownTimeSet = true;
    }

    void Update()
    {
        UpdateObstacle();

        CountdownToProjectileFire();
    }

    public void CountdownToProjectileFire()
    {
        if (StartMovingOnTrack && !GameplayManager.Instance.GameplayPaused && _RandomizedCountdownTimeSet)
        {
            _CurrentProjectileFireCountdownTime -= Time.deltaTime;

            if (_CurrentProjectileFireCountdownTime <= 0f)
            {
                int numToDetermineIfFire = UnityEngine.Random.Range(0, 3);

                if (numToDetermineIfFire > 0)
                {
                    FireArrow();
                }

                _CurrentProjectileFireCountdownTime = ProjectileFireCountdownStartTime;
            }
        }
    }

    private void FireArrow()
    {
        GameObject projectile = null;

        projectile = Instantiate(Resources.Load(ARROW_PROJECTILE_ASSET_PATH, typeof(GameObject))) as GameObject;

        BaseProjectileManager projectileManager = null;

        if (projectile != null && _InRangeOfPlayer && transform.position.z > 0f)
        {
            projectile.transform.parent = this.transform;
            projectile.transform.localPosition = _StartingArrowPosition;

            projectileManager = projectile.GetComponent<BaseProjectileManager>();

            if (projectileManager != null)
            {
                AudioManager.Instance.PlaySFXClip(AudioManager.SFXClips.BowShotSFX);

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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ProjectileRange")
        {
            _InRangeOfPlayer = true;
        }
    }
}
