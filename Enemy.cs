using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	int currentRouteIndex;
	
	public int hp;
	
	float MoveSpeed = 2f;
	
	// Start is called before the first frame update
	void Start()
	{
		hp = 10;
		currentRouteIndex = 0;
	}


	public void Attack(int damage) 
	{
		hp -= damage;
		if(hp <= 0) 
		{
			GameManager.Instance.AddEnergy(1);
			GameManager.Instance.DestroyEnemy(this);
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if(GameManager.Instance.RouteTransforms.Count -1 < currentRouteIndex) 
		{
			gameObject.SetActive(false);
			GameManager.Instance.DestroyEnemy(this);
		} else 
		{
			Transform toTransform = GameManager.Instance.RouteTransforms[currentRouteIndex];
			var moveDir = (toTransform.position - transform.position).normalized;
			Vector3 addedDir = moveDir * MoveSpeed * Time.deltaTime;
			transform.position += addedDir;
			
			if((toTransform.position - transform.position).magnitude < 0.3f) 
			{
				currentRouteIndex++;
			}
		}
	}
	
}
