using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class InfiniteRunnerPlayerController : MonoBehaviour
{
    private Animator _Animator;

    public List<Transform> ListOfPlayerMovementZones = new List<Transform>();

    private enum _MovementDirections { Left, Right }

    private bool _InputBlocked = false;

    // Zone left-right movement variables.
    public Transform ZoneToMoveTo;
    public Transform CurrentZone;
    private int _CurrentZoneIndex;
    private bool _MovingBetweenZones = false;

    // Jumping action variables.
    private bool _PlayerJumping = false;
    private const float TOTAL_JUMP_DURATION = 1.0f;
    private float JUMP_HEIGHT = 1.0f;
    private float _Incrementor = 0f;
    private const float AMOUNT_TO_INCREMENT = 0.4f;

    void Awake()
    {
        _InputBlocked = true;

        _Animator = gameObject.GetComponent<Animator>();

        StartCoroutine("WaitThenGivePlayerControl");
    }

    private IEnumerator WaitThenGivePlayerControl()
    {
        yield return new WaitForSeconds(2.0f);

        _InputBlocked = false;
    }

    public void PopulatePlayerMovementZonesList(List<Transform> listOfPlayerMovementZones)
    {
        if (listOfPlayerMovementZones.Count > 0)
        {
            ListOfPlayerMovementZones = listOfPlayerMovementZones;

            CurrentZone = listOfPlayerMovementZones[2];
            _CurrentZoneIndex = 2;
            ZoneToMoveTo = CurrentZone;
        }
    }

    void UpdateAnimator(float move)
    {
        // update the animator parameters
        _Animator.SetFloat("Speed", move);
        _Animator.SetFloat("Forward", 1.0f, 0.1f, Time.deltaTime);
        if (!_PlayerJumping)
        {
            _Animator.SetBool("OnGround", true);
        }
        else
        {
            _Animator.SetBool("OnGround", false);
        }
        _Animator.Play("HumanoidRun", 0);
    }

    void Update()
    {
        float move = 1.0f;

        UpdateAnimator(move);

        if (!_InputBlocked)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                DetermineZoneToMoveTo(_MovementDirections.Left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                DetermineZoneToMoveTo(_MovementDirections.Right);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _InputBlocked = true;

                JumpAction();
            }
        }

        if (_MovingBetweenZones)
        {
            Vector3 from = CurrentZone.localPosition;
            Vector3 To = ZoneToMoveTo.localPosition;

            transform.position = Vector3.Lerp(from, To, Time.time);

            transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

            if(transform.position == To)
            {
                _MovingBetweenZones = false;

                _InputBlocked = false;
            }
        }

        if (_PlayerJumping)
        {
            _Incrementor += AMOUNT_TO_INCREMENT;
            Vector3 currentPos = Vector3.Lerp(transform.localPosition, transform.localPosition, _Incrementor);
            currentPos.y += JUMP_HEIGHT * Mathf.Sin(Mathf.Clamp01(_Incrementor) * Mathf.PI);
            transform.localPosition = currentPos;

            transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

            // If the player is back on the ground then reset the jump variables and allow player input again.
            if (transform.localPosition.y <= 0f)
            {
                _Incrementor = 0f;

                _PlayerJumping = false;

                _InputBlocked = false;
            }
        }
    }

    private void DetermineZoneToMoveTo(_MovementDirections movementDirection)
    {
        if(movementDirection == _MovementDirections.Left)
        {
            if (_CurrentZoneIndex > 0)
            {
                ZoneToMoveTo = ListOfPlayerMovementZones[_CurrentZoneIndex - 1];

                _CurrentZoneIndex = _CurrentZoneIndex - 1;

                _InputBlocked = true;

                _MovingBetweenZones = true;
            }
        }

        if(movementDirection == _MovementDirections.Right)
        {
            if (_CurrentZoneIndex < ListOfPlayerMovementZones.Count - 1)
            {
                ZoneToMoveTo = ListOfPlayerMovementZones[_CurrentZoneIndex + 1];

                _CurrentZoneIndex = _CurrentZoneIndex + 1;

                _InputBlocked = true;

                _MovingBetweenZones = true;
            }
        }
    }

    private void JumpAction()
    {
        _PlayerJumping = true;

        AudioManager.Instance.PlaySFXClip(AudioManager.SFXClips.YeahSFX);
    }
}
