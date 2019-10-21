using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField]
    private int attackDamage = 1;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
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
}