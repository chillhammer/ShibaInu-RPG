using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorControl : MonoBehaviour
{
    private Animator anim;

    private float lean = 0.0f;

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
        float inputVert = Input.GetAxis("Vertical");
        float inputHor = Input.GetAxis("Horizontal");
        
        lean = Mathf.Lerp(lean, inputHor, 2.0f * Time.deltaTime);
        float movement = Mathf.Sqrt(inputHor * inputHor + inputVert + inputVert) - Mathf.Abs(inputHor) * 0.05f;


        Vector3 newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        Quaternion newRootRotation = anim.rootRotation * Quaternion.AngleAxis(inputHor * 180.0f * Time.deltaTime, Vector3.up);


        float rootMovementSpeed = 1.0f;
        float rootTurnSpeed = 1.0f;

        newRootPosition = Vector3.LerpUnclamped(transform.position, newRootPosition, rootMovementSpeed);
        newRootRotation = Quaternion.LerpUnclamped(transform.rotation, newRootRotation, rootTurnSpeed);


        this.transform.position = newRootPosition;
        this.transform.rotation = newRootRotation;

        

        

        anim.SetFloat("vely", movement);
        anim.SetFloat("velx", inputHor);
        anim.SetFloat("lean", lean);
        
        anim.SetLayerWeight(anim.GetLayerIndex("Lean"), Mathf.Abs(lean) * 1.0f);
        
    }
}
