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
    // Audio Sources
    public AudioSource GameplayAudioSource;
    public AudioSource FrontEndAudioSource;
    public AudioSource ActiveAudioSource;

    public enum AudioSources { FrontEndAudioSource, GameplayAudioSource };

    // Music
    public AudioClip GameplayMusicTrack1;

    public enum MusicClips { GameplayMusicTrack1 };

    // SFX
    public AudioClip OuchSFX;
    public AudioClip BowShotSFX;
    public AudioClip PopupOpeningSFX;

    public enum SFXClips { OuchSFX, BowShotSFX, PopupOpeningSFX };

    // Use this for initialization
    void Start () {
        Debug.Assert(GameplayMusicTrack1 != null);
        Debug.Assert(OuchSFX != null);
        Debug.Assert(BowShotSFX != null);
        Debug.Assert(PopupOpeningSFX != null);

        ActiveAudioSource = FrontEndAudioSource;
    }

    public void SetFrontEndAudioScource(AudioSource frontEndAudioScource)
    {
        if(frontEndAudioScource != null)
        {
            FrontEndAudioSource = frontEndAudioScource;
        }
    }

    public void SetGameplayAudioSource(AudioSource gameplayAudioSource)
    {
        if (gameplayAudioSource != null)
        {
            GameplayAudioSource = gameplayAudioSource;
        }
    }

    public void TogglePrimaryAudioSource(AudioSources audioSourceToToggleOn)
    {
        if (audioSourceToToggleOn == AudioSources.FrontEndAudioSource)
        {
            FrontEndAudioSource.gameObject.GetComponent<AudioListener>().enabled = true;

            ActiveAudioSource = FrontEndAudioSource;

            FrontEndAudioSource.enabled = true;
            GameplayAudioSource.enabled = false;
        }

        if (audioSourceToToggleOn == AudioSources.GameplayAudioSource)
        {
            FrontEndAudioSource.gameObject.GetComponent<AudioListener>().enabled = false;

            ActiveAudioSource = GameplayAudioSource;

            FrontEndAudioSource.enabled = false;
            GameplayAudioSource.enabled = true;
        }
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
        if(ActiveAudioSource != null && ActiveAudioSource.enabled == true)
        {
            ActiveAudioSource.clip = GameplayMusicTrack1;
            ActiveAudioSource.Play();
            ActiveAudioSource.loop = true;
        }
    }

    public void StopMusicTrack()
    {
        if (ActiveAudioSource != null)
        {
            ActiveAudioSource.clip = GameplayMusicTrack1;
            ActiveAudioSource.Stop();
        }
    }

    public void PlaySFXClip(SFXClips sfxClipToPlay)
    {
        switch(sfxClipToPlay)
        {
            case SFXClips.BowShotSFX:
                ActiveAudioSource.clip = BowShotSFX; 
                break;
            case SFXClips.OuchSFX:
                ActiveAudioSource.clip = OuchSFX;
                break;
            case SFXClips.PopupOpeningSFX:
                ActiveAudioSource.clip = PopupOpeningSFX;
                break;
            default:
                ActiveAudioSource.clip = null;
                break;
        }

        if(ActiveAudioSource.clip != null)
        {
            ActiveAudioSource.loop = false;
            ActiveAudioSource.Play();
        }
    }
}
