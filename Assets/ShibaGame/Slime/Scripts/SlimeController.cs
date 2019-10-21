using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IDamageReceiver
{
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    private int health = 2;

    private Animator anim;
    private Rigidbody rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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

    public void TakeDamage(Damage damage)
    {
        health -= damage.Amount;

        Vector3 dir = damage.Direction;
        switch (damage.ImpactType)
        {
            case ImpactType.LIGHT:
                dir = dir * 15.0f + Vector3.up * 15.0f;
                rb.AddForce(dir, ForceMode.Impulse);
                break;
        }

        if (health <= 0)
        {
            //TODO: Add death animation or something
            LootDropper ld = gameObject.GetComponent<LootDropper>();
            Destroy(gameObject);
            ld.DropLoot(transform.position, transform.rotation);
        }
    }
}