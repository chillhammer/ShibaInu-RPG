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
    private float minStrafeTime = 0;
    [SerializeField]
    private float maxStrafeTime = 1;

    [SerializeField]
    private TriggerEvent agroRangeTrigger = null;
    [SerializeField]
    private TriggerEvent strafeRangeTrigger = null;
    [SerializeField]
    private TriggerEvent attackRangeTrigger = null;
    [SerializeField]
    private TriggerEvent footTrigger = null;

    private NavMeshAgent na;
    private Animator anim;

    private NavMeshPath path;
    private Vector3 nextPos;

    private float endStrafeTime;
    private float strafeTimer;

    void Start()
    {
        na = GetComponent<NavMeshAgent>();
        na.updatePosition = false;
        path = new NavMeshPath();
        anim = GetComponent<Animator>();

        agroRangeTrigger.OnTriggerAction += OnAgroRangeTrigger;
        strafeRangeTrigger.OnTriggerAction += OnStrafeRangeTrigger;
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

    private Vector2 currentDirection;
    private Vector2 targetDirection;
    void OnAnimatorMove()
    {
        //I've essentially implemented a state machine using mecanim
        AnimatorStateInfo stateInfo = anim.IsInTransition(0) ? anim.GetNextAnimatorStateInfo(0) : anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("FastApproach")) {
            na.Move(anim.deltaPosition);
            transform.position = new Vector3(na.nextPosition.x, transform.position.y, na.nextPosition.z);
            transform.rotation = anim.rootRotation;

            float rotationAngle = transform.rotation.eulerAngles.y;
            currentDirection = new Vector2(Mathf.Sin(rotationAngle * Mathf.Deg2Rad), Mathf.Cos(rotationAngle * Mathf.Deg2Rad));
            Vector3 relativeTargetPos = nextPos - transform.position;
            targetDirection = new Vector2(relativeTargetPos.x, relativeTargetPos.z);
            float angleBetween = Vector2.SignedAngle(currentDirection, targetDirection);
            float turningFactor = Mathf.Clamp(angleBetween / 30, -1, 1);
            anim.SetFloat("Turning", turningFactor / 2 + .5f);
        } else if (stateInfo.IsName("Strafe")) {
            na.Move(anim.deltaPosition);
            transform.position = new Vector3(na.nextPosition.x, transform.position.y, na.nextPosition.z);

            //Rotate to face target
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            strafeTimer += Time.deltaTime;
            if (strafeTimer >= endStrafeTime) {
                anim.SetTrigger("Attack");
            }
        } else if (stateInfo.IsName("RunUp")) {
            na.Move(anim.deltaPosition);
            transform.position = new Vector3(na.nextPosition.x, transform.position.y, na.nextPosition.z);
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        } else {
            na.Move(anim.deltaPosition);
            transform.position = new Vector3(na.nextPosition.x, transform.position.y, na.nextPosition.z);
            transform.rotation = anim.rootRotation;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(currentDirection.x, 0, currentDirection.y).normalized * 5);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector3(targetDirection.x, 0, targetDirection.y).normalized * 5);
    }

    private void OnAgroRangeTrigger(TriggerEvent.ActionType type, Collider c)
    {
        if (type == TriggerEvent.ActionType.ENTER) {
            if (c.transform == target)
                anim.SetBool("InAgroRange", true);
        } else {
            if (c.transform == target)
                anim.SetBool("InAgroRange", false);
        }
    }

    private void OnAttackRangeTrigger(TriggerEvent.ActionType type, Collider c)
    {
        if (type == TriggerEvent.ActionType.ENTER) {
            if (c.transform == target)
                anim.SetBool("InAttackRange", true);
        } else {
            if (c.transform == target)
                anim.SetBool("InAttackRange", false);
        }
    }

    private void EnterStrafeState()
    {
        anim.SetBool("InStrafeRange", true);
        anim.SetFloat("StrafeDirection", Random.Range(0, 2) == 0 ? 0 : 1); //Randomly choose 0 or 1
        strafeTimer = 0;
        endStrafeTime = Random.Range(minStrafeTime, maxStrafeTime);
    }

    private void OnStrafeRangeTrigger(TriggerEvent.ActionType type, Collider c)
    {
        if (type == TriggerEvent.ActionType.ENTER) {
            if (c.transform == target)
                EnterStrafeState();
        } else {
            if (c.transform == target)
                anim.SetBool("InStrafeRange", false);
        }
    }

    private void OnFootTrigger(TriggerEvent.ActionType type, Collider c)
    {
        if (type == TriggerEvent.ActionType.ENTER) {
            IDamageReceiver dr = c.GetComponent<IDamageReceiver>();
            if (dr != null) {
                Damage damage = new Damage(ImpactType.LIGHT, attackDamage, (c.transform.position - transform.position).normalized);
                dr.TakeDamage(damage);
            }
        }
    }
}
