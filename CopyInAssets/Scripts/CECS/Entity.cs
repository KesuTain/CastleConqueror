using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Базовый класс сущности
public class Entity : MonoBehaviour
{
    [Header("ECS")]
    public List<EntityComponent> Components;
    
    public virtual void Start()
    {
        Components = new List<EntityComponent>(GetComponentsInChildren<EntityComponent>());
    }
    
    void Update()
    {
        
    }

    //Получает компонент определенного типа
    public EntityComponent GetComponentOfType<T>()
    {
        EntityComponent component = null;
        foreach (EntityComponent ec in Components)
        {
            if (ec.GetType().Equals(typeof(T)))
            {
                component = ec;
            }
        }
        return component;
    }

    public List<EntityComponent> GetComponentsOfType<T>()
    {
        List<EntityComponent> components = new List<EntityComponent>();
        foreach (EntityComponent ec in Components)
        {
            if (ec.GetType().Equals(typeof(T)))
            {
                components.Add(ec);
            }
        }
        return components;
    }
}
