using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : MonoBehaviour
{
	[SerializeField]
	Button spawnCatHeroButton;
	
	[SerializeField]
	TextMeshProUGUI energyText;
	
	private void Start() {
		spawnCatHeroButton.onClick.AddListener(OnClickSpawnCatHeroButton);
		
		Refresh();
	}
	
	public void Refresh() 
	{
		energyText.text = GameManager.Instance.Energy.ToString();
	}
	
	public void OnClickSpawnCatHeroButton() 
	{
		SoundManager.Instance.PlayButton();
		if(GameManager.Instance.Energy >= 1) 
		{
			GameManager.Instance.useEnergy(1);
			GameManager.Instance.SpawnMergeCat();
		}
	}
}
