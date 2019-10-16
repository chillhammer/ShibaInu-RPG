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
    public event TriggerAction OnTriggerAction;

    void OnTriggerEnter(Collider c)
    {
        OnTriggerAction(ActionType.ENTER, c);
    }

    void OnTriggerExit(Collider c)
    {
        OnTriggerAction(ActionType.EXIT, c);
    }
}
