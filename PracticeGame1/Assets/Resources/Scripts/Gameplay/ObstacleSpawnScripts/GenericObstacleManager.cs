using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GenericObstacleManager : MonoBehaviour
{
    public bool StartMovingOnTrack = false;

    public float TrackSpeed = 0.001f;

    private Vector3 _PositionToLerpTo = Vector3.zero;

    private const float DESTROY_Z_DISTANCE_THRESHOLD = -100f;

    private Vector3 _SetRotationAngle;
    private Vector3 _SetSpecialEnemyRotationAngle;

    public virtual void Init(float trackSpeed)
    {
        TrackSpeed = trackSpeed;

        _SetRotationAngle = new Vector3(0f, 0f, 0f);
        _SetSpecialEnemyRotationAngle = new Vector3(0f, 180f, 0f);

        _PositionToLerpTo = new Vector3(this.transform.position.x, this.transform.position.y, -150f);

        StartMovingOnTrack = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateObstacle();
    }

    public virtual void UpdateObstacle()
    {
        if (StartMovingOnTrack && !GameplayManager.Instance.GameplayPaused)
        {
            // Building moves on track at a speed over time.
            transform.position = Vector3.Lerp(transform.position, _PositionToLerpTo, TrackSpeed);
            
            // If obstacle is an enemy it needs to be rotated a different way for now.
            // TODO fix this stupid system...
            if (this.gameObject.tag != "Enemy")
            {
                transform.localEulerAngles = _SetRotationAngle;
            }
            else
            {
                transform.localEulerAngles = _SetSpecialEnemyRotationAngle;
            }

            // Destroy if it passes a certain threshold distance.
            if (this.transform.position.z <= DESTROY_Z_DISTANCE_THRESHOLD)
            {
                DestroyImmediate(this.gameObject);
            }
        }
    }
}
