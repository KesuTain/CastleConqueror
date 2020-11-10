using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WarLogicComponent))]
[RequireComponent(typeof(AreaComponent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NPC))]
public class WarriorEntity : Entity
{
    [Header("Parameter of Warrior")]
    public int Health;
    public int Damage;
    public float SpeedAttack;
    public float Scale;
    public float Speed;
    public bool Alive;
    public enum Fraction
    {
        Enemy,
        Friend,
        Neutral
    }

    public Fraction Frac; 

    
    void Start()
    {
        Alive = true;
        SetColorFraction();
    }

    void Update()
    {
        
    }

    public void GetDamage(int Dam)
    {
        if (Alive)
            Health -= Dam;
        if (Health <= 0)
            Die();
    }

    public void Die()
    {

    }

    #region Debug

    void SetColorFraction()
    {
        switch (Frac)
        {
            case Fraction.Enemy:
                transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
                break;
            case Fraction.Friend:
                transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
                break;
            case Fraction.Neutral:
                transform.GetChild(0).GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
    }

    #endregion
}
