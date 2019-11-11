using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTree : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip sound;

    private SoundModulator sm;
    private bool flag = true;

    void Start()
    {
        sm = GetComponent<SoundModulator>();
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody && flag)
        {
            anim.SetBool("FallenTree", true);
            sm.PlayModClip(sound);
            flag = false;
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
