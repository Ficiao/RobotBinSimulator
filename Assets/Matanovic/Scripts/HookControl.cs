using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HookControl : MonoBehaviour
{
    private Transform MoveHook;
    [SerializeField]
    private MainStop checkpoint;
    private enum State { GetNext, Pickup, Calculate, MoveToBox, PutDown, MoveBack, Up, Pause, RotateL, RotateR, BoxMove, BoxCreate}
    [SerializeField]
    private State state;
    private Transform PickUpCube;
    private Vector3 CubePickUpPosition;
    [SerializeField]
    private Transform HookPivot;
    private bool isCubeAttached;
    private Vector3 UpPosition;
    [SerializeField]
    private Transform robot;
    private Vector3 point1;
    private Vector3 point0;
    [SerializeField]
    private Transform box;
    private Vector3 downPosition;
    private int[,,] matrica;
    public int toRotate;
    private ArrayList boxes;
    private Transform boxPosition;
    [SerializeField]
    private GameObject spawnee;
    [SerializeField]
    private Transform pozicija;

    void Start()
    {
        boxes = new ArrayList();
        boxes.Add(box);
        boxPosition = box.transform;
        toRotate = 0;
        MoveHook = transform.parent;
        state = State.GetNext;
        point1 = new Vector3(0, 0, 7.83f);
        point0 = new Vector3(robot.transform.position.x, robot.transform.position.y, robot.transform.position.z);
        matrica = new int[3,5,6];
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < matrica.GetLength(1); j++)
            {
                for(int k = 0; k < matrica.GetLength(2); k++)
                {
                    matrica[i, j, k] = -1;
                }
            }
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.BoxMove:
                foreach(Transform kek in boxes){
                    kek.GetComponent<BoxMover>().Moving();
                }
                state = State.BoxCreate;
                break;

            case State.BoxCreate:
                int detektor = 0;
                foreach (Transform kek in boxes)
                {
                    if(kek.GetComponent<BoxMover>().move==1)detektor=1;
                }
                if (detektor == 0)
                {
                    box=Instantiate(spawnee).transform;
                    boxes.Add(box);
                    pozicija = box.Find("Pozicija").gameObject.transform;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < matrica.GetLength(1); j++)
                        {
                            for (int k = 0; k < matrica.GetLength(2); k++)
                            {
                                matrica[i, j, k] = -1;
                            }
                        }
                    }
                    state = State.Calculate;
                }
                break;

            case State.GetNext:
                if (checkpoint.full == 1)
                {
                    PickUp(checkpoint.collided.transform);
                }
                break;

            case State.Pickup:
                if (Vector3.Distance(MoveHook.position, CubePickUpPosition) > 0.0001f)
                {
                    MoveHook.position = Vector3.MoveTowards(MoveHook.position, CubePickUpPosition, Time.deltaTime * 6);
                }
                else
                {
                    PickUpCube.parent = HookPivot;
                    isCubeAttached = true;
                    state = State.Up;
                }
                break;

            case State.Up:
                if (Vector3.Distance(MoveHook.position, UpPosition) > 0.0001f)
                {
                    MoveHook.position = Vector3.MoveTowards(MoveHook.position, UpPosition, Time.deltaTime * 8);
                }
                else
                {
                    if (isCubeAttached)
                    {
                        state = State.Calculate;
                    }
                    else
                    {
                        if (toRotate != 0)
                        {
                            state = State.RotateR;
                            toRotate = 0;
                        }
                        else state = State.MoveBack;
                    }
                }
                break;

            case State.Calculate:
                Calculate();
                break;

            case State.MoveToBox:
                if (Vector3.Distance(robot.position, point1) > 0.0001f)
                {
                    robot.position = Vector3.MoveTowards(robot.position, point1, Time.deltaTime * 10);
                }
                else
                {
                    UpPosition = MoveHook.position;
                    if (toRotate != 0) state = State.RotateL;
                    else state = State.PutDown;                    
                }
                break;

            case State.RotateL:
                if (Quaternion.Angle(MoveHook.rotation, Quaternion.Euler(0, 90, 0)) > 0.0001f)
                {
                    MoveHook.rotation = Quaternion.RotateTowards(MoveHook.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * 175);
                }
                else
                {
                    state = State.PutDown;
                }
                break;

            case State.PutDown:
                if (Vector3.Distance(MoveHook.position, downPosition) > 0.0001f)
                {
                    MoveHook.position = Vector3.MoveTowards(MoveHook.position, downPosition, Time.deltaTime * 10);
                }
                else{
                    PickUpCube.parent = box;
                    isCubeAttached = false;
                    state = State.Up;
                }
                break;

            case State.RotateR:
                if (Quaternion.Angle(MoveHook.rotation, Quaternion.Euler(0, 0, 0)) > 0.0001f)
                {
                    MoveHook.rotation = Quaternion.RotateTowards(MoveHook.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 175);
                }
                else
                {
                    state = State.MoveBack;
                }
                break;

            case State.MoveBack:
                if (Vector3.Distance(robot.position, point0) > 0.0001f)
                {
                    robot.position = Vector3.MoveTowards(robot.position, point0, Time.deltaTime * 10);
                }
                else
                {
                    UpPosition = MoveHook.position;
                    state = State.GetNext;
                }
                break;
            case State.Pause:                
                break;
        }
    }

    private void Calculate()
    {
        Vector3 lokacija=new Vector3();
        int klasa = PickUpCube.GetComponent<CubeSpawnProp>().klasa;
        switch (klasa)
        {
            case 0:
                lokacija = Rjesavaci.m1x1(matrica, PickUpCube, pozicija);
                break;
            case 1:
            case 3:
                lokacija = Rjesavaci.m1x2(matrica, PickUpCube, klasa, this, pozicija);
                break;
            case 2:
            case 6:
                lokacija = Rjesavaci.m1x3(matrica, PickUpCube, klasa, this, pozicija);
                break;
            case 4:
                lokacija = Rjesavaci.m2x2(matrica, PickUpCube, pozicija);
                break;
            case 5:
            case 7:
                lokacija = Rjesavaci.m2x2(matrica, PickUpCube, pozicija);
                break;
        }
        if (lokacija.x == 0 && lokacija.y == 0 && lokacija.z == 0)
        {
            state = State.BoxMove;
        }
        else
        {
            point1 = new Vector3(robot.position.x, robot.position.y, lokacija.z);
            downPosition = new Vector3(lokacija.x, lokacija.y + 1.15f, lokacija.z);
            state = State.MoveToBox;
        }
    }



    public void PickUp(Transform pick)
    {
        UpPosition = MoveHook.position;
        PickUpCube = pick;
        CubePickUpPosition = new Vector3(PickUpCube.position.x, PickUpCube.position.y + 1.15f, PickUpCube.position.z);
        state = State.Pickup;
    }


}
