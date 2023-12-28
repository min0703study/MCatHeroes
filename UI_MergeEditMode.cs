using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.U2D.IK;

public class UI_MergeEditMode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private bool onDraging;
	
	private bool isActivated;
	private bool isActivatedHeroNode;
	MergeNode activatedMergeNode = null;
	HeroNode activatedHeroNode = null;
	private Vector3 touchOffset;

	// Start is called before the first frame update
	void Start()
	{
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		onDraging = true;
	}

	private Bounds GetIntersectionBounds(Bounds boundsA, Bounds boundsB)
	{
		float minX = Mathf.Max(boundsA.min.x, boundsB.min.x);
		float minY = Mathf.Max(boundsA.min.y, boundsB.min.y);
		float minZ = Mathf.Max(boundsA.min.z, boundsB.min.z);
		float maxX = Mathf.Min(boundsA.max.x, boundsB.max.x);
		float maxY = Mathf.Min(boundsA.max.y, boundsB.max.y);
		float maxZ = Mathf.Min(boundsA.max.z, boundsB.max.z);

		Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);
		Vector3 size = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);

		return new Bounds(center, size);
	}

	// 2. 드래그, 가운데 원이 따라와야 한다. 바깥원까지 
	public void OnDrag(PointerEventData eventData)
	{
		if (isActivated && activatedMergeNode != null)
		{
			// 클릭 된 위치를 스크린 좌표에서 월드 좌표로 변환합니다.
			Vector3 dragWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
			// 월드 좌표를 타일맵 좌표로 변환합니다.
			// Vector3Int tilePos = MapManager.Instance.Map.WorldToCell(clickWorldPos);
			activatedMergeNode.MergeCat.transform.position = dragWorldPos + touchOffset;
		}

		if (isActivatedHeroNode && activatedHeroNode != null)
		{
			// 클릭 된 위치를 스크린 좌표에서 월드 좌표로 변환합니다.
			Vector3 dragWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
			// 월드 좌표를 타일맵 좌표로 변환합니다.
			// Vector3Int tilePos = MapManager.Instance.Map.WorldToCell(clickWorldPos);
			activatedHeroNode.HeroCat.transform.position = dragWorldPos + touchOffset;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);

		RaycastHit2D[] hitAll = Physics2D.RaycastAll(clickWorldPos, Vector2.zero);

		foreach (var hit in hitAll)
		{
			if (hit.collider != null)
			{
				if (hit.collider.gameObject.tag == "MergeNode")
				{
					var mergeNode = hit.collider.gameObject.GetComponent<MergeNode>();

					if (isActivatedHeroNode)
					{

					}
					else
					{
						if (mergeNode.MergeCat != null)
						{
							this.activatedMergeNode = mergeNode;
							isActivated = true;
							touchOffset = (Vector3)mergeNode.MergeCat.transform.position - clickWorldPos;
							
							GameManager.Instance.addButton.gameObject.SetActive(false);
							GameManager.Instance.destroyGO.gameObject.SetActive(true);
						}
					}
				}

				if (hit.collider.gameObject.tag == "HeroNode")
				{
					var heroNode = hit.collider.gameObject.GetComponent<HeroNode>();

					if (heroNode.HeroCat != null)
					{
						this.activatedHeroNode = heroNode;
						isActivatedHeroNode = true;
						touchOffset = (Vector3)heroNode.HeroCat.transform.position - clickWorldPos;
											
						GameManager.Instance.addButton.gameObject.SetActive(false);
						GameManager.Instance.destroyGO.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		onDraging = false;

		Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
		RaycastHit2D[] hitAll = Physics2D.RaycastAll(clickWorldPos, Vector2.zero);

		bool isWorked = false;
		bool isAnythingCollider = false;

		foreach (var hit in hitAll)
		{
			if (hit.collider != null)
			{
				if (hit.collider.gameObject.tag == "MergeNode")
				{
					var mergeNode = hit.collider.gameObject.GetComponent<MergeNode>();

					if (isActivatedHeroNode)
					{
						if (mergeNode.MergeCat == null)
						{
							GameManager.Instance.ToMergeCat(mergeNode, activatedHeroNode.HeroCat);
							SoundManager.Instance.PlayButton();
							isWorked = true;
						}
						else
						{
							GameManager.Instance.ToMergeCat(mergeNode, activatedHeroNode.HeroCat);
							mergeNode.RepositionMergeCat();
							GameManager.Instance.ToHeroCat(activatedHeroNode, mergeNode.MergeCat);
							activatedHeroNode.RepositionMergeCat();
							SoundManager.Instance.PlaySetSlot();
							isWorked = true;
						}

					}
					else
					{
						if (mergeNode.MergeCat == null)
						{
							isWorked = true;
							mergeNode.MergeCat = activatedMergeNode.MergeCat;
							activatedMergeNode.MergeCat = null;
							mergeNode.RepositionMergeCat();
							SoundManager.Instance.PlayButton();

						}
						else if (mergeNode == activatedMergeNode)
						{
						}
						else
						{
							isWorked = true;
							bool result = mergeNode.MergeCat.TryMerge(activatedMergeNode.MergeCat);
							if (result == false)
							{
								var swap = mergeNode.MergeCat;
								mergeNode.MergeCat = activatedMergeNode.MergeCat;
								mergeNode.RepositionMergeCat();
								activatedMergeNode.MergeCat = swap;
								activatedMergeNode.RepositionMergeCat();
								SoundManager.Instance.PlayButton();
								//activatedMergeNode.RepositionMergeCat();
							}
							else
							{
								activatedMergeNode = null;
							}
						}
					}
				}
				else if (hit.collider.gameObject.tag == "HeroNode")
				{
					var heroNode = hit.collider.gameObject.GetComponent<HeroNode>();


					if (isActivatedHeroNode)
					{
						var swap = heroNode.HeroCat;
						heroNode.HeroCat = activatedHeroNode.HeroCat;
						heroNode.RepositionMergeCat();
						activatedHeroNode.HeroCat = swap;
						activatedHeroNode.RepositionMergeCat();
						SoundManager.Instance.PlaySetSlot();
					}
					else
					{
						if (heroNode.HeroCat == null)
						{
							GameManager.Instance.ToHeroCat(heroNode, activatedMergeNode.MergeCat);
							SoundManager.Instance.PlaySetSlot();
							isWorked = true;
						}
						else if (heroNode == activatedHeroNode)
						{ }
						else
						{
							var a = activatedMergeNode.MergeCat;
							GameManager.Instance.ToMergeCat(activatedMergeNode, heroNode.HeroCat);
							activatedMergeNode.RepositionMergeCat();
							GameManager.Instance.ToHeroCat(heroNode, a);
							// heroNode.RepositionMergeCat();
							SoundManager.Instance.PlaySetSlot();
							isWorked = true;
						}
					}
				} else if(hit.collider.gameObject.tag == "Destroy") 
				{
					SoundManager.Instance.PlayDestroy();
					if(isActivated) 
					{
						GameManager.Instance.AddEnergy(activatedMergeNode.MergeCat.MergeLevel);
						GameManager.Instance.DestroyMergeCat(activatedMergeNode.MergeCat);
					} else if(isActivatedHeroNode) 
					{
						GameManager.Instance.AddEnergy(activatedHeroNode.HeroCat.HeroLevel);
						GameManager.Instance.DestroyHeroCat(activatedHeroNode.HeroCat);
					}
				}
			}
		}

		if (isWorked == false)
		{
			if (activatedMergeNode)
			{
				activatedMergeNode.RepositionMergeCat();
			}
			else
			{
				activatedHeroNode.RepositionMergeCat();
			}
		}

		activatedMergeNode = null;
		activatedHeroNode = null;
		isActivatedHeroNode = false;
		isActivated = false;
		
		GameManager.Instance.addButton.gameObject.SetActive(true);
		GameManager.Instance.destroyGO.gameObject.SetActive(false);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if(onDraging == false) 
		{
			GameManager.Instance.addButton.gameObject.SetActive(true);
			GameManager.Instance.destroyGO.gameObject.SetActive(false);
		}

	}
}
