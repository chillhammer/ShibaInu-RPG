using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health, IHealthReceiver
{
    public PlayerPhysicsControl physics;

    [SerializeField]
    private CanvasGroup deathOverlay;

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
        deathOverlay.alpha = 1;
        deathOverlay.blocksRaycasts = true;
        deathOverlay.interactable = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        return true;
    }
}
