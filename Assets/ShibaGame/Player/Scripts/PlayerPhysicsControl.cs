using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsControl : MonoBehaviour
{
    public PlayerAnimatorControl animControl;

    public float LeapForceTime = 1.0f;
    private float leapTimer = 0.0f;

    public LayerMask checkGroundMask;

    public Rigidbody rb;
    private CapsuleCollider collider;

    private bool wasOnGround = true;
    public bool enableAutoJump = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        bool onGround = CheckOnGround();

        // Leap
        if (!IsLeaping() && (Input.GetButtonDown("Jump") || (wasOnGround && !onGround)))
        {
            animControl.Leaping = true;
            Vector3 leapForce = transform.up * 10f + transform.forward * (5.0f + animControl.Movement * 10.0f);
            Debug.Log("Movement: " + animControl.Movement);
            if (enableAutoJump || Input.GetButtonDown("Jump"))
                rb.velocity = leapForce;
            // rb.AddForce(leapForce, ForceMode.VelocityChange);
            leapTimer = LeapForceTime;
        }
        wasOnGround = onGround;

        // Leap Force
        leapTimer = Mathf.Max(leapTimer - Time.deltaTime, 0.0f);
        if (IsLeaping())
        {
            if (leapTimer > 0.0f)
            {
                rb.AddForce(transform.forward * 10.0f * Time.deltaTime, ForceMode.Force);
            }
            else
            {
                rb.AddForce(Vector3.down * 50.0f, ForceMode.Acceleration);
            }
        }

        // Exit Leap
        if (rb.velocity == Vector3.zero && onGround)
        {
            animControl.Leaping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        animControl.Leaping = false;
    }

    bool IsLeaping()
    {
        return animControl.Leaping;
    }

    bool CheckOnGround()
    {
        Vector3 center = transform.position;
        Vector3 extents = collider.bounds.extents;

        RaycastHit hitInfo;
        Ray ray = new Ray(center, Vector3.down);

        Vector3 slimExtents = extents;
        slimExtents.y = 0.1f;

        if (Physics.BoxCast(center, slimExtents, Vector3.down, Quaternion.identity, extents.y + 0.2f, checkGroundMask))
        {
            return true;
        }


        if (Physics.Raycast(ray, out hitInfo, collider.bounds.extents.y + 0.1f, checkGroundMask))
        {
            return true; // Left in here for debugging purposes
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position + collider.center;
        Vector3 extents = collider.bounds.extents;

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(center, extents);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(center + Vector3.down * 0.1f, extents);

        Vector3 slimExtents = extents;
        slimExtents.y = 0.1f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, slimExtents);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center + Vector3.down * (0.2f + extents.y), slimExtents);

        Gizmos.DrawLine(center, center + Vector3.down * (collider.bounds.extents.y + 0.1f));
    }
}
