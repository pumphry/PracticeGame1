using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// Manager that handles all PlayerPrefs file saving updating, clearing, and deleting for persistent player preferences. Note: int values of 0 are false and 1 are true.
/// </summary>
public class PlayerPrefsManager : MonoBehaviourSingleton<PlayerPrefsManager>
{
    // PlayerPrefs key names enum list. Each new value that we add to the game should have the key name added here as part of the enum!
    public enum PlayerPrefKeyNames { BestTimeFloatVal, MuteGameplayMusicIntVal, MuteSoundFXIntVal };

    // Use this for initialization
    void Start()
    {

    }

    /// <summary>
    /// Updates a player pref float value. Recquires the player to pass in the player pref key name for it to match with.
    /// </summary>
    /// <param name="playerPrefKeyName"></param>
    /// <param name="floatVal"></param>
    public void SetPlayerPrefFloatVal (PlayerPrefKeyNames playerPrefKeyName, float floatVal)
    {
        if (playerPrefKeyName.ToString() != null)
        {
            PlayerPrefs.SetFloat(playerPrefKeyName.ToString(), floatVal);

            SaveAllPlayerPrefs();
        }
    }

    /// <summary>
    /// Gets a player pref float value by passing through the player pref key name.
    /// </summary>
    /// <param name="playerPrefKeyName"></param>
    /// <returns></returns>
    public float GetPlayerPrefFloatVal(PlayerPrefKeyNames playerPrefKeyName)
    {
        return PlayerPrefs.GetFloat(playerPrefKeyName.ToString(), 0f);
    }

    /// <summary>
    /// Updates a player pref int value. Recquires the player to pass in the player pref key name for it to match with.
    /// </summary>
    /// <param name="playerPrefKeyName"></param>
    /// <param name="intVal"></param>
    public void SetPlayerPrefIntVal (PlayerPrefKeyNames playerPrefKeyName, int intVal)
    {
        if (playerPrefKeyName.ToString() != null)
        {
            PlayerPrefs.SetInt(playerPrefKeyName.ToString(), intVal);

            SaveAllPlayerPrefs();
        }
    }

    /// <summary>
    /// Gets a player pref int value by passing through the player pref key name.
    /// </summary>
    /// <param name="playerprefKeyName"></param>
    /// <returns></returns>
    public int GetPlayerPrefIntVal (PlayerPrefKeyNames playerprefKeyName)
    {
        return PlayerPrefs.GetInt (playerprefKeyName.ToString(), 0);
    }
    
    /// <summary>
    /// Saves all of the currently existing player pref values.
    /// </summary>
    public void SaveAllPlayerPrefs ()
    {
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Clears all player prefs.
    /// </summary>
    public void ClearAllPlayerPrefs ()
    {
        // TODO
    }
}
