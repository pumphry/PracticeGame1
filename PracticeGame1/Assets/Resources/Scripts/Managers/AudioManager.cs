using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages all game audio both on FrontEnd and in Gameplay.
/// </summary>
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    public AudioSource GameplayAudioSource;

    // Music
    public AudioClip gameplayMusicTrack1; 

    // SFX

    // Use this for initialization
    void Start () {
        Debug.Assert(gameplayMusicTrack1 != null);
	}

    public void SetGameplayAudioSource(AudioSource gameplayAudioSource)
    {
        if (gameplayAudioSource != null)
        {
            GameplayAudioSource = gameplayAudioSource;
        }

        PlayGameplayMusic();
    }

    public void PlayGameplayMusic ()
    {
        if (PlayerPrefsManager.Instance.GetPlayerPrefIntVal(PlayerPrefsManager.PlayerPrefKeyNames.MuteGameplayMusicIntVal) == 0)
        {
            // Play music.
            PlayMusicTrack();
        }
        else
        {
            // Stop playing music.
            StopMusicTrack();
        }
    }

    public void PlayMusicTrack()
    {
        if(GameplayAudioSource != null)
        {
            GameplayAudioSource.clip = gameplayMusicTrack1;
            GameplayAudioSource.Play();
            GameplayAudioSource.loop = true;
        }
    }

    public void StopMusicTrack()
    {
        if (GameplayAudioSource != null)
        {
            GameplayAudioSource.clip = gameplayMusicTrack1;
            GameplayAudioSource.Stop();
        }
    }
}
