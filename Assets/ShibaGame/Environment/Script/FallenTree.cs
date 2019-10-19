using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTree : MonoBehaviour
{
    [SerializeField] private Animator anim;


    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody)
        {
            anim.SetBool("FallenTree", true);

        }
    }
    void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody)
        {
            anim.SetBool("FallenTree", false);

        }
    }


}
