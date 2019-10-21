using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	void OnTriggerEnter(Collider c)
	{
		if (c.attachedRigidbody != null) 
		{
			ItemCollector hc = c.attachedRigidbody.gameObject.GetComponent<ItemCollector>();
			
			if (hc != null) 
			{
				Destroy(this.gameObject);
				Destroy(this.transform.parent.gameObject);
				hc.GetItem();
			}
		}
	}
}
