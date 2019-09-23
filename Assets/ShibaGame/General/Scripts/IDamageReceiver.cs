using UnityEngine;

public enum ImpactType
{
    NONE,
    LIGHT,
    HEAVY,
}

public struct Damage
{
    ImpactType impactType;
    int amount;
    Vector3 direction;
}

public interface IDamageReceiver
{
    void TakeDamage(Damage damage);
}
