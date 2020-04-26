using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody rb;

    [SerializeField] private float attackTime = 0.4f;
    float elapsedTimeSinceLastAttack = 1000;
    bool shouldAttack;
    bool isAttacking;
    int attackComboIndex;
    int maxCombo = 2;
    [SerializeField] float comboExpireTime = 2f;
    
    float inputQueueTime;
    float queuedAngle;
    bool shoudPerformQueuedAttack;

    [SerializeField] AttackPoints attackPoints;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float attackDamage = 1f;
    [SerializeField] float damageDelay = 0.2f;

    [SerializeField] LayerMask damagableLayers;

    private Vector3 driftDirection;
    [SerializeField] float driftSpeed = 10f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();

        inputQueueTime = attackTime / 2;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 dir = playerController.GetMouseClickScreenDirection().normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) / Mathf.PI * 180f;

            if (attackTime < elapsedTimeSinceLastAttack)
            {
                StartAttacking(angle);
                UpdateComboIndex();
                elapsedTimeSinceLastAttack = 0;
            }
            else if(inputQueueTime < elapsedTimeSinceLastAttack)
            {
                shoudPerformQueuedAttack = true;
                queuedAngle = angle;
            }
        }

        if(attackTime < elapsedTimeSinceLastAttack && !isAttacking && shoudPerformQueuedAttack)
        {
            shoudPerformQueuedAttack = false;
            StartAttacking(queuedAngle);
            UpdateComboIndex();
            elapsedTimeSinceLastAttack = 0;
        }

        elapsedTimeSinceLastAttack += Time.deltaTime;
        if (elapsedTimeSinceLastAttack > attackTime && isAttacking)
        {
            isAttacking = false;
            playerController.LockInput(false);
        }
        
        if (comboExpireTime < elapsedTimeSinceLastAttack)
            attackComboIndex = 0;
    }

    private void StartAttacking(float angle)
    {
        isAttacking = true;
        playerController.anim.SetTrigger("Attack");
        playerController.LockInput(true);
        rb.velocity = Vector3.zero;
        SetDirectionThenAttack(angle);
    }

    private void UpdateComboIndex()
    {
        playerController.anim.SetInteger("AttackComboIndex", attackComboIndex);

        if (attackComboIndex < maxCombo)
            attackComboIndex++;
        else attackComboIndex = 0;
    }

    private void SetDirectionThenAttack(float angle)
    {
        if (angle >= -22.5f && angle < 22.5f)
        {
            playerController.SetPlayerDirection(Direction.Right);

            playerController.anim.SetTrigger("RightAttack");
            DealDamage(attackPoints.rightAttackPoint);
            driftDirection = new Vector3(1, 0, 0);
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            playerController.SetPlayerDirection(Direction.UpRight);

            playerController.anim.SetTrigger("UpRightAttack");
            DealDamage(attackPoints.upRightAttackPoint);
            driftDirection = new Vector3(0.7f, 0, 0.7f);
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            playerController.SetPlayerDirection(Direction.Up);

            playerController.anim.SetTrigger("UpAttack");
            DealDamage(attackPoints.upAttackPoint);
            driftDirection = new Vector3(0, 0, 1);
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            playerController.SetPlayerDirection(Direction.UpLeft);

            playerController.anim.SetTrigger("UpLeftAttack");
            DealDamage(attackPoints.upLeftAttackPoint);
            driftDirection = new Vector3(-0.7f, 0, 0.7f);
        }
        else if (angle >= -157.5f && angle < -112.5f)
        {
            playerController.SetPlayerDirection(Direction.DownLeft);

            playerController.anim.SetTrigger("DownLeftAttack");
            DealDamage(attackPoints.downLeftAttackPoint);
            driftDirection = new Vector3(-0.7f, 0, -0.7f);
        }
        else if (angle >= -112.5f && angle < -67.5f)
        {
            playerController.SetPlayerDirection(Direction.Down);

            playerController.anim.SetTrigger("DownAttack");
            DealDamage(attackPoints.downAttackPoint);
            driftDirection = new Vector3(0, 0, -1);
        }
        else if (angle >= -67.5f && angle < -22.5f)
        {
            playerController.SetPlayerDirection(Direction.DownRight);

            playerController.anim.SetTrigger("DownRightAttack");
            DealDamage(attackPoints.downRightAttackPoint);
            driftDirection = new Vector3(0.7f, 0, -0.7f);
        }
        else if (angle >= 157.5f || angle < -157.5f)
        {
            playerController.SetPlayerDirection(Direction.Left);

            playerController.anim.SetTrigger("LeftAttack");
            DealDamage(attackPoints.leftAttackPoint);
            driftDirection = new Vector3(-1, 0, 0);
        }
        StartCoroutine(Drift());
    }

    private void DealDamage(Transform attackPoint)
    {
        Collider[] hitDamagable = Physics.OverlapSphere(attackPoint.position, attackRadius, damagableLayers);

        foreach (var hit in hitDamagable)
        {
            Component damageComponent = hit.gameObject.GetComponent(typeof(IDamagable));
            if (damageComponent)
            {
                StartCoroutine(Hit(damageComponent));
            }
        }
    }

    IEnumerator Drift()
    {
        yield return new WaitForSeconds(0.3f);
        rb.velocity = driftDirection * driftSpeed;
    }

    IEnumerator Hit(Component damageComponent)
    {
        yield return new WaitForSeconds(damageDelay);
        if (damageComponent != null)
        {
            (damageComponent as IDamagable).TakeDamage(attackDamage);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoints.upAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(attackPoints.rightAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(attackPoints.downAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(attackPoints.leftAttackPoint.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoints.upLeftAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(attackPoints.upRightAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(attackPoints.downLeftAttackPoint.position, attackRadius);
        Gizmos.DrawWireSphere(attackPoints.downRightAttackPoint.position, attackRadius);
    }
}
