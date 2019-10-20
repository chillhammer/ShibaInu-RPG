using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingLog : MonoBehaviour
{
    public int cnt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cnt += 1;
        if (cnt % 100 < 50)
        {
            transform.localPosition += new Vector3(0.1f, 0, 0);

        }
        else
        {
            transform.localPosition -= new Vector3(0.1f, 0, 0);
        }

    }
}
