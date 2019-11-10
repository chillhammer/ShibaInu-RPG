using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTreeDamage : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public int treeDamage = 10;
    [SerializeField] private AudioClip pierceSound;

    private SoundModulator sm;

    void Start()
    {
        sm = GetComponent<SoundModulator>();
    }

    void OnTriggerEnter(Collider c)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fallen Tree"))
        {
            IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
            if (dr != null)
            {
                Damage damage = new Damage(ImpactType.LIGHT, treeDamage, (c.transform.position - transform.position).normalized);
                dr.TakeDamage(damage);
                sm.PlayModClip(pierceSound);
            }
        }
    }

}
