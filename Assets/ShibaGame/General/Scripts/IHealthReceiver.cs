using UnityEngine;

public struct Heal
{
    public Heal(int amount)
    {
        Amount = amount;
    }
    public int Amount { get; private set; }
}

public interface IHealthReceiver
{
    void TakeHeal(Heal heal);
}
