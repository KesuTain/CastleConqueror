using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MoveComponent))]
[RequireComponent(typeof(WarAttackComponent))]
public class WarLogicComponent : EntityComponent
{
    [Header("Parameters of WarLogic")]
    public float DistanceFindEnemy;
    [SerializeField]
    private NPCSystem ManageNPC;
    [SerializeField]
    private MoveComponent Motor;
    public GameObject Enemy;
    public enum State
    {
        GoToActPoint,
        FindEnemy,
        GoToEnemy,
        AttackEnemy,
        WinEnemy,
        Idle,
        Die
    }
    public State StateNow;
    void Start()
    {
        Motor = GetComponent<MoveComponent>();
        ManageNPC = GameObject.FindGameObjectWithTag("Manager").GetComponent<NPCSystem>();
        StateNow = State.Idle;
    }

    void Update()
    {
        if (GetComponent<WarriorEntity>().Alive)
        {
            switch (StateNow)
            {
                case State.Idle:
                    StateIdle();
                    break;
                case State.GoToActPoint:
                    StateGoToActPoint();
                    break;
                case State.FindEnemy:
                    StateFindEnemy();
                    break;
                case State.GoToEnemy:
                    //StateGoToEnemy();
                    break;
                case State.AttackEnemy:
                    //StateAttackEnemy();
                    break;
                case State.WinEnemy:
                    //StateWinEnemy();
                    break;
                case State.Die:
                    //StateDie();
                    break;
            }
        }
        else
        {
            //Die
        }
    }

    void StateIdle()
    {
        Motor.MoveTo(transform.position);
    }

    void StateGoToActPoint()
    {
        Motor.MoveTo(ManageNPC.PositionForMove());
        if (Vector3.Distance(transform.position, ManageNPC.PositionForMove()) <= DistanceFindEnemy)
        {
            StateNow = State.FindEnemy;
        }
    }
    void StateFindEnemy()
    {
        Patrol();
    }

    private Vector3 PatrolPoint;
    private NavMeshHit hit;
    void Patrol()
    {
        if (Vector3.Distance(transform.position, PatrolPoint) <= 2.5f)
        {
            do
            {
                float x = Random.Range(ManageNPC.PositionForMove().x - 20f, ManageNPC.PositionForMove().x + 20f);
                float y = 0;
                float z = Random.Range(ManageNPC.PositionForMove().z - 20f, ManageNPC.PositionForMove().z + 20f);
                PatrolPoint = new Vector3(x, y, z);
            }
            while (!NavMesh.SamplePosition(PatrolPoint, out hit, 100f, NavMesh.AllAreas));
        }
        Motor.MoveTo(PatrolPoint);
    }

    void StateGoToEnemy()
    {
        if (Enemy.Alive)
        {
            MoveTo(Enemy.transform.position);
            //Animation Mob_Fight_Walk
            if (Vector3.Distance(transform.position, Enemy.transform.position) <= 2.5f)
            {
                StateMob = State.AttackEnemy;
            }
        }
        else
        {
            StateMob = State.GoToActPoint;
        }
    }
}
