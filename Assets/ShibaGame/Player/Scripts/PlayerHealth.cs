using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageReceiver
{
    public float health = 100.0f;

    public delegate void OnDeath();
    public OnDeath deathEvent;

    public PlayerPhysicsControl physics;

    public void TakeDamage(Damage damage)
    {
        // Health Reduced
        health = Mathf.Max(0.0f, health - damage.amount);

        // Knockback
        Vector3 dir = damage.direction;
        switch (damage.impactType)
        {
            case ImpactType.LIGHT:
                dir = dir * 100.0f + Vector3.up * 10.0f;
                physics.rb.AddForce(dir, ForceMode.Impulse);
                break;
        }
        if (health == 0.0f)
        {
            deathEvent.Invoke();
        }
    }
}
