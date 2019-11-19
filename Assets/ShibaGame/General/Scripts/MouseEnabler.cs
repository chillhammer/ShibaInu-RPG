using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnabler : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
