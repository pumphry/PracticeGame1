using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameplayHudManager : MonoBehaviour {

    public GameObject TopHudOverlay;

    public Text GameStartCountdownTimerText;

    public Text GameDurationTimer;

    private bool _GameplayEnded = false;

    public float GameTimeDuration;
    private bool _GameStarted = false;
    private const float GAME_START_DURATION_TIME = 0f;
    private const string TIMER_TEXT_PREFIX = "TIME: ";

    // Use this for initialization
    void Start ()
    {
        _GameStarted = false;
        _GameplayEnded = false;

        GameDurationTimer.gameObject.SetActive(false);

        GameTimeDuration = GAME_START_DURATION_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_GameplayEnded)
        {
            if (!GameplayManager.Instance.GameOver)
            {
                if (GameplayManager.Instance != null)
                {
                    if (GameplayManager.Instance.TimeLeftOnCountdownInSeconds > 0)
                    {
                        GameStartCountdownTimerText.gameObject.SetActive(true);

                        GameStartCountdownTimerText.text = GameplayManager.Instance.TimeLeftOnCountdownInSeconds.ToString();
                    }
                    else
                    {
                        GameStartCountdownTimerText.gameObject.SetActive(false);

                        GameDurationTimer.gameObject.SetActive(true);
                        _GameStarted = true;
                    }
                }

                if (_GameStarted)
                {
                    GameTimeDuration += Time.deltaTime;

                    GameDurationTimer.text = TIMER_TEXT_PREFIX + (GameTimeDuration).ToString("F2");
                }
            }
            else
            {
                // Stop running timer since gameplay has ended.
                _GameplayEnded = true;

                _GameStarted = false;

                // Check if this run time is the personal best run time.
                GameplayManager.Instance.DetermineIfBestRunTime(GameTimeDuration);

                GameTimeDuration = 0f;
            }
        }
        else
        {
            // Stop running timer since gameplay has ended.
            _GameplayEnded = false;

            _GameStarted = false;

            // Check if this run time is the personal best run time.
            GameplayManager.Instance.DetermineIfBestRunTime(GameTimeDuration);

            GameDurationTimer.gameObject.SetActive(false);

            GameTimeDuration = 0f;
        }
    }
}
