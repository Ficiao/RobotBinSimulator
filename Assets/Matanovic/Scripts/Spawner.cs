using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnee;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    [SerializeField]
    private GameObject gate;
    private int shouldSpawn = 0;
    [SerializeField]
    private GameObject parent;


    private void Start()
    {
        shouldSpawn = 1;
    }

    private void FixedUpdate()
    {
        if (gate.GetComponent<Stop>().full == 0&&shouldSpawn==1)
        {
            InvokeRepeating("SpawnObject", spawnTime + 0.2f, 3f);
            shouldSpawn = 0;
        }

    }

    public void SpawnObject()
    {
        if (gate.GetComponent<Stop>().full == 0)
        {
            Instantiate(spawnee, new Vector3(-5.59f, 2.357702f, -9.910031f), this.transform.rotation,parent.transform);
        }
        else
        {
            CancelInvoke("SpawnObject");
            shouldSpawn = 1;
        }
    }

}
