using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerControl : MonoBehaviour
{
    public bool followPlayer = true;
    public Vector3 Forward
    {
        get
        {
            Vector3 dir = pivotTransform.position - cameraHolder.transform.position;
            dir.y = 0.0f;
            return dir.normalized;
        }
    }

    private GameObject cameraHolder;
    private Transform pivotTransform;
    private float stickLength; // length of the camera boom stick

    public Vector3 cameraEulers;
    public float minPitchAngle = -45.0f;
    public float maxPitchAngle =  45.0f;
    public float pitchSensitivity = 5.0f;
    public float yawSensitivity = 5.0f;
    public float maxZoom = 20.0f;
    public LayerMask canCollideWith;

    public LockOn lockOn;

    // Start is called before the first frame update
    void Start()
    {
        cameraHolder = GameObject.Find("CameraHolder");
        pivotTransform = cameraHolder.transform.parent.transform; // Assumming pivot is camera holder's parent
        stickLength = Mathf.Abs(cameraHolder.transform.localPosition.z);
        UpdateCameraState();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            followPlayer = !followPlayer;
            UpdateCameraState();
        }
    }

    private void LateUpdate()
    {
        if (!followPlayer)
            return;

        transform.position = cameraHolder.transform.position;

        if (!lockOn.IsLocked)
        {
            cameraEulers.x = Mathf.Clamp(cameraEulers.x - Input.GetAxis("Mouse Y") * Time.deltaTime * pitchSensitivity, minPitchAngle, maxPitchAngle);
            cameraEulers.y = cameraEulers.y + Input.GetAxis("Mouse X") * Time.deltaTime * yawSensitivity;
        } else
        {
            
            Quaternion lockOnRotation = Quaternion.LookRotation(lockOn.target.transform.position - lockOn.transform.position);
            Vector3 targetEulers = lockOnRotation.eulerAngles;
            targetEulers.x = 25.0f;
            lockOnRotation = Quaternion.Euler(targetEulers);

            Quaternion temp = Quaternion.Euler(cameraEulers);
            temp = Quaternion.Slerp(temp, lockOnRotation, 0.1f);
            cameraEulers = temp.eulerAngles;

            //cameraEulers = Vector3.Slerp(cameraEulers, targetEulers, 0.1f);
        }

        // Rotating Pivot
        Quaternion pivotRotation = Quaternion.Euler(cameraEulers);
        cameraHolder.transform.parent.transform.rotation = pivotRotation;

        // Zoom
        stickLength = Mathf.Clamp(stickLength - Input.GetAxis("Zoom") * 3.0f, 0.0f, maxZoom);

        // Camera Z Movement - Based on Zoom or World Collision
        float minZ = -maxZoom;
        Ray toCamera = new Ray(pivotTransform.position, cameraHolder.transform.position - pivotTransform.position);
        RaycastHit hit;
        if (Physics.Raycast(toCamera, out hit, stickLength, canCollideWith))
        {
            minZ = Mathf.Max(-hit.distance + 0.2f, minZ);
            // Debug.Log("Hit Distance: " + hit.distance);
        }
        float newZ = Mathf.Clamp(-stickLength, minZ, 0);
        
        cameraHolder.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(cameraHolder.transform.localPosition.z, newZ, 0.5f));
    }

    // Allows initial changes upon enabling/disabling following player
    void UpdateCameraState()
    {
        if (followPlayer)
        {
            transform.SetParent(cameraHolder.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
        {
            transform.SetParent(null);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
