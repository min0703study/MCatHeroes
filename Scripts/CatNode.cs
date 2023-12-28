using System;
using UnityEngine;

public class CatNode : MonoBehaviour
{
	public static Action<CatNode> OnNodeSelected;
	public static Action OnTurretSold;

	[SerializeField] private GameObject attackRangeSprite;
	
	public HeroCat heroCat { get; set; }

	private float _rangeSize;
	private Vector3 _rangeOriginalSize;

	private void Start()
	{
		_rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
		_rangeOriginalSize = attackRangeSprite.transform.localScale;
	}

	// public void SetCatHero(CatHero catHero)
	// {
	// 	this.catHero = catHero;
	// }

	// public bool IsEmpty()
	// {
	// 	return catHero == null;
	// }

	public void CloseAttackRangeSprite()
	{
		attackRangeSprite.SetActive(false);
	}
	
	// public void SelectTurret()
	// {
	//     OnNodeSelected?.Invoke(this);
	//     if (!IsEmpty())
	//     {
	//         ShowTurretInfo();
	//     }
	// }

	// public void SellTurret()
	// {
	//     if (!IsEmpty())
	//     {
	//         CurrencySystem.Instance.AddCoins(Turret.TurretUpgrade.GetSellValue());
	//         Destroy(Turret.gameObject);
	//         Turret = null;
	//         attackRangeSprite.SetActive(false);
	//         OnTurretSold?.Invoke();
	//     }
	// }

	// private void ShowTurretInfo()
	// {
	//     attackRangeSprite.SetActive(true);
	//     attackRangeSprite.transform.localScale = _rangeOriginalSize * Turret.AttackRange / (_rangeSize / 2);
	// }
}
