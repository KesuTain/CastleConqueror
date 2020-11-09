using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaComponent : MonoBehaviour
{
    [Header("Parametrs")]
    [SerializeField]
    private Mob Parent;
    public GameObject Target;
    void Start()
    {
        Parent = transform.parent.gameObject.GetComponent<Mob>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyArea") && other.transform.parent.GetComponent<Enemy>().Alive == true)
        {
            if (Parent.StateMob == Mob.State.GoToActPoint || Parent.StateMob == Mob.State.FindEnemy || Parent.StateMob == Mob.State.Idle)
            {
                Parent.Enemy = other.gameObject.transform.parent.gameObject.GetComponent<Enemy>();
                Parent.StateMob = Mob.State.GoToEnemy;
            }
        }
    }
}
