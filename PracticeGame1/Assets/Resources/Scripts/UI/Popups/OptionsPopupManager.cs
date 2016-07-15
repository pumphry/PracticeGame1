using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsPopupManager : MonoBehaviour
{

    public Toggle MusicMuteToggle;

    void Awake()
    {
        // Get current toggle state for gameplay music mute setting.
        if (MusicMuteToggle != null)
        {
            int isMuted = PlayerPrefsManager.Instance.GetPlayerPrefIntVal(PlayerPrefsManager.PlayerPrefKeyNames.MuteGameplayMusicIntVal);
            if (isMuted == 0)
            {
                MusicMuteToggle.isOn = false;
            }
            else
            {
                MusicMuteToggle.isOn = true;
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
	    
	}

    public void CloseOptionsPopup()
    {
        Debug.Log("Closing the Options Popup.");
        UIManager.Instance.RemoveLastPopupListed();
    }

    public void ToggleGameplayMusicMuteButtonPressed()
    {
        if(MusicMuteToggle.isOn == false)
        {
            if (PlayerPrefsManager.Instance.GetPlayerPrefIntVal(PlayerPrefsManager.PlayerPrefKeyNames.MuteGameplayMusicIntVal) == 0)
            {
                // Do nothing and return out of this function.
                return;
            }
        }
        else
        {
            if(PlayerPrefsManager.Instance.GetPlayerPrefIntVal(PlayerPrefsManager.PlayerPrefKeyNames.MuteGameplayMusicIntVal) == 1)
            {
                // Do nothing and return out of this function.
                return;
            }
        }

        if (PlayerPrefsManager.Instance.GetPlayerPrefIntVal(PlayerPrefsManager.PlayerPrefKeyNames.MuteGameplayMusicIntVal) == 0)
        {
            PlayerPrefsManager.Instance.SetPlayerPrefIntVal(PlayerPrefsManager.PlayerPrefKeyNames.MuteGameplayMusicIntVal, 1);

            Debug.Log("PlayerPrefs for MuteGameplayMusicIntVal set to 1 (true). Gameplay music now muted.");
        }
        else
        {
            PlayerPrefsManager.Instance.SetPlayerPrefIntVal(PlayerPrefsManager.PlayerPrefKeyNames.MuteGameplayMusicIntVal, 0);

            Debug.Log("PlayerPrefs for MuteGameplayMusicIntVal set to 0 (false). Gameplay music now un-muted.");
        }

        // TODO make this check for if in game first in the future.
        AudioManager.Instance.PlayGameplayMusic();
    }
}
