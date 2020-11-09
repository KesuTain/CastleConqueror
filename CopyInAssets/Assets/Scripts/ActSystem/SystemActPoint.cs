using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemActPoint : MonoBehaviour
{
    [Header("Points")]
    public List<GameObject> Points;
    public int NowState;
    void Start()
    {
        NowState = 0;
    }

    void Update()
    {
        
    }

    public Vector3 PositionForMove()
    {
        return Points[NowState].transform.position;
    }
}
