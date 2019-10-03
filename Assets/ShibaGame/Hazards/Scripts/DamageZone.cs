using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to hurt entities with IDamageReceiver if they overlap
public class DamageZone : MonoBehaviour
{
    public ImpactType impactType = ImpactType.LIGHT;
    public int defaultDamage = 10;

    private void OnTriggerEnter(Collider collision)
    {
        IDamageReceiver receiver = collision.gameObject.GetComponent<IDamageReceiver>();
        if (receiver != null)
        {
            Vector3 dir = collision.gameObject.transform.position - transform.position;
            Damage damage = new Damage();
            damage.impactType = impactType;
            damage.amount = defaultDamage;
            damage.direction = dir;

            receiver.TakeDamage(damage);
        }
    }
}
