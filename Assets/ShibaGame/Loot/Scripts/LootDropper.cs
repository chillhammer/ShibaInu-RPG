using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
	public GameObject item;

	public void DropLoot(Vector3 pos, Quaternion rot)
	{	
		Droppable loot = item.GetComponent<Droppable>();
		loot.Drop(pos, rot);
	}

}
