using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private Health[] ninjas = null;

    private int deathCount;

    private Animator anim;
    private AudioSource aud;

    void Start()
    {
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        foreach (Health h in ninjas) {
            h.OnDeathExternal += HandleDeath;
        }
    }

    void HandleDeath()
    {
        ++deathCount;
        if (deathCount == ninjas.Length) {
            anim.Play("Lower");
            aud.Play();
        }
    }
}
