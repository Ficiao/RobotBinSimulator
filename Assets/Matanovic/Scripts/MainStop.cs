using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainStop : Stop
{
    public GameObject collided;
    // Start is called before the first frame update
    void Start()
    {
        full = 0; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Cube"))
        {
            collided = other;
            full = 1;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Cube"))
        {
            full = 0;
        }
    }
}
