using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksHit : MonoBehaviour
{
    public int rockDamage = 1;
    public Rigidbody rb;
    public float minSpeed = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.rigidbody && rb.velocity.magnitude>minSpeed)
        {
            IDamageReceiver dr = c.gameObject.GetComponent<IDamageReceiver>();
            if (dr != null)
            {
                Damage damage = new Damage(ImpactType.LIGHT, rockDamage, (c.transform.position - transform.position).normalized);
                dr.TakeDamage(damage);
            }
        }
    }
}
