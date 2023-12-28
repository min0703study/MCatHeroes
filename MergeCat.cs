using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeCat : MonoBehaviour
{
	public CatHeroInfoSO CatHeroInfo;

	[SerializeField]	
	TextMesh mergeLevelText;
		
	[SerializeField]
	public BoxCollider2D boxCollider2D;
	
	public int MergeLevel { get; set; } = 1;
	
	GameObject emptyGO, activeGO;

	// Start is called before the first frame update
	void Start()
	{
		Refresh();
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void Refresh() 
	{
		mergeLevelText.text = MergeLevel.ToString();
	}
	
	public bool TryMerge(MergeCat activatedNode) 
	{
		if(activatedNode.CatHeroInfo.CatType == CatHeroInfo.CatType&& activatedNode.MergeLevel == MergeLevel) 
		{
			SoundManager.Instance.PlayMergeEffect();
			MergeLevel += 1;
			GameManager.Instance.DestroyMergeCat(activatedNode);
			Refresh();
			return true;
		}
		return false;
	}
}
