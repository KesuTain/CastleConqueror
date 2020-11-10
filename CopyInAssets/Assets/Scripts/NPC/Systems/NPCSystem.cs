using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSystem : MonoBehaviour
{
    [Header("Parameters of NPCSystem")]
    [SerializeField]
    private List<GameObject> Points;
    [SerializeField]
    private int NowState;
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
