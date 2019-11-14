using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private AudioClip sound1;
    [SerializeField]
    private AudioClip sound2;
    [SerializeField]
    private AudioClip sound3;
    [SerializeField]
    private AudioClip sound4;

    private Animator anim;
    private Rigidbody rb;
    private Health health;
    private SoundModulator sm;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        sm = GetComponent<SoundModulator>();

        health.OnDeath = OnDeath;
    }

    private void OnCollisionEnter(Collision coll)
    {
        Collider c = coll.collider;
        IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
        if (dr != null)
        {
            Damage damage = new Damage(ImpactType.LIGHT, attackDamage, (c.transform.position - transform.position).normalized);
            dr.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
        if (dr != null)
        {
            anim.SetBool("SlimeHop", true);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
        if (dr != null)
        {
            anim.SetBool("SlimeHop", false);
        }
    }

    public bool OnDeath()
    {
        //TODO: Add death animation or something
        LootDropper ld = gameObject.GetComponent<LootDropper>();
        ld.DropLoot(transform.position, transform.rotation);
        return false;
    }

    // Animation event audio callbacks
    public void OnJump()
    {
        sm.PlayModClip(sound3);
    }

    public void OnLand()
    {
        sm.PlayModClip(sound1);
    }
}