using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    #region Singleton Preset
    //Change instance in inherited classes
    public static EntitySystem instance;

    public void Awake()
    {
        instance = this;
    }

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
