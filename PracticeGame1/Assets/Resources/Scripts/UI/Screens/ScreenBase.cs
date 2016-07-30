using UnityEngine;
using System.Collections;

public class ScreenBase : MonoBehaviour {

    void Awake()
    {
        GameStartupManager.OnEnteringFrontEnd += UpdateScreenData;
    }

    /// <summary>
    /// Update the popup data on entering the FrontEnd or other scenarios here.
    /// </summary> 
    public virtual void UpdateScreenData()
    {

    }

    void DestroyImmediate()
    {
        GameStartupManager.OnEnteringFrontEnd -= UpdateScreenData;
    }

    void Destroy()
    {
        GameStartupManager.OnEnteringFrontEnd -= UpdateScreenData;
    }
}
