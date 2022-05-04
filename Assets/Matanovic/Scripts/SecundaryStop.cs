using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecundaryStop : Stop
{
    public int info;
    [SerializeField]
    private Stop gate;

    // Start is called before the first frame update
    void Start()
    {
        full = 0;
        this.GetComponent<Collider>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        info = gate.GetComponent<Stop>().full;
        if (info == 1)
        {
            this.GetComponent<Collider>().enabled = true;
        }
        else
        {
            this.full = 0;
            this.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Cube"))
        {
            full = 1;
        }
    }
}
