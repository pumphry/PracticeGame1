using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameplayHudManager : MonoBehaviour {

    public Text CountdownTimerText;

	// Use this for initialization
	void Start ()
    {

	}

    // Update is called once per frame
    void Update()
    {
        if (GameplayManager.Instance != null)
        { 
            if (GameplayManager.Instance.TimeLeftOnCountdownInSeconds > 0)
            {
                CountdownTimerText.gameObject.SetActive(true);

                CountdownTimerText.text = GameplayManager.Instance.TimeLeftOnCountdownInSeconds.ToString();
            }
            else
            {
                CountdownTimerText.gameObject.SetActive(false);
            }
        }
    }
}
