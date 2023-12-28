using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroNode : MonoBehaviour
{
	public HeroCat HeroCat { get; set; }
	
	public void RepositionMergeCat() 
	{
		if(HeroCat != null) 
		{
			HeroCat.transform.position = transform.position;
		}
	}
}
