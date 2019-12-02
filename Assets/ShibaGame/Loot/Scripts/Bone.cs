using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bone : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryMenu = null;

    private float spinSpeed = 90;
    private float bobSpeed = 1;
    private float bobAmplitude = 0.2f;
    private bool idling = true;
    private float originY;

    private CanvasGroup cg;

    void Start()
    {
        cg = victoryMenu.GetComponent<CanvasGroup>();

        originY = transform.position.y;
    }

    void Update()
    {
        if (idling) {
            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
            Vector3 newPos = transform.position;
            newPos.y = originY + Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
            transform.position = newPos;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
