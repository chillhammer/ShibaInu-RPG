﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    public float range = 1.5f;
    public float speed = 2.0f;
    public float direction = 1;

    private Quaternion startPosition;

    void Start()
    {
        startPosition = transform.rotation;
    }

    void Update()
    {
        Quaternion theta = startPosition;
        theta.x += direction * (Mathf.Sin(Time.time * speed) * range);
        transform.rotation = theta;

    }
}