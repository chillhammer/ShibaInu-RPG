using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksController : MonoBehaviour
{
    [SerializeField] private AudioClip RockSlidesound;

    private SoundModulator sm;
    public int rocknum;
    public GameObject[] rocks;
    private bool flag=true;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider c)
    {
        if (flag)
        {
            for (int i = 0; i < rocknum; i++)
            {
                Rigidbody rb = rocks[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;
                    rocks[i].SetActive(true);   
                }
            }
            sm = GetComponent<SoundModulator>();
            sm.PlayModClip(RockSlidesound);
            flag = false;
        }
    }
}
