using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAnimationController : MonoBehaviour
{
	public Animation anim;
	public float speed = 0.5f;

    void Awake()
    {
    	anim = gameObject.GetComponent<Animation>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
