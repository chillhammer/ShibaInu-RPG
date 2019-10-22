using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyController : MonoBehaviour
{
	public int attackDamage = 1;

	[SerializeField]
    private AudioClip pierceSound;

    private SoundModulator sm;

    void Start()
    {
    	sm = GetComponent<SoundModulator>();
    }

    void OnTriggerEnter(Collider c) 
    {
    	IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
    	if (dr != null)
    	{
    		Damage damage = new Damage(ImpactType.LIGHT, attackDamage, (c.transform.position - transform.position).normalized);
    		dr.TakeDamage(damage);
    		sm.PlayModClip(pierceSound);
    	}
    }
}
