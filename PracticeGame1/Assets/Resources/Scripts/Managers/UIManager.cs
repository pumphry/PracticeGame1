using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviourSingleton<UIManager> 
{
	
	// Enum group of UI Screen names that get translated to strings and are used to load the screen prefabs for each screen.
	public enum UIScreens { MainMenuScreen }; 

	// Enum group of (unique, non-generic) UI Popup names that get translated to strings and are used to load the popup prefabs for each popup.
	public enum UIPopups { TestPopup }; 

	public List<string> LoadedScreenNames = new List<string> ();
	public List<string> LoadedPopupNames = new List<string> ();

	public const string UI_SCREEN_PREFABS_PATH = "Prefabs/UI/Screens/";
	public const string UI_POPUP_PREFABS_PATH = "Prefabs/UI/Popups/";

	private const string UI_ROOT_NAME = "UIRoot";
	private const string UI_SCREEN_LAYER_NAME = "UIScreenLayer";
	private const string UI_POPUP_LAYER_NAME = "UIPopupLayer";

	// Event setup that will call once all the of the UI framework is done building.
	public delegate void UIFrameworkFinishedBuilding();
	public static event UIFrameworkFinishedBuilding OnUIFrameworkFinishedBuilding;

	private GameObject _UIRoot;
	private GameObject _UIScreenLayer;
	private RectTransform _UIScreenLayerRect;
	private GameObject _UIPopupLayer;
	private RectTransform _UIPopupLayerRect;

	private const float LOAD_INTO_START_SCREEN_DELAY = 1.0f;

	// Use this for initialization
	void Start () 
	{
		Debug.Log ("UIManager is active.");

		ClearUIManagerVariables ();

		CreateUIRoot ();

		// TODO remove this when done testing.
		StartCoroutine (OpenTestPopup ());
	}

	void ClearUIManagerVariables()
	{
		_UIRoot = null;
		_UIScreenLayer = null;
		_UIPopupLayer = null;

		LoadedScreenNames.Clear ();
		LoadedPopupNames.Clear ();
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

			// Create the UIPopupLayer GameObject second so it is second in the Heirarchy and will draw 
			//UI elements OVER the UIScreenLayer when they are parented to it.
			GameObject _UIPopupLayer = new GameObject ();
			_UIPopupLayer.name = UI_POPUP_LAYER_NAME;
			_UIPopupLayer.transform.parent = UIRoot.transform;
			_UIPopupLayer.transform.localPosition = Vector3.zero;
			_UIPopupLayer.transform.localScale = Vector3.one;
			_UIPopupLayerRect = _UIPopupLayer.AddComponent<RectTransform> ();

			// The three UI docking components (UIRoot, UIScreenLayer, and UIPopupLayer) are finished building.
			Debug.Log ("UIScreenLayer and UIPopupLayer have been created.");
			OnUIFrameworkFinishedBuilding ();
		}
	}

	/// <summary>
	/// Loads the desired screen if not already loaded and then switches to that scree in UI view if switchToScreenOnLoad is true.
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
		if (!screenPreviouslyLoaded) 
		{
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

			Debug.LogFormat ("{0} screen has been loaded.", screenToLoad.ToString ());
		}
	}

	/// <summary>
	/// Loads a unique popup (a non GenericPopup popup) and makes it a child of the _UIPopupLayer layer.
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

	public void TransitionToStartupScreen()
	{
		// Instantiate startup scene prefab as parent of UIScreenLayer.
		LoadScreen (UIScreens.MainMenuScreen, true);
	}

	public IEnumerator OpenTestPopup()
	{
		yield return new WaitForSeconds (2.0f);

		LoadAndShowUniquePopup (UIPopups.TestPopup);
	}
}
