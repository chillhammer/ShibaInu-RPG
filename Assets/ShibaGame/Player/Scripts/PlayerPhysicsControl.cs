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

    [SerializeField]
    private bool wasOnGround = true;
    private float framesSinceGroundContact = 0.0f;
    private float groundContactTime = 5.0f;
    public bool enableAutoJump = false;

    private bool jumpedThisFrame = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        //Debug.Log("New Frame. Y: " + transform.position.y);
        jumpedThisFrame = false;
        framesSinceGroundContact++;
        bool onGround = CheckOnGround();

        if (onGround && !wasOnGround)
            Debug.Log("Was not on ground, and now am onGround");

        // Leap
        if (!IsLeaping() && (Input.GetButtonDown("Jump") || (wasOnGround && !onGround && !CheckNearGround(0.5f))))
        {
            if (Input.GetButtonDown("Jump"))
                Debug.Log("Starting Leap because Jump is Down");
            if (wasOnGround && !onGround)
                Debug.Log("Starting Leap because wasOnGround and now not");

            animControl.Leaping = true;
            jumpedThisFrame = true;
            animControl.JumpSound();
            //onGround = false;
            //framesSinceGroundContact = groundContactTime;
            Vector3 leapForce = transform.up * 15f + transform.forward * (2.0f + animControl.Movement * 10.0f);
            //Debug.Log("Movement: " + animControl.Movement);
            if (enableAutoJump || Input.GetButtonDown("Jump"))
                rb.velocity = leapForce;
            else
            {
                //Continue lateral momentum
                leapForce = transform.forward * (animControl.Movement * 15.0f);
                rb.velocity = leapForce;
            }
            // rb.AddForce(leapForce, ForceMode.VelocityChange);
            leapTimer = LeapForceTime;
        }
        wasOnGround = onGround;

        // Leap Force
        leapTimer = Mathf.Max(leapTimer - 1, 0.0f);
        if (IsLeaping())
        {

            if (leapTimer > 0.0f)
            {
                rb.AddForce(transform.forward * 5.0f, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(animControl.MovementDir * 50.0f, ForceMode.Acceleration);

                // Clamp Lateral Velocity
                Vector3 lateralVelocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
                lateralVelocity = Vector3.ClampMagnitude(lateralVelocity, 12.0f);

                // In-air drag
                if (animControl.Movement <= 0.1f)
                {
                    float lateralVelocityMagnitude = lateralVelocity.magnitude;
                    Debug.Log("in-air drag. magnitude: " + lateralVelocityMagnitude + " and movementDir: " + animControl.MovementDir);
                    lateralVelocity -= lateralVelocity.normalized * Mathf.Min(lateralVelocityMagnitude, 10.0f);
                }
                rb.velocity = new Vector3(lateralVelocity.x, rb.velocity.y, lateralVelocity.z);


                rb.AddForce(Vector3.down * 40.0f, ForceMode.Acceleration);
            }
        }
        else
        {
            rb.AddForce(Vector3.down * 50.0f, ForceMode.Acceleration);
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
        if (!jumpedThisFrame && IsNormalOnGround(collision))
        {
            animControl.Leaping = false;
            Debug.Log("Exit Leap From Collision");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!jumpedThisFrame && IsNormalOnGround(collision))
        {
            animControl.Leaping = false;
            framesSinceGroundContact = 0.0f;
            //Debug.Log("Setting Frames Since Ground Contact to 0 due to CollisionStay: " + collision.other.gameObject.name);
        } else
        {
            rb.AddForce(Vector3.down * 20.0f, ForceMode.Acceleration);
            //Debug.Log("CollsionStay: Accelerating down");
        }

        // Orientation
        //Vector3 orientVec = FindAverageCollisionNormal(collision);
        //Quaternion orientQuat = Quaternion.FromToRotation(Vector3.up, orientVec);
        //Quaternion rot = transform.rotation;
        //rot.eulerAngles = new Vector3(orientQuat.eulerAngles.x, transform.rotation.eulerAngles.y, orientQuat.eulerAngles.z);
        //transform.rotation = rot;
        //transform.up = orientVec;
        //transform.rotation = Quaternion.FromToRotation(transform.up, orientVec) * transform.rotation;
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

    bool CheckNearGround(float amount)
    {
        Vector3 center = transform.position;
        Vector3 extents = collider.bounds.extents;

        RaycastHit hitInfo;
        Ray ray = new Ray(center, Vector3.down);

        Vector3 slimExtents = extents;
        slimExtents.y = 0.1f;

        if (Physics.BoxCast(center, slimExtents, Vector3.down, Quaternion.identity, extents.y + 0.2f + amount, checkGroundMask))
        {
            return true;
        }


        if (Physics.Raycast(ray, out hitInfo, collider.bounds.extents.y + 0.1f + amount, checkGroundMask))
        {
            return true; // Left in here for debugging purposes
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        //Vector3 center = transform.position + collider.center;
        //Vector3 extents = collider.bounds.extents;

        ////Gizmos.color = Color.red;
        ////Gizmos.DrawWireCube(center, extents);
        ////Gizmos.color = Color.blue;
        ////Gizmos.DrawWireCube(center + Vector3.down * 0.1f, extents);

        //Vector3 slimExtents = extents;
        //slimExtents.y = 0.1f;

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(center, slimExtents);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(center + Vector3.down * (0.2f + extents.y), slimExtents);

        //Gizmos.DrawLine(center, center + Vector3.down * (collider.bounds.extents.y + 0.1f));
    }

    private bool IsNormalOnGround(Collision collision)
    {
        foreach (ContactPoint point in collision.contacts)
        {
            Vector3 normal = point.normal;
            if (Vector3.Dot(normal, Vector3.up) > 0.6f)
                return true;
        }
        return false;
    }

    private Vector3 FindAverageCollisionNormal(Collision collision = null)
    {
        if (collision == null)
            return Vector3.up;

        Vector3 averageNormal = Vector3.zero;
        foreach (ContactPoint point in collision.contacts)
        {
            averageNormal += point.normal;
        }
        averageNormal /= collision.contacts.Length;

        return averageNormal;
    }
}
