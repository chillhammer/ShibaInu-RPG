using UnityEngine;

public enum ImpactType
{
    NONE,
    LIGHT,
    HEAVY,
}

public struct Damage
{
    public Damage(ImpactType impactType, int amount, Vector3 direction)
    {
        ImpactType = impactType;
        Amount = amount;
        Direction = direction;
    }
    public ImpactType ImpactType { get; private set; }
    public int Amount { get; private set; }
    public Vector3 Direction { get; private set; }
}

public interface IDamageReceiver
{
    void TakeDamage(Damage damage);
}
