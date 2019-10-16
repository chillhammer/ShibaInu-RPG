using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NinjaController : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private TriggerEvent agroTrigger = null;
    [SerializeField]
    private TriggerEvent attackRangeTrigger = null;
    [SerializeField]
    private TriggerEvent footTrigger = null;

    private NavMeshAgent na;
    private Animator anim;

    private NavMeshPath path;
    private Vector3 nextPos;

    void Start()
    {
        na = GetComponent<NavMeshAgent>();
        na.updatePosition = false;
        path = new NavMeshPath();
        anim = GetComponent<Animator>();

        agroTrigger.OnTriggerAction += OnAgroTrigger;
        attackRangeTrigger.OnTriggerAction += OnAttackRangeTrigger;
        footTrigger.OnTriggerAction += OnFootTrigger;
    }

    void Update()
    {
        na.nextPosition = transform.position;
        na.CalculatePath(target.position, path);
        if (path.corners.Length >= 2) {
            nextPos = path.corners[1];
        }
    }

    private Vector2 planarVelocity;
    private Vector2 targetPlanarVelocity;
    void OnAnimatorMove()
    {
        na.Move(anim.deltaPosition);
        transform.position = new Vector3(na.nextPosition.x, transform.position.y, na.nextPosition.z);
        transform.rotation = anim.rootRotation;

        float rotationAngle = transform.rotation.eulerAngles.y;
        planarVelocity = new Vector2(Mathf.Sin(rotationAngle * Mathf.Deg2Rad), Mathf.Cos(rotationAngle * Mathf.Deg2Rad));
        Vector3 relativeNextPos = nextPos - transform.position;
        targetPlanarVelocity = new Vector2(relativeNextPos.x, relativeNextPos.z);
        float angleBetween = Vector2.SignedAngle(planarVelocity, targetPlanarVelocity);
        anim.SetFloat("Turning", Mathf.Clamp(angleBetween / 30, -1, 1) / 2 + .5f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(planarVelocity.x, 0, planarVelocity.y).normalized * 5);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector3(targetPlanarVelocity.x, 0, targetPlanarVelocity.y).normalized * 5);
    }

    private void OnAgroTrigger(TriggerEvent.ActionType type, Collider c)
    {
        if (type == TriggerEvent.ActionType.ENTER) {
            Debug.Log(c.name + " enterd agro");
            anim.SetBool("Agro", true);
        } else {
            Debug.Log(c.name + "exited agro");
            anim.SetBool("Agro", false);
        }
    }

    private void OnAttackRangeTrigger(TriggerEvent.ActionType type, Collider c)
    {
        if (type == TriggerEvent.ActionType.ENTER) {
            Debug.Log(c.name + " enterd attack");
            if (c.transform == target)
                anim.SetBool("InAttackRange", true);
        } else {
            Debug.Log(c.name + "exited attack");
            if (c.transform == target)
                anim.SetBool("InAttackRange", false);
        }
    }

    private void OnFootTrigger(TriggerEvent.ActionType type, Collider c)
    {
        if (type == TriggerEvent.ActionType.ENTER) {
            IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
            if (dr != null) {
                Damage damage = new Damage(ImpactType.LIGHT, attackDamage, c.transform.position - transform.position);
                dr.TakeDamage(damage);
            }
        }
    }
}
