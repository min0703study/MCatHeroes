using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	public static T Instance { get; private set; }

	protected virtual void Awake()
	{	
		if (Instance == null)
		{
			Instance = this as T;
		}
		else
		{
			//Destroy(this);
		}
	}
	
}
