using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOn : MonoBehaviour
{
    [SerializeField]
    private Camera lockOnCamera;

    [SerializeField]
    private Transform pivotTransform;

    [SerializeField]
    private Canvas lockOnCanvas;

    [SerializeField]
    private Image lockOnImage;

    [SerializeField]
    private CameraPlayerControl cameraControl;

    public LockOnTarget target;

    public bool IsLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            LockOnTarget[] targets = GameObject.FindObjectsOfType<LockOnTarget>();

            // Find best target
            target = null;
            float bestDot = 0.0f;
            foreach (LockOnTarget t in targets)
            {
                float dot = Vector3.Dot(lockOnCamera.transform.forward, (t.transform.position - lockOnCamera.transform.position).normalized);
                if (dot > bestDot)
                {
                    target = t;
                    bestDot = dot;
                }
            }

        }

        lockOnImage.enabled = false;
        IsLocked = false;
        if (Input.GetMouseButton(1) && target != null)
        {
            lockOnImage.enabled = true;
            lockOnImage.transform.position = lockOnCamera.WorldToScreenPoint(target.transform.position);
            IsLocked = true;
        }
    }
    private void LateUpdate()
    {
        
    }
}
