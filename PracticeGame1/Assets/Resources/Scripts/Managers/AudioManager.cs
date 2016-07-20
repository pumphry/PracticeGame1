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
    public List<AudioSource> GameplayAudioSources = new List<AudioSource>();
    public List<AudioSource> FrontEndAudioSources = new List<AudioSource>();
    public List<AudioSource> ActiveAudioSources = new List<AudioSource>();

    public enum AudioSources { FrontEndAudioSources, GameplayAudioSources };

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
    }

    public void SetFrontEndAudioScource(List<AudioSource> frontEndAudioScources)
    {
        if(frontEndAudioScources.Count > 0)
        {
            FrontEndAudioSources = frontEndAudioScources;

            ActiveAudioSources = FrontEndAudioSources;
        }
    }

    public void SetGameplayAudioSource(List<AudioSource> gameplayAudioSources)
    {
        if (gameplayAudioSources.Count > 0)
        {
            GameplayAudioSources = gameplayAudioSources;
        }
    }

    public void TogglePrimaryAudioSources(AudioSources audioSourcesToToggleOn)
    {
        if (audioSourcesToToggleOn == AudioSources.FrontEndAudioSources)
        {
            ActiveAudioSources.Clear();

            ActiveAudioSources = FrontEndAudioSources;

            foreach (AudioSource audioSource in FrontEndAudioSources)
            {
                FrontEndAudioSources[0].gameObject.GetComponent<AudioListener>().enabled = true;

                audioSource.enabled = true;
            }

            foreach (AudioSource audioSource in GameplayAudioSources)
            {
                audioSource.enabled = false;
            }
        }

        if (audioSourcesToToggleOn == AudioSources.GameplayAudioSources)
        {
            ActiveAudioSources.Clear();

            ActiveAudioSources = GameplayAudioSources;

            foreach (AudioSource audioSource in FrontEndAudioSources)
            {
                FrontEndAudioSources[0].gameObject.GetComponent<AudioListener>().enabled = false;

                audioSource.enabled = false;
            }

            foreach (AudioSource audioSource in GameplayAudioSources)
            {
                audioSource.enabled = true;
            }
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
        if(ActiveAudioSources[0] != null && ActiveAudioSources[0].enabled == true)
        {
            ActiveAudioSources[0].clip = GameplayMusicTrack1;
            ActiveAudioSources[0].Play();
            ActiveAudioSources[0].loop = true;
        }
    }

    public void StopMusicTrack()
    {
        if (ActiveAudioSources[0] != null)
        {
            ActiveAudioSources[0].clip = GameplayMusicTrack1;
            ActiveAudioSources[0].Stop();
        }
    }

    public void PlaySFXClip(SFXClips sfxClipToPlay)
    {
        int indexClipToPlay = -1;

        switch(sfxClipToPlay)
        {
            case SFXClips.BowShotSFX:
                indexClipToPlay = 1;
                ActiveAudioSources[1].clip = BowShotSFX; 
                break;
            case SFXClips.OuchSFX:
                indexClipToPlay = 2;
                ActiveAudioSources[2].clip = OuchSFX;
                break;
            case SFXClips.PopupOpeningSFX:
                indexClipToPlay = 3;
                ActiveAudioSources[3].clip = PopupOpeningSFX;
                break;
            default:
                break;
        }

        if(indexClipToPlay >= 0 && ActiveAudioSources[indexClipToPlay].clip != null)
        {
            ActiveAudioSources[indexClipToPlay].loop = false;
            ActiveAudioSources[indexClipToPlay].Play();
        }
    }
}
