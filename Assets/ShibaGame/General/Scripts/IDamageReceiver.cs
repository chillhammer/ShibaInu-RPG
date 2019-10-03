using UnityEngine;

public enum ImpactType
{
    NONE,
    LIGHT,
    HEAVY,
}

public struct Damage
{
    public ImpactType impactType;
    public int amount;
    public Vector3 direction;
}

public interface IDamageReceiver
{
    void TakeDamage(Damage damage);
}
