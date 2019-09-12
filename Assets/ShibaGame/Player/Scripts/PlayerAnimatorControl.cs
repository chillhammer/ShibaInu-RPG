using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorControl : MonoBehaviour
{
    private Animator anim;

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

        Vector3 newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        Quaternion newRootRotation = anim.rootRotation;


        float rootMovementSpeed = 1.0f;
        float rootTurnSpeed = 1.0f;

        newRootPosition = Vector3.LerpUnclamped(transform.position, newRootPosition, rootMovementSpeed);
        newRootRotation = Quaternion.LerpUnclamped(transform.rotation, newRootRotation, rootTurnSpeed);


        this.transform.position = newRootPosition;
        this.transform.rotation = newRootRotation;

        float inputVert = Input.GetAxis("Vertical");
        float inputHor = Input.GetAxis("Horizontal");
        float movement = Mathf.Sqrt(inputHor * inputHor + inputVert + inputVert) - Mathf.Abs(inputHor) * 0.75f;

        anim.SetFloat("vely", movement);
        anim.SetFloat("velx", Input.GetAxis("Horizontal"));
        
        anim.SetLayerWeight(anim.GetLayerIndex("Lean"), Mathf.Abs(inputHor) * 0.25f);
        
    }
}
