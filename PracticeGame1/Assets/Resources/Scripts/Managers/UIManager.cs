using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// User interface manager that instantiates the three layers of the UI (Screen, Overlay, and Popup) and is used to load and unload UI on each of these levels.
/// </summary>
public class UIManager : MonoBehaviourSingleton<UIManager> 
{
	
	// Enum group of UI Screen names that get translated to strings and are used to load the screen prefabs for each screen.
	public enum UIScreens { MainMenuScreen, PauseGameplayScreen }; 

	// Enum group of Overlay names that get translated to strings and are used to load the overlay prefabs for each overlay.
	public enum UIOverlays { FrontEndHud, GameplayHud };

	// Enum group of (unique, non-generic) UI Popup names that get translated to strings and are used to load the popup prefabs for each popup.
	public enum UIPopups { OptionsPopup }; 

	public List<string> LoadedScreenNames = new List<string> ();
	public List<string> LoadedOverlayNames = new List<string> ();
	public List<string> LoadedPopupNames = new List<string> ();

    public List<GameObject> LoadedScreens = new List<GameObject> ();
    public List<GameObject> LoadedOverlays = new List<GameObject>();
    public List<GameObject> LoadedPopups = new List<GameObject>();

    public const string UI_SCREEN_PREFABS_PATH = "Prefabs/UI/Screens/";
	public const string UI_OVERLAY_PREFABS_PATH = "Prefabs/UI/Overlays/";
	public const string UI_POPUP_PREFABS_PATH = "Prefabs/UI/Popups/";

	private const string UI_ROOT_NAME = "UIRoot";
	private const string UI_SCREEN_LAYER_NAME = "UIScreenLayer";
	private const string UI_OVERLAY_LAYER_NAME = "UIOverlayLayer";
	private const string UI_POPUP_LAYER_NAME = "UIPopupLayer";

    private const string CLONE_STR = "(Clone)";

	// Event setup that will call once all the of the UI framework is done building.
	public delegate void UIFrameworkFinishedBuilding();
	public static event UIFrameworkFinishedBuilding OnUIFrameworkFinishedBuilding;

	private GameObject _UIRoot;
	private GameObject _UIScreenLayer;
	private RectTransform _UIScreenLayerRect;
	private GameObject _UIOverlayLayer;
	private RectTransform _UIOverlayLayerRect;
	private GameObject _UIPopupLayer;
	private RectTransform _UIPopupLayerRect;

	private const float LOAD_INTO_START_SCREEN_DELAY = 1.0f;

	// Use this for initialization
	void Start () 
	{
		Debug.Log ("UIManager is active.");

		ClearUIManagerVariables ();

		CreateUIRoot ();

	}

	void ClearUIManagerVariables()
	{
		_UIRoot = null;
		_UIScreenLayer = null;
		_UIPopupLayer = null;

		LoadedScreenNames.Clear ();
		LoadedPopupNames.Clear ();
        LoadedOverlayNames.Clear();

        LoadedScreens.Clear ();
        LoadedPopups.Clear ();
        LoadedOverlays.Clear ();
	}

	void CreateUIRoot()
	{
		// Create a UIRoot that will host the two layers of the UI (screen layer and popup layer).
		GameObject _UIRoot = new GameObject();
		_UIRoot.transform.position = Vector3.zero;
		_UIRoot.transform.localScale = Vector3.one;
		_UIRoot.name = UI_ROOT_NAME;
		Canvas canvas = _UIRoot.AddComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.pixelPerfect = false;
		canvas.sortingOrder = 0;
		CanvasScaler canvasScaler = _UIRoot.AddComponent<CanvasScaler> ();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
		canvasScaler.scaleFactor = 1f;
		canvasScaler.referencePixelsPerUnit = 100f;
		GraphicRaycaster graphicRaycaster = _UIRoot.AddComponent<GraphicRaycaster> ();
		graphicRaycaster.ignoreReversedGraphics = true;
		graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

        Debug.Log ("UIRoot has been created.");

		AddUILayersToRoot (_UIRoot);
	}

	void AddUILayersToRoot(GameObject UIRoot)
	{
		if (UIRoot != null) 
		{
			// Create the UIScreenLayer GameObject first so it is the first in Heirarchy.
			GameObject _UIScreenLayer = new GameObject ();
			_UIScreenLayer.name = UI_SCREEN_LAYER_NAME;
			_UIScreenLayer.transform.parent = UIRoot.transform;
			_UIScreenLayer.transform.localPosition = Vector3.zero;
			_UIScreenLayer.transform.localScale = Vector3.one;
			_UIScreenLayerRect = _UIScreenLayer.AddComponent<RectTransform> ();

			// Create the UIOverlayLayer GameObjet second so it is second in the Heirarchy and will draw
			// UI elements OVER the UIScreenLayer whent they are parented to it.
			GameObject _UIOverlayLayer = new GameObject ();
			_UIOverlayLayer.name = UI_OVERLAY_LAYER_NAME;
			_UIOverlayLayer.transform.parent = UIRoot.transform;
			_UIOverlayLayer.transform.localPosition = Vector3.zero;
			_UIOverlayLayer.transform.localScale = Vector3.one;
			_UIOverlayLayerRect = _UIOverlayLayer.AddComponent<RectTransform> ();

			// Create the UIPopupLayer GameObject third so it is third in the Heirarchy and will draw 
			//UI elements OVER the UIScreenLayer and the UIOverlayLayer when they are parented to it.
			GameObject _UIPopupLayer = new GameObject ();
			_UIPopupLayer.name = UI_POPUP_LAYER_NAME;
			_UIPopupLayer.transform.parent = UIRoot.transform;
			_UIPopupLayer.transform.localPosition = Vector3.zero;
			_UIPopupLayer.transform.localScale = Vector3.one;
			_UIPopupLayerRect = _UIPopupLayer.AddComponent<RectTransform> ();

			// The three UI docking components (UIRoot, UIScreenLayer, and UIPopupLayer) are finished building.
			Debug.Log ("UIScreenLayer, UIOverlayLayer, and UIPopupLayer have been created as children of UIRoot.");
			OnUIFrameworkFinishedBuilding ();
		}
	}

	/// <summary>
	/// Loads the desired screen if not already loaded and then switches to that screen in UI view if switchToScreenOnLoad is true.
	/// </summary>
	/// <param name="screenToLoad">Screen to load.</param>
	/// <param name="switchToScreenOnLoad">If set to <c>true</c> switch to screen on load.</param>
	public void LoadScreen(UIScreens screenToLoad, bool switchToScreenOnLoad = true)
	{
		bool screenPreviouslyLoaded = false;
		// Check all screen names in LoadedScreenNames list to make sure this screen hasn't already been loaded previously.
		if (LoadedScreenNames.Count > 0) 
		{
			foreach (string screenName in LoadedScreenNames) 
			{
				if (screenName == screenToLoad.ToString ()) 
				{
					screenPreviouslyLoaded = true;
				}
			}
		}

		// If the screen hasn't been loaded previously, then load it and make it a child of the _UIScreenLayer.
		if (!screenPreviouslyLoaded) {
			GameObject screen = Instantiate (Resources.Load (UI_SCREEN_PREFABS_PATH + screenToLoad.ToString (), typeof(GameObject))) as GameObject;

			if (screen != null) {
				RectTransform screenRect = screen.GetComponent<RectTransform> ();

				if (screenRect != null) {
					screenRect.transform.SetParent (_UIScreenLayerRect.transform, true);
					screenRect.transform.localPosition = Vector3.zero;
					screenRect.transform.localScale = Vector3.one;
				}
			}

			// Toggle the loaded screen either active or inactive.
			screen.gameObject.SetActive (switchToScreenOnLoad);

			LoadedScreenNames.Add (screenToLoad.ToString ());
            LoadedScreens.Add(screen.gameObject);

			Debug.LogFormat ("{0} screen has been loaded.", screenToLoad.ToString ());
		} 
		else 
		{
			Debug.LogErrorFormat ("{0} screen has been previously loaded. Cannot load again!", screenToLoad.ToString());
		}
	}

    public void ToggleScreenActive(UIScreens screenToToggle, bool isActive)
    {
        if (screenToToggle.ToString() != null)
        {
            foreach (string screenName in LoadedScreenNames)
            {
                if(screenName == screenToToggle.ToString())
                {
                    for (int i = 0; i < LoadedScreenNames.Count; i++)
                    {
                        if (LoadedScreenNames[i] == screenToToggle.ToString())
                        {
                            LoadedScreens[i].gameObject.SetActive(isActive);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Hides or shows the screen UI element that matches the name string passed through.
    /// </summary>
    /// <param name="nameOfScreenToHide"></param>
    /// <param name="hide"></param>
    public void ToggleScreen(string nameOfScreen, bool show)
    {
        if (nameOfScreen != null)
        {
            GameObject screen = GameObject.Find(nameOfScreen + CLONE_STR);
            if (screen != null)
            {
                screen.gameObject.SetActive(show);
            }
        }
    }

    /// <summary>
    /// Loads the desired overlay passed through if it hasn't already been previously loaded and then switches to that overlay if switchToOverlayOnLoad is true.
    /// </summary>
    /// <param name="overlayToLoad">Overlay to load.</param>
    /// <param name="switchToOverlayOnLoad">If set to <c>true</c> switch to overlay on load.</param>
    public void LoadOverlay(UIOverlays overlayToLoad, bool switchToOverlayOnLoad = true)
	{
		bool overlayPreviouslyLoaded = false;
		// Check all screen names in LoadedOverlayNames list to make sure this overlay hasn't already been loaded previously.
		if (LoadedOverlayNames.Count > 0) 
		{
			foreach (string overlayName in LoadedOverlayNames) 
			{
				if (overlayName == overlayToLoad.ToString ()) 
				{
					overlayPreviouslyLoaded = true;
				}
			}
		}

		if (!overlayPreviouslyLoaded) 
		{
			GameObject overlay = Instantiate (Resources.Load (UI_OVERLAY_PREFABS_PATH + overlayToLoad.ToString (), typeof(GameObject))) as GameObject;

			if (overlay != null) 
			{
				RectTransform overlayRect = overlay.GetComponent<RectTransform> ();

				if (overlayRect != null) 
				{
					overlayRect.transform.SetParent (_UIOverlayLayerRect.transform, true);
					overlayRect.transform.localPosition = Vector3.zero;
					overlayRect.transform.localScale = Vector3.one;
				}
			}

			// Toggle the loaded overlay either active or inactive.
			overlay.gameObject.SetActive (switchToOverlayOnLoad);

			LoadedOverlayNames.Add (overlayToLoad.ToString ());

			Debug.LogFormat ("{0} overlay has been loaded.", overlayToLoad.ToString ());
		}
		else 
		{
			Debug.LogErrorFormat ("{0} overlay has been previously loaded. Cannot load again!", overlayToLoad.ToString());
		}
	}

    /// <summary>
    /// Hides or shows the overlay UI element that has a matching name to the name string passed through.
    /// </summary>
    /// <param name="nameOfOverlayToToggle"></param>
    /// <param name="hide"></param>
    public void ToggleOverlay(string nameOfOverlayToToggle, bool show)
    {
        if (nameOfOverlayToToggle != null)
        {
            GameObject overlayToHide = GameObject.Find(nameOfOverlayToToggle + CLONE_STR);
            if (overlayToHide != null)
            {
                overlayToHide.gameObject.SetActive(show);
            }
        }
    }

	/// <summary>
	/// Loads a unique popup (a non-GenericPopup popup) and makes it a child of the _UIPopupLayer layer.
	/// </summary>
	/// <param name="popupToLoad">Popup to load.</param>
	public void LoadAndShowUniquePopup(UIPopups popupToLoad)
	{
		GameObject popup = Instantiate (Resources.Load (UI_POPUP_PREFABS_PATH + popupToLoad.ToString (), typeof(GameObject))) as GameObject;

		if (popup != null) {
			RectTransform popupRect = popup.GetComponent<RectTransform> ();

			if (popupRect != null) {
				popupRect.transform.SetParent (_UIPopupLayerRect.transform, true);
				popupRect.transform.localPosition = Vector3.zero;
				popupRect.transform.localScale = Vector3.one;
			}
		}

		LoadedPopupNames.Add (popupToLoad.ToString ());

		Debug.LogFormat ("{0} popup has been loaded and is showing.", popupToLoad.ToString ());
	}

	/// <summary>
	/// Opens the generic popup, with the SetData values passed through for it.
	/// </summary>
	public void OpenGenericPopup()
	{
		// TODO Implement the generic popup script, corresponding prefab, then this.
	}

    // Finds the last popup that has been opened and closes it.
    public void RemoveLastPopupListed()
    {
        if (LoadedPopupNames.Count > 0)
        {
            GameObject popupToClose = GameObject.Find(LoadedPopupNames[LoadedPopupNames.Count - 1] + "(Clone)");
            if (popupToClose != null)
            {
                // Destroy the popup.
                Destroy(popupToClose);
                // Remove the popups name from the active loaded list of popups we have.
                Debug.LogFormat("UIManager.CloseLastPopupListed - Closing the {0} popup.", LoadedPopupNames[LoadedPopupNames.Count - 1].ToString());
                LoadedPopupNames.Remove(LoadedPopupNames[LoadedPopupNames.Count - 1]);
            }
            else
            {
                Debug.LogWarning("UIManager.CloseLastPopupListed - Couldn't find a popup GameObject that matched the LoadedPopupNames list final popup name.");
            }
        }
        else
        {
            Debug.Log("UIManager.CloseLastPopupListed - No popups listed to be closed.");
        }
    }

	public void LoadStartupScreen()
	{
		// Instantiate startup scene prefab as parent of UIScreenLayer.
		LoadScreen (UIScreens.MainMenuScreen, true);
	}

	public void LoadFrontEndHudOverlay()
	{
		// Insantiate FrontEndHud overlay as parent of UIOverlayLayer.
		LoadOverlay (UIOverlays.FrontEndHud, true);
	}

    /// <summary>
    /// Iterates through all loaded screens and overlays and sets FrontEnd ones to inactive or active. May need to be extended to popups.
    /// </summary>
    public void ToggleFrontendUI(bool show)
    {
        foreach (string screenName in LoadedScreenNames)
        {
            if (screenName == "MainMenuScreen")
            {
                ToggleScreen("MainMenuScreen", show);
            }
        }

        foreach (string overlayName in LoadedOverlayNames)
        {
            if (overlayName == "FrontEndHud")
            {
                ToggleOverlay("FrontEndHud", show);
            }
        }
    }
}
