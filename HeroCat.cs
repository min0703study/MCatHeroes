using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCat : MonoBehaviour
{
	[SerializeField]
	public CatHeroInfoSO CatHeroInfo;

	public float cooldownTime = 1.0f; // 발사 쿨타임

	private float lastShootTime; // 마지막으로 발사한 시간
	
	public int HeroLevel; 
	
	public int Power => HeroLevel * 2; 
	
	[SerializeField]
	TextMesh heroLevelText;
	
	// Start is called before the first frame update
	void Start()
	{
		lastShootTime = Time.time; 
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void Refresh() 
	{
		heroLevelText.text = HeroLevel.ToString();
	}
	
	private void OnTriggerStay2D(Collider2D collided) {
		if(collided.tag == "Enemy") 
		{
 			if (Time.time - lastShootTime >= cooldownTime) {
				Vector2 shootDirection = ( collided.transform.position - transform.position).normalized;

				Quaternion QSpreadAngle = Quaternion.LookRotation(shootDirection);
			// 총알 프리팹을 복제하여 발사
				GameObject bulletGO = Instantiate(CatHeroInfo.bulletPrefab, transform.position, Quaternion.identity);
				var bullet = Util.GetOrAddComponent<Bullet>(bulletGO);
				bullet.SetInfo(shootDirection, collided.gameObject.transform, Power);
				
				SoundManager.Instance.PlayAttackSound();
				
			
				lastShootTime = Time.time; // 발사 시간 갱신
			}
		}
	}
	
}
