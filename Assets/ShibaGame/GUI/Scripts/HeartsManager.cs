using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsManager : MonoBehaviour
{
    [SerializeField]
    private uint heartImageWidth = 20;
    [SerializeField]
    private PlayerHealth playerHealth = null;

    private Slider s;

    void Start()
    {
        s = GetComponent<Slider>();
        s.maxValue = playerHealth.MaxHealth;

        //Size to match the max health
        RectTransform rTrans = GetComponent<RectTransform>();
        rTrans.offsetMax = new Vector2(heartImageWidth * playerHealth.MaxHealth, rTrans.offsetMax.y);

        playerHealth.OnHealthChange += OnHealthChange;
    }

    private void OnHealthChange(int oldVal, int newVal)
    {
        s.value = newVal;
    }
}
