using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTreeDamage : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public int treeDamage = 10;

    void OnTriggerEnter(Collider c)
    {
        if (anim.GetBool("FallenTree"))
        {
            IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
            if (dr != null)
            {
                Damage damage = new Damage(ImpactType.LIGHT, treeDamage, (c.transform.position - transform.position).normalized);
                dr.TakeDamage(damage);
            }
        }
    }

}
