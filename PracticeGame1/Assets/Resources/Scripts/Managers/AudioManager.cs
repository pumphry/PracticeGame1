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
    public AudioSources CurrentAudioSources = AudioSources.FrontEndAudioSources;

    // Music
    public AudioClip GameplayMusicTrack1;

    public enum MusicClips { GameplayMusicTrack1 };

    // SFX
    public AudioClip OuchSFX;
    public AudioClip BowShotSFX;
    public AudioClip PopupOpeningSFX;
    public AudioClip YeahSFX;
    public AudioClip RunningThroughGrassSFX;

    public enum SFXClips { OuchSFX, BowShotSFX, PopupOpeningSFX, YeahSFX, RunningThroughGrassSFX };

    // Use this for initialization
    void Start () {
        Debug.Assert(GameplayMusicTrack1 != null);
        Debug.Assert(OuchSFX != null);
        Debug.Assert(BowShotSFX != null);
        Debug.Assert(PopupOpeningSFX != null);
        Debug.Assert(YeahSFX != null);
        Debug.Assert(RunningThroughGrassSFX != null);
    }

    public void SetFrontEndAudioScource(List<AudioSource> frontEndAudioScources)
    {
        if(frontEndAudioScources.Count > 0)
        {
            FrontEndAudioSources = frontEndAudioScources;

            ActiveAudioSources = FrontEndAudioSources;
            CurrentAudioSources = AudioSources.FrontEndAudioSources;
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
            CurrentAudioSources = AudioSources.FrontEndAudioSources;

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
            CurrentAudioSources = AudioSources.GameplayAudioSources;

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
            if (CurrentAudioSources == AudioSources.GameplayAudioSources)
            {
                // Play music.
                PlayMusicTrack();
            }
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

    public void PlaySFXClip(SFXClips sfxClipToPlay, bool isLooping = false)
    {
        if (PlayerPrefsManager.Instance.GetPlayerPrefIntVal(PlayerPrefsManager.PlayerPrefKeyNames.MuteSoundFXIntVal) == 0)
        {
            int indexClipToPlay = -1;

            switch (sfxClipToPlay)
            {
                // Running through grass loops indefinitely.
                case SFXClips.RunningThroughGrassSFX:
                    indexClipToPlay = 1;
                    if (ActiveAudioSources.Count >= indexClipToPlay)
                    {
                        ActiveAudioSources[indexClipToPlay].clip = RunningThroughGrassSFX;
                    }
                    break;
                case SFXClips.BowShotSFX:
                    indexClipToPlay = 3;
                    if (ActiveAudioSources.Count >= indexClipToPlay)
                    {
                        ActiveAudioSources[indexClipToPlay].clip = BowShotSFX;
                    }
                    break;
                case SFXClips.OuchSFX:
                    indexClipToPlay = 2;
                    if (ActiveAudioSources.Count >= indexClipToPlay)
                    {
                        ActiveAudioSources[indexClipToPlay].clip = OuchSFX;
                    }
                    break;
                case SFXClips.PopupOpeningSFX:
                    indexClipToPlay = 3;
                    if (ActiveAudioSources.Count >= indexClipToPlay)
                    {
                        ActiveAudioSources[indexClipToPlay].clip = PopupOpeningSFX;
                    }
                    break;
                case SFXClips.YeahSFX:
                    indexClipToPlay = 2;
                    if (ActiveAudioSources.Count >= indexClipToPlay)
                    {
                        ActiveAudioSources[indexClipToPlay].clip = YeahSFX;
                    }
                    break;
                default:
                    break;
            }

            if (indexClipToPlay >= 0 && ActiveAudioSources.Count >= indexClipToPlay && ActiveAudioSources[indexClipToPlay].clip != null)
            {
                ActiveAudioSources[indexClipToPlay].loop = isLooping;
                ActiveAudioSources[indexClipToPlay].Play();
            }
        }
    }

    public void PauseSFXClip(AudioManager.SFXClips sfxClipToPause)
    {
        if(sfxClipToPause == AudioManager.SFXClips.RunningThroughGrassSFX)
        {
            ActiveAudioSources[1].Pause();
        }
    }

    public void UnpauseSFXClip(AudioManager.SFXClips sfxClipToUnpause)
    {
        if(sfxClipToUnpause == AudioManager.SFXClips.RunningThroughGrassSFX)
        {
            ActiveAudioSources[1].UnPause();
        }
    }

    public void PauseAllSFXClips()
    {
        int audioSourceIndex = 0;

        foreach(AudioSource audioSource in ActiveAudioSources)
        {
            // We skip the first Audio Source since it is going to be the gameplay music and/or frontend music and we don't want to pause this.
            if (audioSourceIndex == 0)
            {
                if (audioSource.clip != null)
                {
                    audioSource.Pause();
                }
            }
            audioSourceIndex++;
        }
    }

    public void UnpauseAllSFXClips()
    {
        int audioSourceIndex = 0;

        foreach (AudioSource audioSource in ActiveAudioSources)
        {
            // We skip the first Audio Source since it is going to be the gameplay music and/or frontend music and we didn't pause this.
            if (audioSourceIndex == 0)
            {
                if (audioSource.clip != null)
                {
                    audioSource.Play();
                }
            }
            audioSourceIndex++;
        }
    }
}
