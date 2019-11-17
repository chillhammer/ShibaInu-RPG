using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls animations and other movement
public class PlayerAnimatorControl : MonoBehaviour
{

    public bool Leaping { get { return anim.GetBool("Leap"); } set { anim.SetBool("Leap", value); } }
    public bool Attacking { get { return anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"); } }
    public float Movement { get { return Mathf.Clamp01(Mathf.Sqrt(inputHor * inputHor + inputVert * inputVert)); } }

    // Finds out how much the player is facing towards the input direction

    public Vector3 InputDir { get { return Vector3.Normalize(new Vector3(inputHor, 0, inputVert)); } }
    public Vector3 MovementDir { get { return Quaternion.LookRotation(cameraControl.Forward, Vector3.up) * InputDir; } }
    private float DotToMovementDir {  get {
            Vector3 facing = transform.forward;

            Vector3 movementDir = MovementDir;

            return Mathf.Max(Vector3.Dot(facing, movementDir), 0.0f);
        } }

    public CameraPlayerControl cameraControl;

    public float leanLerpSpeed = 2.0f; // Animation speed to switch from leans
    public float turnSpeedFollowingPlayer = 290.0f;
    public float turnSpeedFreeCamera = 180.0f;

    public float snapToFacingAngleThreshold = 20.0f;

    public int attackDamage = 1;

    public TriggerEvent pawColliderTrigger;

    private Animator anim;

    [SerializeField]
    private LockOn lockOn;

    private float lean = 0.0f;
    private float inputHor;
    private float inputVert;
    private float centerOffset;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        centerOffset = transform.localPosition.z;

        pawColliderTrigger.OnTrigger += OnPawTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !Leaping)
        {
            //Attacking = true;
            anim.SetTrigger("Attack");

            if (lockOn.IsLocked)
                transform.parent.rotation = Quaternion.LookRotation(    
                    new Vector3(lockOn.target.transform.position.x - transform.parent.position.x, 0.0f, lockOn.target.transform.position.z - transform.parent.position.z));
        }
    }

    void OnAnimatorMove()
    {
        inputVert = Input.GetAxis("Vertical");
        inputHor = Input.GetAxis("Horizontal");

        // Can't Move Backwards if Camera Free
        if (!cameraControl.followPlayer) inputVert = Mathf.Clamp01(inputVert);

        float movement = Movement;

        // Rotation is isolated
        HandleRotationCameraFollowing();
        HandleRotationCameraFree();


        lean = Mathf.Lerp(lean, inputHor, 2.0f * Time.deltaTime);

        // Testing Fixed Rotation, So that Forward Movement Only Works Once Faced in the Right Direction
        movement = DotToMovementDir * DotToMovementDir;

        movement = Mathf.Clamp(movement, 0, (Input.GetKey(KeyCode.LeftShift) ? 0.1f : 1.0f)); // Allow to slow down by holding Shift



        // Movement
        Vector3 newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);// - transform.forward * centerOffset;
        this.transform.parent.position += anim.deltaPosition; // newRootPosition;
        

        anim.SetFloat("vely", movement);
        anim.SetFloat("velx", inputHor);
        anim.SetFloat("lean", lean);

        anim.SetLayerWeight(anim.GetLayerIndex("Lean"), Mathf.Abs(lean) * 1.0f);

    }

    void HandleRotationCameraFollowing()
    {
        if (!cameraControl.followPlayer || Movement == 0.0f || Attacking)
            return;

        float movement = Movement;

        float turnSpeed = GetTurnSpeed();


        Vector3 inputDir = Vector3.Normalize(new Vector3(inputHor, 0, inputVert));
        Vector3 facing = transform.forward;

        Vector3 movementDir = Quaternion.LookRotation(cameraControl.Forward, Vector3.up) * inputDir;
        float turnAngle = Vector3.SignedAngle(facing, movementDir, Vector3.up);

        // Change Turning To Turn to Input Direction
        float turnSign = Mathf.Clamp(turnAngle, -1.0f, 1.0f);

        Quaternion newRootRotation = anim.rootRotation * Quaternion.AngleAxis(turnSign * turnSpeed * Time.deltaTime, Vector3.up);

        // Snap to Target
        if (Mathf.Abs(turnAngle) < snapToFacingAngleThreshold)
        {
            newRootRotation = Quaternion.FromToRotation(Vector3.forward, movementDir);
        }

        // Setting rotation
        transform.parent.rotation = newRootRotation;

        // Failed attempt to extract only y component of rotation to coordinate with x,z components being set to terrain's normals
        //Quaternion orientQuat = Quaternion.FromToRotation(Vector3.up, orientVec);
        //Quaternion rot = transform.parent.rotation;
        //rot.eulerAngles = new Vector3(rot.x, newRootRotation.eulerAngles.y, rot.z);
        //transform.parent.rotation = rot;

    }

    void HandleRotationCameraFree()
    {
        if (cameraControl.followPlayer)
            return;

        float turnSpeed = GetTurnSpeed();

        Quaternion newRootRotation = anim.rootRotation * Quaternion.AngleAxis(inputHor * turnSpeed * Time.deltaTime, Vector3.up);

        this.transform.parent.rotation = newRootRotation;
    }

    float GetTurnSpeed()
    {
        // Modify Turn Speed
        float turnSpeed = (cameraControl.followPlayer ? turnSpeedFollowingPlayer : turnSpeedFreeCamera);
        // Air Slow Turn - Currently disabled. Weird edge case that needs to be fixed here
        //turnSpeed *= (Leaping ? 0.05f : 1.0f);

        return turnSpeed;
    }

    private void OnPawTrigger(TriggerEvent.ActionType type, Collider c)
    {
        AnimatorStateInfo stateInfo = anim.IsInTransition(0) ? anim.GetNextAnimatorStateInfo(0) : anim.GetCurrentAnimatorStateInfo(0);

        //TODO: Change "Attack" to whatever the attack state is called in the shiba animator
        if (type == TriggerEvent.ActionType.ENTER && stateInfo.IsName("Attack")) {
            IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
            if (dr != null) {
                Damage damage = new Damage(ImpactType.LIGHT, attackDamage, (c.transform.position - transform.position).normalized);
                dr.TakeDamage(damage);
            }
        }
    }
}
