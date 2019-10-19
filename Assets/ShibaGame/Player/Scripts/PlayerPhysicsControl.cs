using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsControl : MonoBehaviour
{
    public PlayerAnimatorControl animControl;

    public float LeapForceTime = 1.0f;
    private float leapTimer = 0.0f;

    public Rigidbody rb;
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
            Vector3 leapForce = transform.up * 10f + transform.forward * (5.0f + animControl.Movement * 10.0f);
            Debug.Log("Movement: " + animControl.Movement);
            rb.velocity = leapForce;
            // rb.AddForce(leapForce, ForceMode.VelocityChange);
            leapTimer = LeapForceTime;
        }

        // Leap Force
        leapTimer = Mathf.Max(leapTimer - Time.deltaTime, 0.0f);
        if (animControl.Leaping && leapTimer > 0.0f)
        {
            rb.AddForce(transform.forward * 10.0f * Time.deltaTime, ForceMode.Force);
        } else if (animControl.Leaping)
        {
            rb.AddForce(-Vector3.up * 50.0f, ForceMode.Acceleration);  
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
