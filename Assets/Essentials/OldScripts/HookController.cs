using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookController : MonoBehaviour
{

    //Raycast
    private RaycastHit HitInfo;
    private Ray LandingRay;

    //Movement
    private Transform MoveHook;
    private string State;
    private bool scan;
    private Vector3 StartPosition = new Vector3(-1.5f, 4.5f, 0);
    private Vector3 ScanPosition = new Vector3(-5.8f, 4.5f, 0);
    private Vector3 PoprecneScanPosition = new Vector3(-3, 4.5f, 0);
    public Vector3 MoveToward;
    private Transform PickUpPlank;
    private Vector3 UpPosition;
    private bool isPlankAttached;
    private Vector3 PlankDetachPosition;
    private Vector3 PlankPickUpPosition;

    //Scanned values
    public Transform PlankToPick;
    public Transform PoprecnaToPick;
    public Transform Platform;
    private float plankY;
    private float poprecnaY;
    private float platformY;
    private float hitY;

    //StateMachine
    public GameObject StateMachine;
    private string StateScanForward;
    public Transform HookPivot;

    // Start is called before the first frame update
    void Start()
    {
        plankY = 0f;
        poprecnaY = 0f;
        platformY = 0f;
        MoveHook = transform.parent;
      //MoveHook = HookPivot;
    }

    // Update is called once per frame
    void Update()
    {

        //Scan planks
        if (scan)
        {
            LandingRay = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(LandingRay, out HitInfo, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * HitInfo.distance, Color.yellow);
                hitY = HitInfo.transform.position.y;

                //Which to pick next
                if (HitInfo.transform.tag == "Plank" && hitY > plankY)
                {
                    plankY = HitInfo.transform.position.y;
                    PlankToPick = HitInfo.transform;
                }
                else if (HitInfo.transform.tag == "PlankP" && hitY > poprecnaY)
                {
                    poprecnaY = HitInfo.transform.position.y;
                    PoprecnaToPick = HitInfo.transform;
                }

                if (HitInfo.transform.tag == "StartPlatform")
                {
                    platformY = hitY;
                    if (platformY > plankY && platformY > poprecnaY)
                    {
                        PlankToPick = Platform;
                        PoprecnaToPick = Platform;
                    }
                }
            }
        }

        //Move hook to start position
        if (State == "StartScan")
        {
            ScanForward();
        }

        //Move hook to given position
 /*       if (State == "MoveToward")
        {
            if (Vector3.Distance(MoveHook.position, MoveToward) > 0.01f)
            {
                MoveHook.position = Vector3.MoveTowards(MoveHook.position, MoveToward, Time.deltaTime * 6);
            }
            else
            {
                State = "Done";
            }
        }
*/
        //Pick up given plank
        if(State == "PickUp")
        {
            if (Vector3.Distance(MoveHook.position, PickUpPlank.position) > 1)
            {
                Debug.Log("Distance = " + Vector3.Distance(MoveHook.position, PickUpPlank.position));
                MoveHook.position = Vector3.MoveTowards(MoveHook.position, PlankPickUpPosition, Time.deltaTime * 6);
            }
            else
            {

                Debug.Log("ovo testirati");
                PickUpPlank.parent = HookPivot;
                PickUpPlank.transform.localPosition = Vector3.zero;
             //   PickUpPlank.transform.localRotation = Quaternion.identity;
                isPlankAttached = true;
                State = "Up";
            }
        }

        //Return up after picking up plank
        if(State == "Up")
        {
            if (Vector3.Distance(MoveHook.position, UpPosition) > 0.0001f)
            {
                MoveHook.position = Vector3.MoveTowards(MoveHook.position, UpPosition, Time.deltaTime * 6);
            }
            else
            {
                State = "Done";
                if (StateMachine.gameObject.GetComponent<StateMachine>().GetPoprecnaState() == "WaitPickUp")
                {
                    if (isPlankAttached)
                    {
                        StateMachine.gameObject.GetComponent<StateMachine>().SetPoprecnaState("PickUpDone");
                    }
                    else
                    {
                        StateMachine.gameObject.GetComponent<StateMachine>().SetPoprecnaState("DetachementDone");
                    }
                }
                else if (StateMachine.gameObject.GetComponent<StateMachine>().GetPoprecnaToPlatformState() == "WaitPickUp")
                {
                    if (isPlankAttached)
                    {
                        StateMachine.gameObject.GetComponent<StateMachine>().SetPoprecnaToPlatformState("PickUpDone");
                    }
                    else
                    {
                        StateMachine.gameObject.GetComponent<StateMachine>().SetPoprecnaToPlatformState("DetachementDone");
                    }
                }
                else
                {
                    if (isPlankAttached)
                    {
                        StateMachine.gameObject.GetComponent<StateMachine>().SetState("PickUpDone");
                    }
                    else
                    {
                        StateMachine.gameObject.GetComponent<StateMachine>().SetState("DetachementDone");
                    }
                }
            }
        }

        //Detach plank on given position
        if (State == "Detach")
        {
            if (Vector3.Distance(MoveHook.position, PlankDetachPosition) > 0.0001f)
            {
                MoveHook.position = Vector3.MoveTowards(MoveHook.position, PlankDetachPosition, Time.deltaTime * 6);
            }
            else
            {
                PickUpPlank.parent = null;
                isPlankAttached = false;
                State = "Up";
            }
        }

    }


    private void ScanForward()
    {
        if (StateScanForward == "StartScan")
        {
            if (Vector3.Distance(MoveHook.position, StartPosition) != 0)
            {
                MoveHook.position = Vector3.MoveTowards(MoveHook.position, StartPosition, Time.deltaTime * 6f);
            }
            else
            {
                scan = true;
                StateScanForward = "MoveForward";
            }
        }

        //Move hook to scan position
        if (StateScanForward == "MoveForward")
        {
            if (Vector3.Distance(MoveHook.position, ScanPosition) > 0.0001f)
            {
                MoveHook.position = Vector3.MoveTowards(MoveHook.position, ScanPosition, Time.deltaTime * 6f);
            }
            else
            {
                StateScanForward = "MoveBack";
            }
        }

        //Move hook to back to position for across scan
        if (StateScanForward == "MoveBack")
        {
            if (Vector3.Distance(MoveHook.position, PoprecneScanPosition) > 0.0001f)
            {
                MoveHook.position = Vector3.MoveTowards(MoveHook.position, PoprecneScanPosition, Time.deltaTime * 6f);
            }
            else
            {
                State = "Done";
                StateMachine.gameObject.GetComponent<StateMachine>().SetState("ForwardScanDone");
            }
        }
    }

    public void PickUp(Transform pick)
    {
        UpPosition = MoveHook.position;
        PickUpPlank = pick;
        PlankPickUpPosition = new Vector3(PickUpPlank.position.x, PickUpPlank.position.y + 0.9600004f, PickUpPlank.position.z);
        State = "PickUp";
    }

    public void DetachPlank(Vector3 pos)
    {
        plankY = 0f;
        poprecnaY = 0f;
        platformY = 0f; 
        UpPosition = MoveHook.position;
        PlankDetachPosition = pos;
        State = "Detach";
    }

    public void ResetPlanks()
    {
        PlankToPick = Platform;
        PoprecnaToPick = Platform;
    }

    public void SetStateScanForward(string s)
    {
        StateScanForward = s;
    }

    public void SetState(string s)
    {
        State = s;
    }

    public string GetState()
    {
        return State;
    }
    
    public void SetScan(bool s)
    {
        scan = s;
    }

    public bool PlankAttached()
    {
        return isPlankAttached;
    }

    public void DebugF()
    {
        Debug.Log("Hook script working");
    }
}
