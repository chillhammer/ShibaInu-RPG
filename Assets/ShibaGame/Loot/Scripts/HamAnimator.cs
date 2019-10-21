using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamAnimator : MonoBehaviour
{

	private Animation anim;

	public float speed = 1f;
	public bool isActivated = false;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animation>();

        if (anim == null)
            Debug.Log("Animation could not be found");

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider c) 
    {
    	if (c.attachedRigidbody != null) {
			
			ItemCollector hc = c.attachedRigidbody.gameObject.GetComponent<ItemCollector>();
			
			if (hc != null) {
				anim.Play("HamDance");
				hc.isClose = true;
            }
        }

    }


    void OnTriggerExit(Collider c) 
    {
    	if (c.attachedRigidbody != null) {
			
			ItemCollector hc = c.attachedRigidbody.gameObject.GetComponent<ItemCollector>();
			
			if (hc != null) {
				anim.Stop();
				hc.isClose = false;
			}
        }

    }
}

