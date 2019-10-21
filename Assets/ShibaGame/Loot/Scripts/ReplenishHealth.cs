using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishHealth : MonoBehaviour
{
    public int defaultHeal = 10;

    private void OnTriggerEnter(Collider collision)
    {
        IHealthReceiver receiver = collision.gameObject.GetComponent<IHealthReceiver>();
        if (receiver != null)
        {
            Heal heal = new Heal(defaultHeal);
            receiver.TakeHeal(heal);
        }
    }
}
