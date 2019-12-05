using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health, IHealthReceiver
{
    public PlayerPhysicsControl physics;

    [SerializeField]
    private string resetScene = null;

    override protected void Start()
    {
        base.Start();
        OnDeath = RealOnDeath;
    }

    public void TakeHeal(Heal heal)
    {
        Amount = Math.Min(Amount + heal.Amount, maxHealth);
    }

    private bool RealOnDeath()
    {
        SceneManager.LoadScene(resetScene);
        return true;
    }
}
