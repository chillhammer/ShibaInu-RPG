using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggler : MonoBehaviour
{
    [SerializeField]
    private GameObject target = null;

    public void Enable()
    {
        target.SetActive(true);
    }

    public void Disable()
    {
        target.SetActive(false);
    }
}
