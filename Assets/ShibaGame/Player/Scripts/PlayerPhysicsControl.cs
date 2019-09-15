using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsControl : MonoBehaviour
{
    public PlayerAnimatorControl animControl;

    public float LeapForceTime = 1.0f;
    private float leapTimer = 0.0f;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Leap
        if (!animControl.Leaping && Input.GetAxis("Fire1") > 0)
        {
            animControl.Leaping = true;
            Vector3 leapForce = transform.up * 4f + transform.forward * (25.0f + animControl.Movement * 2.0f);
            rb.velocity = leapForce;
            // rb.AddForce(leapForce, ForceMode.VelocityChange);
            leapTimer = LeapForceTime;
        }

        // Leap Force
        leapTimer = Mathf.Max(leapTimer - Time.deltaTime, 0.0f);
        if (animControl.Leaping && leapTimer > 0.0f)
        {
            rb.AddForce(transform.forward * 1000.0f * Time.deltaTime, ForceMode.Force);
        } else if (animControl.Leaping)
        {
            rb.AddForce(-Vector3.up * 20.0f, ForceMode.Acceleration);
        }

        // Exit Leap
        if (rb.velocity == Vector3.zero)
        {
            animControl.Leaping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        animControl.Leaping = false;
    }
}
