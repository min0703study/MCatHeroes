using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	Rigidbody2D _rigidbody2D;
	
	Vector3 moveDir = Vector3.zero;
	
	Transform target;
	
	float MoveSpeed = 10f;
	
	int Power = 1;
	
	// Start is called before the first frame update
	void Start()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		var moveDir = (target.transform.position - transform.position).normalized;
		Vector3 addedDir = moveDir * MoveSpeed * Time.deltaTime;
		transform.position += addedDir;
	}
	
	public void SetInfo(Vector3 moveDir, Transform target, int power) 
	{
		this.moveDir = moveDir;
		this.target = target;
		this.Power = power;
	}

	private void OnTriggerEnter2D(Collider2D collisionData)
	{
		if (collisionData.gameObject.tag == "Enemy")
		{
			collisionData.gameObject.GetComponent<Enemy>().Attack(Power);
			Destroy(gameObject);
		}
	}
}
