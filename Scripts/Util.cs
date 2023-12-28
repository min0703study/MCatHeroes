using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class Util
{
	public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
	{
		T component = go.GetComponent<T>();
		if (component == null)
			component = go.AddComponent<T>();
		return component;
	}
	
	public static void DestroyChilds(GameObject go)
	{
		Transform[] children = new Transform[go.transform.childCount];
		for (int i = 0; i < go.transform.childCount; i++)
		{
			children[i] = go.transform.GetChild(i);
		}

		// 모든 자식 오브젝트 삭제
		foreach (Transform child in children)
		{
			child.SetParent(null);
			// ResourceManager.Instance.Destroy(child.gameObject);
		}
	}
	
	public static void GetFirstChild(GameObject go)
	{
		
	}
}