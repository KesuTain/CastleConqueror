using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    private Rigidbody Rigid;
    [SerializeField]
    private SystemActPoint SystemActPoints;
    [Space]


    [Header("Parametrs")]
    public int Health;
    public int Damage;
    public float SpeedAttack;
    public float DistanceFindMob;
    public bool Alive;
    public float TimeForDeath;
    [Space]


    [Header("Logic Components")]
    public Vector3 PositionMove;
    public enum State
    {
        GoToActPoint,
        FindMob,
        GoToMob,
        AttackMob,
        WinMob,
        Idle,
        Die
    }
    public State StateEnemy;
    public Mob Mob;
    [SerializeField]
    private bool CanAttack;
    [SerializeField]
    private NavMeshHit hit;

    void Start()
    {
        StartSettings();
        StateEnemy = State.Idle;
    }
    void FixedUpdate()
    {
        if (Alive)
        {
            switch (StateEnemy)
            {
                case State.Idle:
                    StateIdle();
                    break;
                case State.GoToActPoint:
                    StateGoToActPoint();
                    break;
                case State.FindMob:
                    StateFindMob();
                    break;
                case State.GoToMob:
                    StateGoToMob();
                    break;
                case State.AttackMob:
                    StateAttackMob();
                    break;
                case State.WinMob:
                    StateWinMob();
                    break;
                case State.Die:
                    StateDie();
                    break;
            }
        }
        else
        {
            //Die
        }
    }
    void StartSettings()
    {
        Alive = true;
        CanAttack = true;

        Rigid = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();
        SystemActPoints = GameObject.FindGameObjectWithTag("SystemActPoint").GetComponent<SystemActPoint>();

        Rigid.isKinematic = true;
        Rigid.useGravity = false;
        Agent.speed = 7f;
        Agent.angularSpeed = 1000f;
        Agent.acceleration = 100f;
        Agent.stoppingDistance = 2f;
    }

    public void MoveTo(Vector3 Pos)
    {
        Agent.SetDestination(Pos);
    }

    void StateGoToActPoint()
    {
        Agent.speed = 7f;
        MoveTo(SystemActPoints.PositionForMove());
        //Animation Enemy_Walk
        if (Vector3.Distance(transform.position, SystemActPoints.PositionForMove()) <= DistanceFindMob)
        {
            StateEnemy = State.FindMob;
        }
    }

    void StateFindMob()
    {
        Patrol();
    }

    private Vector3 PatrolPoint;
    void Patrol()
    {
        Agent.speed = 3f;
        if (Vector3.Distance(transform.position, PatrolPoint) <= 2.5f)
        {
            do
            {
                float x = Random.Range(SystemActPoints.PositionForMove().x - 20f, SystemActPoints.PositionForMove().x + 20f);
                float y = 0;
                float z = Random.Range(SystemActPoints.PositionForMove().z - 20f, SystemActPoints.PositionForMove().z + 20f);
                PatrolPoint = new Vector3(x, y, z);
            }
            while (!NavMesh.SamplePosition(PatrolPoint, out hit, 100f, NavMesh.AllAreas));
            Debug.Log("Pos for Patrol = " + PatrolPoint);
        }
        MoveTo(PatrolPoint);
    }

    void StateGoToMob()
    {
        Agent.speed = 4f;
        if (Mob.Alive)
        {
            MoveTo(Mob.transform.position);
            //Animation Enemy_Fight_Walk
            if (Vector3.Distance(transform.position, Mob.transform.position) <= 2.5f)
            {
                StateEnemy = State.AttackMob;
            }
        }
        else
        {
            StateEnemy = State.GoToActPoint;
        }
    }

    void StateAttackMob()
    {
        if (CanAttack && Mob.Alive)
            StartCoroutine(Attack());
        if (!Mob.Alive || Vector3.Distance(transform.position, Mob.transform.position) >= 3f)
            StateEnemy = State.GoToActPoint;
    }

    IEnumerator Attack()
    {
        CanAttack = false;
        //Animation Enemy_Fight
        Mob.MinesHealth(Damage);
        Debug.Log("Attack!");
        yield return new WaitForSeconds(SpeedAttack);
        CanAttack = true;
    }

    void StateWinMob()
    {

    }
    void StateIdle()
    {
        MoveTo(transform.position);
        //Animation Enemy_Idle
    }
    private bool CanDie = true;
    void StateDie()
    {
        Alive = false;
        if (CanDie)
            StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        CanDie = false;
        //Animation Enemy_Die
        yield return new WaitForSeconds(TimeForDeath);
        Destroy(gameObject);
    }

    public void MinesHealth(int GetDamage)
    {
        Health -= GetDamage;
        if (Health <= 0)
        {
            StateEnemy = State.Die;
        }
    }
}
