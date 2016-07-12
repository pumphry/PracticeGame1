using UnityEngine;
using System.Collections;

public class OptionsPopupManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void CloseOptionsPopup()
    {
        Debug.Log("Closing the Options Popup.");
        UIManager.Instance.RemoveLastPopupListed();
    }
}
