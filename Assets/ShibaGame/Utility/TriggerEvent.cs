using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public enum ActionType
    {
        ENTER,
        EXIT,
    }
    public delegate void TriggerAction(ActionType type, Collider collider);
    public event TriggerAction OnTrigger;

    void OnTriggerEnter(Collider c)
    {
        OnTrigger(ActionType.ENTER, c);
    }

    void OnTriggerExit(Collider c)
    {
        OnTrigger(ActionType.EXIT, c);
    }
}
