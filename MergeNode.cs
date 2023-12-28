using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MergeNode : MonoBehaviour
{
	public MergeCat MergeCat { get; set; }
	
	public void RepositionMergeCat() 
	{
		if(MergeCat != null) 
		{
			MergeCat.transform.position = transform.position;
		}
	}
	
	
}
