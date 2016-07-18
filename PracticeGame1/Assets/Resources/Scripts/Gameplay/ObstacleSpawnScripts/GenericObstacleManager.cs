using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GenericObstacleManager : MonoBehaviour
{
    private bool _StartMovingOnTrack = false;

    public float TrackSpeed = 0.001f;

    private Vector3 _PositionToLerpTo = Vector3.zero;

    private const float DESTROY_Z_DISTANCE_THRESHOLD = -100f;

    public void Init(float trackSpeed)
    {
        TrackSpeed = trackSpeed;

        _PositionToLerpTo = new Vector3(this.transform.position.x, this.transform.position.y, -150f);

        _StartMovingOnTrack = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_StartMovingOnTrack && !GameplayManager.Instance.GameplayPaused)
        {
            // Building moves on track at a speed over time.
            transform.position = Vector3.Lerp(transform.position, _PositionToLerpTo, TrackSpeed);

            // Destroy if it passes a certain threshold distance.
            if(this.transform.position.z <= DESTROY_Z_DISTANCE_THRESHOLD)
            {
                DestroyImmediate(this.gameObject);
            }
        }
    }
}
