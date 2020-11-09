using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class Mob : MonoBehaviour
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
    public float DistanceFindEnemy;
    public bool Alive;
    public float TimeForDeath;
    [Space]


    [Header("Logic Components")]
    public Vector3 PositionMove;
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
    public State StateMob;
    public Enemy Enemy;
    [SerializeField]
    private bool CanAttack;
    [SerializeField]
    private NavMeshHit hit;

    void Start()
    {
        StartSettings();
        StateMob = State.Idle;
    }
    void FixedUpdate()
    {
        if (Alive)
        {
            switch (StateMob)
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
                    StateGoToEnemy();
                    break;
                case State.AttackEnemy:
                    StateAttackEnemy();
                    break;
                case State.WinEnemy:
                    StateWinEnemy();
                    break;
                case State.Die:
                    StateDie();
                    break;
            }
        } else
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
        //Animation Mob_Walk
        if (Vector3.Distance(transform.position, SystemActPoints.PositionForMove()) <= DistanceFindEnemy)
        {
            StateMob = State.FindEnemy;
        }
    }

    void StateFindEnemy()
    {
        Patrol();
    }

    private Vector3 PatrolPoint;
    void Patrol()
    {
        Agent.speed = 3f;
        if(Vector3.Distance(transform.position, PatrolPoint) <= 2.5f)
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

    void StateGoToEnemy()
    {
        Agent.speed = 4f;
        if (Enemy.Alive)
        {
            MoveTo(Enemy.transform.position);
            //Animation Mob_Fight_Walk
            if(Vector3.Distance(transform.position, Enemy.transform.position) <= 2.5f)
            {
                StateMob = State.AttackEnemy;
            }
        } else
        {
            StateMob = State.GoToActPoint;
        }
    }

    void StateAttackEnemy()
    {
        if (CanAttack && Enemy.Alive)
            StartCoroutine(Attack());
        if (!Enemy.Alive || Vector3.Distance(transform.position, Enemy.transform.position) >= 3f)
            StateMob = State.GoToActPoint;
    }

    IEnumerator Attack()
    {
        CanAttack = false;
        //Animation Mob_Fight
        Enemy.MinesHealth(Damage);
        Debug.Log("Attack!");
        yield return new WaitForSeconds(SpeedAttack);
        CanAttack = true;
    }

    void StateWinEnemy()
    {

    }
    void StateIdle()
    {
        MoveTo(transform.position);
        //Animation Mob_Idle
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
        //Animation Mob_Die
        yield return new WaitForSeconds(TimeForDeath);
        Destroy(gameObject);
    }
   
    public void MinesHealth(int GetDamage)
    {
        Health -= GetDamage;
        if(Health <= 0)
        {
            StateMob = State.Die;
        }
    }
}
