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
    private float attackDistance = 3f;

    [SerializeField]
    private float attackRestartTime = 5f;

    [SerializeField]
    private float safeDinstance = 18f;

    [SerializeField]
    private Transform monsterBody;

    public static Monster instance;

    private CharacterController monsterController;
    private AudioSource monsterAudio;

    private MonsterState currentMonsterState = MonsterState.Move;

    private int currentMoveToPointIndex = 0;
    private Transform currentMoveToPoint;
    private Vector3 monsterVelocity;
    private bool reverse = false;
    private bool canAttack = true;
    private PlayerMovement playerMovement;

    private bool monsterIsAlive = true;

    private void Start()
    {
        monsterController = GetComponent<CharacterController>();
        monsterAudio = GetComponent<AudioSource>();

        currentMoveToPoint = moveToPoints[currentMoveToPointIndex];

        instance = this;
    }

    private void Update()
    {
        if (GameManager.IsPaused)
            return;

        if (!monsterIsAlive)
            return;

        /// GIRLS DON'T REMOVE THIS SWITCH, THE AI WON'T WORK WITHOUT IT
        switch (currentMonsterState)
        {
            case MonsterState.Move:
                PointMovement();
                break;
            case MonsterState.Attack:
                AttackMovement();
                break;
        }

        float angle = Mathf.Atan2(monsterVelocity.x, monsterVelocity.z) * Mathf.Rad2Deg;

        monsterBody.localEulerAngles = new Vector3(0, angle, 0);

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

        if (distance <= attackDistance)
        {
            GameManager.instance.SetGameState(GameState.Death);
            playerMovement.Kill(transform.position);
            
            StopMovement();


            return;
        }

        if (distance >= safeDinstance)
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

        float distance = Vector3.Distance(transform.position, playerMovement.transform.position);

        if (angle <= attackAngle && canAttack && distance <= safeDinstance)
        {
            currentMonsterState = MonsterState.Attack;
            //monsterAudio.Play();
        }
    }
    
    public void StopMovement()
    {
        monsterIsAlive = false;
    }
}