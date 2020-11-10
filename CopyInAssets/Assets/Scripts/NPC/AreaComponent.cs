using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AreaComponent : EntityComponent
{

    [Header("Parameters of AreaView")]
    [SerializeField]
    private SphereCollider AreaView;
    [SerializeField]
    private WarriorEntity Parent;
    public float RadiusView;
    void Start()
    {
        AreaView = GetComponent<SphereCollider>();
        AreaView.isTrigger = true;
        AreaView.radius = RadiusView;
    }

    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Warrior") && other.transform.parent.GetComponent<WarriorEntity>().Frac != GetComponent<WarriorEntity>().Frac && other.transform.parent.GetComponent<WarriorEntity>().Alive)
        {
            GetComponent<WarLogicComponent>().Enemy = other.gameObject;
        }
    }
}
