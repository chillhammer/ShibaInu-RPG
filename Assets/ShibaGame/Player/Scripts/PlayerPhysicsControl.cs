using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsControl : MonoBehaviour
{
    public PlayerAnimatorControl animControl;

    public float LeapForceTime = 0.1f;
    private float leapTimer = 0.0f;

    public LayerMask checkGroundMask;

    public Rigidbody rb;
    public CapsuleCollider collider;

    private bool wasOnGround = true;
    private float framesSinceGroundContact = 0.0f;
    private float groundContactTime = 10.0f;
    public bool enableAutoJump = false;

    private bool jumpedThisFrame = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        Debug.Log("New Frame. Y: " + transform.position.y);
        jumpedThisFrame = false;
        framesSinceGroundContact++;
        bool onGround = CheckOnGround();

        // Leap
        if (!IsLeaping() && (Input.GetButtonDown("Jump") || (wasOnGround && !onGround)))
        {
            if (Input.GetButtonDown("Jump"))
                Debug.Log("Starting Leap because Jump is Down");
            if (wasOnGround && !onGround)
                Debug.Log("Starting Leap because wasOnGround and now not");

            animControl.Leaping = true;
            jumpedThisFrame = true;
            //onGround = false;
            //framesSinceGroundContact = groundContactTime;
            Vector3 leapForce = transform.up * 20f + transform.forward * (2.0f + animControl.Movement * 10.0f);
            Debug.Log("Movement: " + animControl.Movement);
            if (enableAutoJump || Input.GetButtonDown("Jump"))
                rb.velocity = leapForce;
            else
            {
                //Continue lateral momentum
                leapForce = transform.forward * (animControl.Movement * 15.0f);
                rb.velocity = leapForce;
                // TODO: snap to floor if near
            }
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
                rb.AddForce(transform.forward * 10.0f * Time.deltaTime, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(Vector3.down * 60.0f, ForceMode.Acceleration);
            }
        }
        else
        {
            rb.AddForce(Vector3.down * 20.0f, ForceMode.Acceleration);
        }

        // Exit Leap
        if (onGround && IsLeaping())
        {
            Debug.Log("Exit Leap From OnGround. WasOnGround is: " + (wasOnGround ? "True" : "False"));
            animControl.Leaping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!jumpedThisFrame)
        {
            animControl.Leaping = false;
            Debug.Log("Exit Leap From Collision");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!jumpedThisFrame)
        {
            framesSinceGroundContact = 0.0f;
            Debug.Log("Setting Frames Since Ground Contact to 0 due to CollisionStay: " + collision.other.gameObject.name);
        }
    }

    bool IsLeaping()
    {
        return animControl.Leaping;
    }

    bool CheckOnGround()
    {
        if (Input.GetButtonDown("Jump"))
            framesSinceGroundContact = groundContactTime;

        return framesSinceGroundContact < groundContactTime;
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
