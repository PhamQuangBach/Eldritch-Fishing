using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public enum MonsterState
{
    Move,
    Attack
}


public class Monster : MonoBehaviour
{
    [SerializeField]
    private List<Transform> moveToPoints = new();

    [SerializeField]
    private float monsterSpeed = 5f;

    [SerializeField]
    private float pointDistance = 0.5f;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackAngle = 60f;

    [SerializeField]
    private float attackDistance = 2f;

    [SerializeField]
    private float attackRestartTime = 5f;

    [SerializeField]
    private float safeDinstance = 18f;

    private CharacterController monsterController;

    private MonsterState currentMonsterState = MonsterState.Move;

    private int currentMoveToPointIndex = 0;
    private Transform currentMoveToPoint;
    private Vector3 monsterVelocity;
    private bool reverse = false;
    private bool canAttack = true;
    private PlayerMovement playerMovement;

    private void Start()
    {
        monsterController = GetComponent<CharacterController>();

        currentMoveToPoint = moveToPoints[currentMoveToPointIndex];
    }

    private void Update()
    {
        if (GameManager.IsPaused)
            return;

        switch (currentMonsterState)
        {
            case MonsterState.Move:
                PointMovement();
                break;
            case MonsterState.Attack:
                AttackMovement();
                break;
        }

        monsterController.Move(monsterVelocity * Time.deltaTime);

        LookAtPlayer();

        Debug.DrawRay(transform.position, monsterVelocity, Color.red);
        Debug.DrawRay(attackPoint.position, attackPoint.forward * safeDinstance, Color.green);
    }

    private Vector3 GetDirectcion()
    {
        Vector3 direction = currentMoveToPoint.position - transform.position;
        direction.Normalize();
        direction.y = 0;

        return direction;
    }

    private float DistanceToMoveToPoint()
    {
        return Vector3.Distance(transform.position, currentMoveToPoint.position);
    }

    private void ChangePoint()
    {
        if (reverse)
        {
            currentMoveToPointIndex--;
        }
        else
        {
            currentMoveToPointIndex++;
        }

        if (currentMoveToPointIndex == 0 || currentMoveToPointIndex == moveToPoints.Count - 1)
        {
            reverse = !reverse;
        }

        currentMoveToPoint = moveToPoints[currentMoveToPointIndex];
    }

    private void PointMovement()
    {
        Vector3 wishDir = GetDirectcion();

        monsterVelocity = Vector3.Lerp(monsterVelocity, wishDir * monsterSpeed, Time.deltaTime);

        float dinstance = DistanceToMoveToPoint();

        if (dinstance < pointDistance)
        {
            ChangePoint();
        }
    }

    private IEnumerator restartAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackRestartTime);
        canAttack = true;
    }

    private void AttackMovement()
    {
        Vector3 monsterPos = transform.position;
        Vector3 playerPos = playerMovement.transform.position;

        Vector3 wishDir = playerPos - monsterPos;
        wishDir.Normalize();
        wishDir.y = 0;

        monsterVelocity = Vector3.Lerp(monsterVelocity, wishDir * monsterSpeed, Time.deltaTime);

        float distance = Vector3.Distance(monsterPos, playerPos);

        Debug.Log(distance);
        if (distance >= 20f)
        {
            currentMonsterState = MonsterState.Move;
            StartCoroutine(restartAttack());
        }
    }

    private void LookAtPlayer()
    {
        if (playerMovement == null)
        {
            playerMovement = PlayerMovement.instance;
            return;
        }
            

        attackPoint.LookAt(playerMovement.transform);

        float angle = Vector3.Angle(monsterVelocity.normalized, attackPoint.forward);

        if (angle <= attackAngle && canAttack)
        {
            currentMonsterState = MonsterState.Attack;
        }
    }
}