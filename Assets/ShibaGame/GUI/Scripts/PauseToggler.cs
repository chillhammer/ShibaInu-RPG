using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseToggler : MonoBehaviour
{
    private CanvasGroup cg;
    private CursorLockMode prevCursorLockState;
    private bool prevCursorVisible;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        ToggleCanvasGroup(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (cg.alpha == 1) {
                ToggleCanvasGroup(false);
                Time.timeScale = 1;
                Cursor.lockState = prevCursorLockState;
                Cursor.visible = prevCursorVisible;
            } else {
                ToggleCanvasGroup(true);
                Time.timeScale = 0;
                prevCursorLockState = Cursor.lockState;
                prevCursorVisible = Cursor.visible;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void ToggleCanvasGroup(bool shouldEnable)
    {
        cg.alpha = shouldEnable ? 1 : 0;
        cg.blocksRaycasts = shouldEnable;
        cg.interactable = shouldEnable;
    }
}
