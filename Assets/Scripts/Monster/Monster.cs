using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;


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
    private Transform attackPoint;

    [SerializeField]
    private float attackAngle = 60f;

    private CharacterController monsterController;

    private MonsterState monsterState = MonsterState.Move; 
    private int currentMoveToPointIndex = 0;
    private Transform currentMoveToPoint;

    private Vector3 monsterVelocity;

    private bool reverse = false;

    private PlayerMovement playerMovement;

    private MonsterState currentMonsterState = MonsterState.Move;

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
        Debug.DrawRay(attackPoint.position, attackPoint.forward * 10f, Color.green);
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

        if (dinstance < 0.5f)
        {
            ChangePoint();
        }
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
        //if (distance >= )
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

        if (angle <= attackAngle)
        {
            currentMonsterState = MonsterState.Attack;
        }
    }
}