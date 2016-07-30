using UnityEngine;
using System.Collections;

public class PlayerCollisionsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    /// <summary>
    /// Check for tag names for anything the player object collides with to see if it triggers something in the gameplay.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if(GameplayManager.Instance != null)
        {
            if (other.tag == "Obstacle")
            {
                AudioManager.Instance.PauseSFXClip(AudioManager.SFXClips.RunningThroughGrassSFX);

                AudioManager.Instance.PlaySFXClip(AudioManager.SFXClips.OuchSFX);

                EnterGameOverPhase("You hit an obstacle!");

                return;
            }

            if (other.tag == "Pike")
            {
                AudioManager.Instance.PauseSFXClip(AudioManager.SFXClips.RunningThroughGrassSFX);

                AudioManager.Instance.PlaySFXClip(AudioManager.SFXClips.OuchSFX);

                EnterGameOverPhase("Ouch! You ran into a row of pikes!");

                return;
            }

            if (other.tag == "Hill")
            {
                AudioManager.Instance.PauseSFXClip(AudioManager.SFXClips.RunningThroughGrassSFX);

                AudioManager.Instance.PlaySFXClip(AudioManager.SFXClips.OuchSFX);

                EnterGameOverPhase("Ugh! You tripped over a hill!");

                return;
            }

            if (other.tag == "Enemy")
            {
                AudioManager.Instance.PauseSFXClip(AudioManager.SFXClips.RunningThroughGrassSFX);

                AudioManager.Instance.PlaySFXClip(AudioManager.SFXClips.OuchSFX);

                EnterGameOverPhase("Oh no an enemy has caught you!");

                return;
            }

            if (other.tag == "Projectile")
            {
                AudioManager.Instance.PauseSFXClip(AudioManager.SFXClips.RunningThroughGrassSFX);

                AudioManager.Instance.PlaySFXClip(AudioManager.SFXClips.OuchSFX);

                EnterGameOverPhase("You were shot by an archer!");

                return;
            }
        }
    }

    private void EnterGameOverPhase(string gameOverMessage)
    {
        GameplayManager.Instance.EnterGameOverPhase();

        // When the GameplayManager enters the game over phase it creates a GameOverPopup clone. Find it and init it.
        GameObject popup = GameObject.Find("GameOverPopup(Clone)");
        if (popup != null)
        {
            GameOverPopup popupScript = popup.GetComponent<GameOverPopup>();
            if (popupScript != null)
            {
                popupScript.Init(gameOverMessage);
            }
        }
    }
}
