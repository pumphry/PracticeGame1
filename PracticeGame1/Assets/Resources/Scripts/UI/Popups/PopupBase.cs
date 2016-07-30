using UnityEngine;
using System.Collections;

/// <summary>
/// Base script for all Popups to derive from.
/// </summary>
public class PopupBase : MonoBehaviour {

    void Awake()
    {
        GameStartupManager.OnEnteringFrontEnd += UpdatePopupData;
    }

    /// <summary>
    /// Update the popup data on entering the FrontEnd or other scenarios here.
    /// </summary>
    public virtual void UpdatePopupData()
    {
        
    }

    void DestroyImmediate()
    {
        GameStartupManager.OnEnteringFrontEnd -= UpdatePopupData;
    }

    void Destroy()
    {
        GameStartupManager.OnEnteringFrontEnd -= UpdatePopupData;
    }
}
