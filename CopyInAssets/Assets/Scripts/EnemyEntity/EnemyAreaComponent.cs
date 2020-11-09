using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaComponent : MonoBehaviour
{
    [Header("Parametrs")]
    [SerializeField]
    private Enemy Parent;
    public GameObject Target;
    void Start()
    {
        Parent = transform.parent.gameObject.GetComponent<Enemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MobArea") && other.transform.parent.GetComponent<Mob>().Alive == true)
        {
            if(Parent.StateEnemy == Enemy.State.GoToActPoint || Parent.StateEnemy == Enemy.State.FindMob || Parent.StateEnemy == Enemy.State.Idle)
            {
                Parent.Mob = other.gameObject.transform.parent.gameObject.GetComponent<Mob>();
                Parent.StateEnemy = Enemy.State.GoToMob;
            }
        }
    }
}
