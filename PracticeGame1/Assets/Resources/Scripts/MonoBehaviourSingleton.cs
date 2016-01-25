using UnityEngine;
using System.Collections;

/// <summary>
/// Base clase for all singleton scripts in the game. Any generic code for those types of scripts will go here.
/// </summary>
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour 
{

	private static T _theInstance = null;

	public static T Instance
	{
		get 
		{
			if (_theInstance == null) 
			{
				return null;
			}

			return _theInstance;
		}
	}

	public static bool IsInstanceValid()
	{
		return(_theInstance != null);
	}

	protected virtual void Awake()
	{
		Debug.Assert(Instance == null, "Can't have 2 singleton intances of type " + typeof(T).Name);

		_theInstance = this as T;

		DontDestroyOnLoad (this);
	}

	protected virtual void OnDestroy()
	{
		_theInstance = null;
	}

}
