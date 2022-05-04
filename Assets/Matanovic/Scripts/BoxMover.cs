using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BoxMoved();

public class BoxMover : MonoBehaviour
{
    [SerializeField]
    private GameObject hook;
    public int move;
    private Vector3 dest;

    void Start()
    {
        move = 0;
    }

    private void Update()
    {
        if (move == 1)
        {
            if (Vector3.Distance(this.transform.position, dest) > 0.0001f)
            {
               this.transform.position = Vector3.MoveTowards(this.transform.position, dest, Time.deltaTime * 6);
            }
            else
            {
                move = 0;
            }
        }
    }

    public void Moving()
    {
        dest = this.transform.position;
        dest.z = dest.z + 5;
        move = 1;
    }
}
