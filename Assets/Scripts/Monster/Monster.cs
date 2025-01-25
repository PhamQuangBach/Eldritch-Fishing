using UnityEngine;
using System.Collections.Generic;


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

    private CharacterController monsterController;

    private MonsterState monsterState = MonsterState.Move; 
    private int currentMoveToPointIndex = 0;
    private Transform currentMoveToPoint;

    private Vector3 monsterVelocity;

    private void Start()
    {
        monsterController = GetComponent<CharacterController>();

        currentMoveToPoint = moveToPoints[currentMoveToPointIndex];
    }

    private void Update()
    {
        if (GameManager.IsPaused)
            return;

        Movement();

        Debug.DrawRay(transform.position, monsterVelocity, Color.red);
    }

    private Vector3 GetDirectcion()
    {
        Vector3 direction = currentMoveToPoint.position - transform.position;
        direction.Normalize();
        direction.y = 0;

        return direction;
    }

    private void Movement()
    {
        Vector3 wishDir = GetDirectcion();

        monsterVelocity = wishDir * monsterSpeed;
    }
}