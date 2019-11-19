using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health, IHealthReceiver
{
    public PlayerPhysicsControl physics;

    [SerializeField]
    private string titleScreenScene = null;

    override protected void Start()
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
        SceneManager.LoadScene(titleScreenScene);
        return true;
    }
}
