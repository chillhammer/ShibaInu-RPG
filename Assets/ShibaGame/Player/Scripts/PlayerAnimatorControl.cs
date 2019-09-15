using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls animations and other movement
public class PlayerAnimatorControl : MonoBehaviour
{

    public bool Leaping { get { return anim.GetBool("Leap"); } set { anim.SetBool("Leap", value); } }
    public float Movement { get { return Mathf.Clamp01(Mathf.Sqrt(inputHor * inputHor + inputVert * inputVert)); } }

    public CameraPlayerControl cameraControl;

    public float leanLerpSpeed = 2.0f; // Animation speed to switch from leans
    public float turnSpeedFollowingPlayer = 290.0f;
    public float turnSpeedFreeCamera = 180.0f;

    private Animator anim;

    private float lean = 0.0f;
    private float inputHor;
    private float inputVert;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorMove()
    {
        inputVert = Input.GetAxis("Vertical");
        inputHor = Input.GetAxis("Horizontal");

        // Can't Move Backwards if Camera Free
        if (!cameraControl.followPlayer) inputVert = Mathf.Clamp01(inputVert);

        float movement = Movement;

        float turnSpeed = turnSpeedFreeCamera;


        Vector3 inputDir = Vector3.Normalize(new Vector3(inputHor, 0, inputVert));
        Vector3 facing = transform.forward;

        // Change Input If Following Player
        if (cameraControl.followPlayer)
        {
            Vector3 movementDir = Quaternion.LookRotation(cameraControl.Forward, Vector3.up) * inputDir;
            float turnAngle = Vector3.SignedAngle(facing, movementDir, Vector3.up);
            if (Mathf.Abs(turnAngle) < 5.0f) turnAngle = 0.0f; // Remove Jitter

            // Change Turning To Turn to Input Direction
            inputHor = Mathf.Clamp(turnAngle, -1.0f, 1.0f);

            

            // Modify Turn Speed
            turnSpeed = turnSpeedFollowingPlayer;
        }
        // Air Slow Turn
        turnSpeed *= (Leaping ? 0.1f : 1.0f);


        lean = Mathf.Lerp(lean, inputHor, 2.0f * Time.deltaTime);
        movement = Mathf.Clamp(movement, 0, (Input.GetKey(KeyCode.LeftShift) ? 0.5f : 1.0f)); // Allow to slow down by holding Shift


        Vector3 newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        Quaternion newRootRotation = anim.rootRotation * Quaternion.AngleAxis(inputHor * turnSpeed * Time.deltaTime, Vector3.up);


        this.transform.parent.position = newRootPosition;
        this.transform.parent.rotation = newRootRotation;


        anim.SetFloat("vely", movement);
        anim.SetFloat("velx", inputHor);
        anim.SetFloat("lean", lean);
        
        anim.SetLayerWeight(anim.GetLayerIndex("Lean"), Mathf.Abs(lean) * 1.0f);
        
    }

}
