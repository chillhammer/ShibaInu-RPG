using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageReceiver, IHealthReceiver
{
    public delegate void DeathAction();
    public event DeathAction OnDeath;

    public delegate void HealthChangeAction(int oldVal, int newVal);
    public event HealthChangeAction OnHealthChange;

    public PlayerPhysicsControl physics;

    [SerializeField]
    private int maxHealth = 8;
    public int MaxHealth { get { return maxHealth; }}

    private int _health;
    private int Health
    {
        get { return _health; }
        set {
            int oldVal = _health;
            _health = value;
            OnHealthChange?.Invoke(oldVal, _health);

            if (Health == 0) {
                OnDeath?.Invoke();
            }
        }
    }

    void Start()
    {
        Health = maxHealth;
    }

    public void TakeDamage(Damage damage)
    {
        // Health Reduced
        Health = Math.Max(0, Health - damage.Amount);

        // Knockback
        Vector3 dir = damage.Direction;
        switch (damage.ImpactType) {
            case ImpactType.LIGHT:
                dir = dir * 10.0f + Vector3.up * 10.0f;
                physics.rb.AddForce(dir, ForceMode.Impulse);
                break;
        }
    }

    public void TakeHeal(Heal heal)
    {
        Health = Math.Max(Health + heal.Amount, maxHealth);
    }
}
