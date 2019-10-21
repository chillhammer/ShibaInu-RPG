using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
	public int count = 0; // replace with health value
	public bool hasItem = false;
	public bool isClose = false;

	public void ApproachItem()
	{
		isClose = true;
	}

	//TODO: Change this to increment player health instead of count variable
	public void GetItem()
	{
		hasItem = true;
		count += 1;
	}
}
