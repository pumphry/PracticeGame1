using UnityEngine;
using System.Collections;

public class PlayerCollisionsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(GameplayManager.Instance != null)
        {
            if (other.tag == "Obstacle")
            {
                CreateGameOverPopup("You hit an obstacle!");
            }

            if (other.tag == "Pike")
            {
                CreateGameOverPopup("Ouch! You ran into a row of pikes!");
            }

            if (other.tag == "Building")
            {
                CreateGameOverPopup("You hit a building!");
            }
        }
    }

    private void CreateGameOverPopup(string message)
    {
        GameplayManager.Instance.OpenGameOverPopup();

        GameObject popup = GameObject.Find("GameOverPopup(Clone)");
        if (popup != null)
        {
            GameOverPopup popupScript = popup.GetComponent<GameOverPopup>();
            if (popupScript != null)
            {
                popupScript.Init(message);
            }
        }
    }
}
