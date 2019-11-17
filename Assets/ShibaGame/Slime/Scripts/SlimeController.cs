using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private AudioClip sound1;
    [SerializeField]
    private AudioClip sound2;
    [SerializeField]
    private AudioClip sound3;
    [SerializeField]
    private AudioClip sound4;

    private Animator anim;
    private Rigidbody rb;
    private Health health;
    private SoundModulator sm;

    private GameObject player;
    private bool pursuePlayer;
    private Vector3 forward;

    public bool playJumpSound;
    public bool playIdleSound;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        sm = GetComponent<SoundModulator>();

        health.OnDeath = OnDeath;
        pursuePlayer = false;
        forward = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        if (player != null && pursuePlayer)
        {
            Vector3 towardPlayer = player.transform.position - transform.position;
            Quaternion rotationTowardPlayer = Quaternion.LookRotation(towardPlayer);
            Vector3 euler = rotationTowardPlayer.eulerAngles;
            // Fix the rotation around the y-axis only so that the slime still always faces upright
            transform.rotation = Quaternion.Euler(0, euler.y, 0);
            forward.x = towardPlayer.x * Time.deltaTime * 100;
            forward.z = towardPlayer.z * Time.deltaTime * 100;
            //Debug.Log("transform.position before: " + transform.position);
            //transform.position = new Vector3(transform.position.x + forward.x, transform.position.y + forward.y, transform.position.z + forward.z);
            //Debug.Log("transform.position after: " + transform.position);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        Collider c = coll.collider;
        IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
        if (dr != null)
        {
            Damage damage = new Damage(ImpactType.LIGHT, attackDamage, (c.transform.position - transform.position).normalized);
            dr.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            player = c.gameObject;
            anim.SetBool("SlimeHop", true);
            pursuePlayer = true;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            player = c.gameObject;
            anim.SetBool("SlimeHop", false);
            pursuePlayer = false;
        }
    }

    public bool OnDeath()
    {
        //TODO: Add death animation or something
        LootDropper ld = gameObject.GetComponent<LootDropper>();
        ld.DropLoot(transform.position, transform.rotation);
        return false;
    }

    // Animation event audio callbacks
    public void OnJump()
    {
        if (playJumpSound)
        {
            sm.PlayModClip(sound2);
        }
        // transform.position = transform.position 
        // + 
        // new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
        Debug.Log(forward);

        //transform.position = transform.position + forward;
    }

    public void OnIdle()
    {
        if (playIdleSound)
        {
            sm.PlayModClip(sound4);
        } 
    }
}