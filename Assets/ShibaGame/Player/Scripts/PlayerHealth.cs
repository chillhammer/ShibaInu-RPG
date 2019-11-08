using System;
using UnityEngine;

public class PlayerHealth : Health, IHealthReceiver
{
    public PlayerPhysicsControl physics;

    new void Start()
    {
        base.Start();
        OnDeath = RealOnDeath;
    }

    public void TakeHeal(Heal heal)
    {
        Amount = Math.Max(Amount + heal.Amount, maxHealth);
    }

    private bool RealOnDeath()
    {
        return true;
    }
}
