using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{	
	public int Energy { get; set; }
	
	[SerializeField]
	List<CatHeroInfoSO> allCatHeros = new List<CatHeroInfoSO>();

	public HashSet<MergeCat> MergeCats { get; private set; } = new HashSet<MergeCat>();
	public HashSet<HeroCat> HeroCats { get; private set; } = new HashSet<HeroCat>();
	public HashSet<Enemy> Enemies { get; private set; } = new HashSet<Enemy>();
	
	[SerializeField]
	public List<Transform> RouteTransforms = new List<Transform>();
	
	public GameObject enemyPrefab;
	
	
	[SerializeField]
	public Transform mergeCatGroupTransform;
	public Transform mergeNodeGroupTransform;
	
	public Transform heroCatGroupTransform;
	
	MergeNode[] MergeNodes;
	HeroNode[] HeroNodes;
	
	[SerializeField]
	UI_GameScene ui_gameScene;
	
	[SerializeField]
	public Button addButton;
	
	public GameObject destroyGO;
	
	// Start is called before the first frame update
	void Start()
	{
		Energy = 15;
		
		MergeNodes = mergeNodeGroupTransform.GetComponentsInChildren<MergeNode>();
		HeroNodes = mergeNodeGroupTransform.GetComponentsInChildren<HeroNode>();
		
		StartCoroutine(CoStartSpawnEnemy());
		ui_gameScene.Refresh();
	}
	
	public void DestroyMergeCat(MergeCat mergeCat)
	{
		foreach (var mergeNode in MergeNodes) 
		{
			if(mergeNode.MergeCat == mergeCat) 
			{
				mergeNode.MergeCat = null;
			}	
		}
		
		mergeCat.gameObject.SetActive(false);
		MergeCats.Remove(mergeCat);
		Destroy(mergeCat);
	}
	
	public void DestroyHeroCat(HeroCat heroCat)
	{
		foreach (var heroNode in HeroNodes) 
		{
			if(heroNode.HeroCat == heroCat) 
			{
				heroNode.HeroCat = null;
			}	
		}
		
		heroCat.gameObject.SetActive(false);
		HeroCats.Remove(heroCat);
		Destroy(heroCat);
	}
	
	public void DestroyEnemy(Enemy enemy)
	{		
		enemy.gameObject.SetActive(false);
		Enemies.Remove(enemy);
		Destroy(enemy);
	}
	
	public void SpawnMergeCat()
	{
		foreach (var mergeNode in MergeNodes) 
		{
			if(mergeNode.MergeCat == null) 
			{
				int randomIndex = Random.Range(0, allCatHeros.Count);
				var randomHero = allCatHeros[randomIndex];
				GameObject go = Object.Instantiate(randomHero.prefab, mergeCatGroupTransform);
				go.transform.position = mergeNode.transform.position;
				var mergeCat = Util.GetOrAddComponent<MergeCat>(go);
				go.name = randomHero.prefab.name;
				
				mergeNode.MergeCat = mergeCat;
				MergeCats.Add(mergeCat);
				
				break;
			}	
		}		
	}
	
	public void ToHeroCat(HeroNode heroNode, MergeCat mergeCat)
	{
		GameObject go = Object.Instantiate(mergeCat.CatHeroInfo.heroPrefab, heroCatGroupTransform);
		go.transform.position = heroNode.transform.position;
		var heroCat = Util.GetOrAddComponent<HeroCat>(go);
		heroCat.HeroLevel = mergeCat.MergeLevel;
		go.name = mergeCat.CatHeroInfo.heroPrefab.name;
		
		heroNode.HeroCat = heroCat;
		HeroCats.Add(heroCat);
		heroCat.Refresh();
		
		DestroyMergeCat(mergeCat);
	}
	
	public void ToMergeCat(MergeNode mergeNode, HeroCat heroCat)
	{
		GameObject go = Object.Instantiate(heroCat.CatHeroInfo.prefab, mergeCatGroupTransform);
		go.transform.position = mergeNode.transform.position;
		var mergeCat = Util.GetOrAddComponent<MergeCat>(go);
		mergeCat.MergeLevel = heroCat.HeroLevel;
		go.name = mergeCat.CatHeroInfo.prefab.name;
		
		mergeNode.MergeCat = mergeCat;
		MergeCats.Add(mergeCat);
		mergeCat.Refresh();
		
		DestroyHeroCat(heroCat);
	}
	
	public void AddEnergy(int energy)
	{
		this.Energy += energy;
		ui_gameScene.Refresh();
	}
	
	public void useEnergy(int energy)
	{
		this.Energy -= energy;
		ui_gameScene.Refresh();
	}

	IEnumerator CoStartSpawnEnemy() 
	{
		while(true) 
		{
			int wave = 1;
			int count = 0;
			while(count < 5) 
			{
				yield return new WaitForSeconds(Random.Range(1, 2));
				var enemyGO = Instantiate(enemyPrefab);
				enemyGO.transform.position = RouteTransforms.First().position;
				var enemy = Util.GetOrAddComponent<Enemy>(enemyGO);
				enemy.hp = 8 + (wave * 2);
				Enemies.Add(enemy);
				count++;
			}
			
			if(wave < 20) 
			{
				wave++;
			}
			
			yield return new WaitForSeconds(17.0f);
		}
	}
	
	
}
