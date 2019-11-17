using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// A standard implementation of IDamageReceiver
/// Issues knockback and manages health
public class Health : MonoBehaviour, IDamageReceiver
{
    public delegate void TakeDamageAction();
    public event TakeDamageAction OnTakeDamage;

    public delegate void HealthChangeAction(int oldVal, int newVal);
    public event HealthChangeAction OnHealthChange;

    /// Returns true if it handles destroying the GameObject
    public delegate bool DeathAction();
    public DeathAction OnDeath;

    [SerializeField]
    private float lightKnockback = 5;
    [SerializeField]
    private float heavyKnockback = 7;
    [SerializeField]
    protected int maxHealth = 3;
    [SerializeField]
    private float invulnerableDuration = 1;

    public int MaxHealth { get { return maxHealth; }}

    private int _amount;
    protected int Amount
    {
        get { return _amount; }
        set {
            int oldVal = _amount;
            _amount = value;
            OnHealthChange?.Invoke(oldVal, _amount);

            if (_amount == 0) {
                OnDeath?.Invoke();
            }
        }
    }

    private Rigidbody rb;

    private float invulnerableTimer;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        Amount = maxHealth;
    }

    protected virtual void Update()
    {
        if (invulnerableTimer > 0)
            invulnerableTimer -= Time.deltaTime;
    }

    public void TakeDamage(Damage damage)
    {
        if (invulnerableTimer <= 0) {
            invulnerableTimer = invulnerableDuration;

            Amount -= damage.Amount;

            switch (damage.ImpactType) {
                case ImpactType.LIGHT:
                {
                    Vector3 force = damage.Direction * lightKnockback + Vector3.up * 10;
                    rb.AddForce(force, ForceMode.Impulse);
                    break;
                }
                case ImpactType.HEAVY:
                {
                    Vector3 force = damage.Direction * heavyKnockback + Vector3.up * 10;
                    rb.AddForce(force, ForceMode.Impulse);
                    break;
                }
            }

            OnTakeDamage?.Invoke();

            if (Amount <= 0) {
                if (OnDeath == null || !OnDeath()) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
