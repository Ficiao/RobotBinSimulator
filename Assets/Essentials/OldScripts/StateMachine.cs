using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
    public GameObject Menu;
    private bool begin = false;
    private string State;
    private string poprecnaState;
    private string poprecnaToPlatformState;

    //Given values
    public Transform DropdownBroj;
    public Transform DropdownDuljina;
    private int brojPoRedu;
    private float duljinaDaske;

    //Hook related variables
    public GameObject Hook;
    private HookController HookScript;
    private Transform PlankToPick;

    //Robot movement variables
    public GameObject RobotMovement;
    private RobotMovementController MovementScript;

    //Platforms info
    //New
    private int NewPlatformCurrentPlankCount;
    public int NewPlatformPoprecneCount;
    private Vector3 NextPositionNewPlatform;
    private Vector3 NextPoprecnaNewPlatform;
    //Garbage
    private int GarbagePlatformCurrentPlankCount;
    public int GarbagePlatformPoprecneCount;
    private Vector3 NextPositionGarbagePlatform;
    private Vector3 NextPoprecnaGarbagePlatform;
    //Poprecne
    private Transform NextToPickPoprecnePlatform;
    private Vector3 NextToDetachPoprecnePlatform;
    public Transform[] Poprecne = new Transform[6];
    private List<Transform> PoprecneList;
    private int brojPoprecnih;
    private Vector3 PoprecnaPosition;
    private int waitPoprecna;
    private bool skipToScan;
    //Where to put plank
    private Vector3 PlankDetachPosition;
    private int PlankDetachPlatformWaypoint;


    public void initRobot()
    {
        Debug.Log("Button clicked");
        begin = true;
        State = "Scan";

        brojPoRedu = DropdownBroj.GetComponent<Dropdown>().value + 2;
        duljinaDaske = DropdownDuljina.GetComponent<Dropdown>().value;
        if(duljinaDaske == 0)
        {
            duljinaDaske = 6;
        }
        else if(duljinaDaske == 1)
        {
            duljinaDaske = 6.5f;
        }
        else if (duljinaDaske == 2)
        {
            duljinaDaske = 7;
        }
        Menu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        HookScript = Hook.gameObject.GetComponent<HookController>();
        MovementScript = RobotMovement.gameObject.GetComponent<RobotMovementController>();

        HookScript.DebugF();
        MovementScript.DebugF();

        PoprecneList = new List<Transform>();
        for(int i = 0; i < 6; i++)
        {
            PoprecneList.Insert(i, Poprecne[i]);
        }
        NextToPickPoprecnePlatform = PoprecneList[0];
        brojPoprecnih = 6;

        NewPlatformCurrentPlankCount = 0;
        NewPlatformPoprecneCount = 2;
        GarbagePlatformCurrentPlankCount = 0;
        GarbagePlatformPoprecneCount = 2;

        NextPositionGarbagePlatform = new Vector3(10, 1.5f, 15);
        NextPositionNewPlatform = new Vector3(25.5f, 1.5f, 0);
        NextPoprecnaNewPlatform = new Vector3(24, 1.8f, 2);
        NextPoprecnaGarbagePlatform = new Vector3(8, 1.8f, 13.5f);

        NextToDetachPoprecnePlatform = new Vector3(10.5f, 0.6f, -13);

    poprecnaState = "Start";
        poprecnaToPlatformState = "Start";
    }

    // Update is called once per frame
    void Update()
    {
        //Robot work enabled
        if (begin)
        {
            Debug.Log("MovementScript " + MovementScript.GetState() + " State: " + State + " Hook: " + HookScript.GetState());
            //Scan forward
            if (State == "Scan")
            {
                HookScript.SetStateScanForward("StartScan");
                HookScript.SetState("StartScan");
                State = "Wait";
            }

            //Scan across
            if (State == "ForwardScanDone")
            {
                MovementScript.SetMovementState("MoveLeft");
                MovementScript.SetState("ScanAcross");
                State = "Wait";
            }

            //Find whick plank to pick
            if (MovementScript.GetState() == "ScanAcrossDone")
            {
                HookScript.SetScan(false);
                if (HookScript.PlankToPick.tag == "StartPlatform")
                {
                    State = "Finish";
                    return;
                }
                    if (HookScript.PlankToPick.position.y > HookScript.PoprecnaToPick.position.y)
                {
                    PlankToPick = HookScript.PlankToPick;
                }
                else
                {
                    PlankToPick = HookScript.PoprecnaToPick;
                }

                HookScript.ResetPlanks();
                MovementScript.SetState("Wait");
                State = "Calculate";
                return;

            }

            //Calcuclate next action depending on PlankToPick (before picking it up)
            if (State == "Calculate")
            {


                //New platform
                if (PlankToPick.localScale.z == duljinaDaske && PlankToPick.tag == "Plank")
                {
                    Debug.Log("first if: " + PlankToPick.localScale.z + "tag: " + PlankToPick.tag);
                    if (NewPlatformPoprecneCount == 2)
                    {
                        if (NewPlatformCurrentPlankCount < brojPoRedu)
                        {
                            //Set to pick it up
                           MovementScript.SetState("OnPosition");
                            State = "PickUp";

                           PlankDetachPosition = NextPositionNewPlatform;
                           NewPlatformCurrentPlankCount += 1;
                           NextPositionNewPlatform.x -= 1;
                        }
                        else if (NewPlatformCurrentPlankCount == brojPoRedu)
                        {
                            NextPositionNewPlatform.y += 0.6f;
                            NextPositionNewPlatform.x = 25.5f;
                            NewPlatformCurrentPlankCount = 0;
                            NewPlatformPoprecneCount = 0;

                            BringPoprecnaTo(4);
                            waitPoprecna = 4; 
                            State = "WaitPoprecna";
                            poprecnaState = "Start";
                            Finish();
                        }
                        
                    }
                    else
                    {
                        BringPoprecnaTo(4); //ovde ide 6, postavit poprecna to pick
                        waitPoprecna = 4;
                        State = "WaitPoprecna";
                        poprecnaState = "Start";
                        Finish();
                    }
                   
                }

                //Garbage platform
                else if (PlankToPick.localScale.z != duljinaDaske && PlankToPick.tag == "Plank")
                {
                    Debug.Log("second if: " + PlankToPick.localScale.z + "tag: " + PlankToPick.tag);
                    if (GarbagePlatformPoprecneCount == 2)
                    {
                        if (GarbagePlatformCurrentPlankCount < brojPoRedu)
                        {
                            //Set to pick it up
                            MovementScript.SetState("OnPosition");
                            State = "PickUp";

                            PlankDetachPosition = NextPositionGarbagePlatform;
                            GarbagePlatformCurrentPlankCount += 1;
                            NextPositionGarbagePlatform.z -= 1;
                        }
                        else if (GarbagePlatformCurrentPlankCount == brojPoRedu)
                        {
                            NextPositionGarbagePlatform.y += 0.6f;
                            NextPositionGarbagePlatform.z = 15;
                            GarbagePlatformCurrentPlankCount = 0;
                            GarbagePlatformPoprecneCount = 0;

                            BringPoprecnaTo(2);
                            waitPoprecna = 2;
                            State = "WaitPoprecna";
                            poprecnaState = "Start";
                            Finish();
                        }

                    }
                    else
                    {
                        BringPoprecnaTo(2);
                        waitPoprecna = 2;
                        State = "WaitPoprecna";
                        poprecnaState = "Start";
                        Finish();
                    }
                }

                //Poprecne
                else
                {
                    State = "WaitPoprecnaPickUp";
                   MovementScript.MoveToPosition(new Vector3(0, 0, PlankToPick.position.z));
                }
            }

            //Pick up Plank
            if (State == "PickUp" && MovementScript.GetState() == "OnPosition" && !HookScript.PlankAttached())
            {
                MovementScript.SetState("Wait");
                State = "Wait";
                HookScript.PickUp(PlankToPick);


            }

            //Detach plank
            if (State == "Detach" && MovementScript.GetState() == "OnPosition" && HookScript.PlankAttached()) 
            {
                MovementScript.SetState("Wait");//tu je bug, nakon detacha izgleda je se onposition ili nešto ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo
                State = "Wait";
                HookScript.DetachPlank(PlankDetachPosition);
            }

            //odluči kud s daskom
            if (State == "PickUpDone")
            {
                if (PlankToPick.tag == "Plank")
                {
                    if (PlankToPick.localScale.z == duljinaDaske)
                    {
                        MovementScript.MoveToWaypoint(4); //4 is waypoint index for newPlatform
                        State = "WaitPlank";
                    }
                    else
                    {
                        MovementScript.MoveToWaypoint(2); //2 is waypoint index for garbagePlatform
                        State = "WaitPlank";
                    }
                }
                else
                {
                    if (NewPlatformPoprecneCount < 2 || (NewPlatformPoprecneCount == 2 && NewPlatformCurrentPlankCount == brojPoRedu))
                    {
                        skipToScan = true;
                        waitPoprecna = 4;
                        State = "WaitPoprecna";
                        poprecnaState = "PickUpDone";
                        BringPoprecnaTo(4);
                        Finish();
                    }
                   else if (GarbagePlatformPoprecneCount < 2 || (GarbagePlatformPoprecneCount == 2 && GarbagePlatformCurrentPlankCount == brojPoRedu))
                    {
                        Debug.Log("GarbagePlatformPoprecneCount>>>>" + GarbagePlatformPoprecneCount);
                        Debug.Log("GarbagePlatformCurrentPlankCount>>>>" + GarbagePlatformCurrentPlankCount);
                        Debug.Log("brojPoRedu>>>>" + brojPoRedu);
                        skipToScan = true;
                        waitPoprecna = 2;
                        State = "WaitPoprecna";
                        poprecnaState = "PickUpDone";
                        BringPoprecnaTo(2);
                        Finish();
                    }
                    else
                    {
                        State = "PoprecnaToPoprecnePlatform";
                    }
                }
                
            }

            //Go back to start position
            if(State == "DetachementDone")
            {
                MovementScript.MoveToWaypoint(0);
                State = "WaitBeforeScan";
            }

            //Start process again
            if(State == "WaitBeforeScan")
            {
                if(MovementScript.GetState() == "OnWaypoint")
                {
                    State = "Scan";
                }
            }

            //Put poprecna to poprecna platfrom and begin new process
            if (State == "PoprecnaToPoprecnePlatform")
            {
                MovePoprecnaToPlatform();
            }

            if(State == "WaitPoprecna")
            {
                BringPoprecnaTo(waitPoprecna);
            }

            if(State == "WaitPoprecnaPickUp")
            {
                if(MovementScript.GetState() == "OnPosition")
                {
                    State = "PickUp";
                }
            }

            if(State == "WaitPlank")
            {
                if (MovementScript.GetState() == "OnWaypoint")
                {
                    State = "Detach";
                    MovementScript.SetState("OnPosition");
                }
            }

            if(State == "Finish")
            {
                begin = false;
            }
        }
    }

    private void BringPoprecnaTo(int waypoint)
    {

        Debug.Log("poprecnaState == " + poprecnaState);
        Debug.Log("Waypoint == " + waypoint);
        if (poprecnaState == "Start")
        {
            MovementScript.MoveToWaypoint(6); //poprecne platform
            poprecnaState = "FindPoprecna";
        }

        if (poprecnaState == "FindPoprecna")
        {
            if (MovementScript.GetState() == "OnWaypoint")
            {
                Vector3 pos = new Vector3(NextToDetachPoprecnePlatform.x - 0.5f, MovementScript.Waypoints[6].position.y, MovementScript.Waypoints[6].position.z);
                MovementScript.MoveToPosition(pos);
                poprecnaState = "PickUp";
            }
        }

        if (poprecnaState == "PickUp")
        {
            if (MovementScript.GetState() == "OnPosition")
            {
                poprecnaState = "WaitPickUp";
                Debug.Log("NextToPickPoprecnePlatform " + NextToPickPoprecnePlatform.name);
                Debug.Log("NextToPickPoprecnePlatform " + NextToPickPoprecnePlatform.name);
                Debug.Log("NextToPickPoprecnePlatform " + NextToPickPoprecnePlatform.name);
                Debug.Log("NextToPickPoprecnePlatform " + NextToPickPoprecnePlatform.name);
                HookScript.PickUp(NextToPickPoprecnePlatform);
                brojPoprecnih -= 1;

                Debug.Log("brojPoprecnih >>>>>>" + brojPoprecnih);

                NextToDetachPoprecnePlatform = new Vector3(NextToDetachPoprecnePlatform.x - 0.5f, 0.6f, -13);

                if(brojPoprecnih > 0)
                {
                    List<Transform> dummy = new List<Transform>(brojPoprecnih * 2);
                    for (int i = 1; i <= brojPoprecnih; i++)
                    {
                        dummy.Insert(i - 1, PoprecneList[i]);
                    }
                    PoprecneList = new List<Transform>(brojPoprecnih * 2);

                    for (int i = 0; i < brojPoprecnih; i++)
                    {
                        PoprecneList.Insert(i, dummy[i]);
                        Debug.Log("PoprecneList.Insert  index>>> " + i + "Name>>>> " + dummy[i].name);
                    }

                    NextToPickPoprecnePlatform = PoprecneList[0];
                    Debug.Log("NextToPickPoprecnePlatform 0000>>>" + PoprecneList[0].name);
                    Debug.Log("NextToPickPoprecnePlatform 0000>>>>" + PoprecneList[0].name);
                    Debug.Log("NextToPickPoprecnePlatform 0000>>>>" + PoprecneList[0].name);
                }
            }
        }

        if (poprecnaState == "PickUpDone")
        {
            MovementScript.MoveToWaypoint(waypoint);
            poprecnaState = "WaitMovement";
        }

        if(poprecnaState == "WaitMovement")
        {

            if (MovementScript.GetState() == "OnWaypoint")
            {
                poprecnaState = "OnWaypoint";
            }
        }

        if (poprecnaState == "OnWaypoint")
        {
            if(waypoint == 4)
            {
                Debug.Log("NewPlatformPoprecneCount = " + NewPlatformPoprecneCount);
                if (NewPlatformPoprecneCount == 0 || NewPlatformPoprecneCount == 2)
                {
                    if (NewPlatformPoprecneCount == 2)
                    {
                        NewPlatformPoprecneCount = 0;
                        if (NewPlatformCurrentPlankCount == brojPoRedu){
                            NewPlatformCurrentPlankCount = 0;
                            NextPositionNewPlatform.y += 0.6f;
                            NextPositionNewPlatform.x = 25.5f;
                        }
                    }
                    MovementScript.MoveToPosition(new Vector3(MovementScript.Waypoints[4].position.x, MovementScript.Waypoints[4].position.y, 2));
                    NewPlatformPoprecneCount += 1;
                    PoprecnaPosition = NextPoprecnaNewPlatform;
                    NextPoprecnaNewPlatform.z = -2;
                    poprecnaState = "Detach";
                }
                else
                {
                    MovementScript.MoveToPosition(new Vector3(MovementScript.Waypoints[4].position.x, MovementScript.Waypoints[4].position.y, -2));
                    NewPlatformPoprecneCount += 1;
                    PoprecnaPosition = NextPoprecnaNewPlatform;
                    NextPoprecnaNewPlatform.z = 2;
                    NextPoprecnaNewPlatform.y += 0.6f;
                    poprecnaState = "Detach";
                }
            }

           else
            {
                if (GarbagePlatformPoprecneCount == 0 || GarbagePlatformPoprecneCount == 2)
                {
                    if (GarbagePlatformPoprecneCount == 2)
                    {
                        GarbagePlatformPoprecneCount = 0;
                        if(GarbagePlatformCurrentPlankCount == brojPoRedu){
                            GarbagePlatformCurrentPlankCount = 0;
                            NextPositionGarbagePlatform.y += 0.6f;
                            NextPositionGarbagePlatform.z = 15;
                        }
                    }
                    MovementScript.MoveToPosition(new Vector3(8, MovementScript.Waypoints[2].position.y, MovementScript.Waypoints[2].position.z));
                    GarbagePlatformPoprecneCount += 1;
                    PoprecnaPosition = NextPoprecnaGarbagePlatform;
                    NextPoprecnaGarbagePlatform.x = 12;
                    poprecnaState = "Detach";
                }
                else
                {
                    MovementScript.MoveToPosition(new Vector3(12, MovementScript.Waypoints[2].position.y, MovementScript.Waypoints[2].position.z));
                    GarbagePlatformPoprecneCount += 1;
                    PoprecnaPosition = NextPoprecnaGarbagePlatform;
                    NextPoprecnaGarbagePlatform.x = 8;
                    NextPoprecnaGarbagePlatform.y += 0.6f;
                    poprecnaState = "Detach";
                }
            }
        }

        if(poprecnaState == "Detach")
        {
            if(MovementScript.GetState() == "OnPosition")
            {
                HookScript.DetachPlank(PoprecnaPosition);
                poprecnaState = "Wait";
            }
        }

        if(poprecnaState == "DetachementDone")
        {
            MovementScript.MoveToWaypoint(0);
            poprecnaState = "WaitReturn";
        }

        if(poprecnaState == "WaitReturn")
        {
            if(MovementScript.GetState() == "OnWaypoint")
            {
                poprecnaState = "Start";
                if (skipToScan)
                {
                    skipToScan = false;
                    State = "Scan";
                }
                else
                {
                    State = "Calculate";
                }
            }
        }
    }

    private void MovePoprecnaToPlatform()
    {
        Debug.Log("poprecnaToPlatformState == " + poprecnaToPlatformState);
        if (poprecnaToPlatformState == "Start")
        {
            MovementScript.MoveToWaypoint(6);
            poprecnaToPlatformState = "WaitWaypoint";
            return;
        }

        if (poprecnaToPlatformState == "WaitWaypoint")
        {
            if (MovementScript.GetState() == "OnWaypoint")
            {
                Vector3 pos = new Vector3(NextToDetachPoprecnePlatform.x, MovementScript.Waypoints[6].position.y, MovementScript.Waypoints[6].position.z);
                MovementScript.MoveToPosition(pos);
                poprecnaToPlatformState = "WaitPosition";
                return;
            }
        }

        if (poprecnaToPlatformState == "WaitPosition")
        {
            if (MovementScript.GetState() == "OnPosition")
            {
                poprecnaToPlatformState = "WaitPickUp";
                HookScript.DetachPlank(new Vector3((NextToDetachPoprecnePlatform.x ), (NextToDetachPoprecnePlatform.y + 0.9600004f), NextToDetachPoprecnePlatform.z));
                brojPoprecnih += 1;

                NextToDetachPoprecnePlatform = new Vector3(NextToDetachPoprecnePlatform.x + 0.5f, 0.6f, -13);
                
                List<Transform> dummy = new List<Transform>();
                dummy.Add(PlankToPick);
                int loop = PoprecneList.Count;
                Debug.Log("PoprecneList.Count>>> " + PoprecneList.Count);
                for (int i = 0; i < loop; i++)
                {
                    Debug.Log("dummy.Insert ADD index>>> " + i + " Name>>>> " + dummy[i].name);
                    dummy.Add(PoprecneList[i]);
                }
                PoprecneList = new List<Transform>();

                Debug.Log("dummy.Count>>> " + dummy.Count);
                for (int i = 0; i < dummy.Count; i++)
                {
                    
                    PoprecneList.Add(dummy[i]);
                    
                    Debug.Log("PoprecneList.Insert ADD index>>> " + i + " Name>>>> " + PoprecneList[i].name);
                }

                NextToPickPoprecnePlatform = PoprecneList[0];

                Debug.Log("PoprecneList.Insert NextToPickPoprecnePlatform >>> " + 0 + " Name>>>> " + NextToPickPoprecnePlatform.name);
            }
        }

        if (poprecnaToPlatformState == "DetachementDone")
        {
            MovementScript.MoveToWaypoint(0);
            poprecnaToPlatformState = "WaitReset";
        }

        if(poprecnaToPlatformState == "WaitReset")
        {
            if (MovementScript.GetState() == "OnWaypoint")
            {
                poprecnaToPlatformState = "Start";
                State = "Scan";
            }
        }
    }


    //Check for end
    private void Finish()
    {
        if (brojPoprecnih == 0)
        {
            State = "Finish";
            begin = false;
            return;
        }
    }

    public void SetState(string s)
    {
        State = s;
    }

    public void SetPoprecnaState(string s)
    {
        poprecnaState = s;
    }

    public string GetPoprecnaState()
    {
        return poprecnaState;
    }

    public void SetPoprecnaToPlatformState(string s)
    {
        poprecnaToPlatformState = s;
    }

    public string GetPoprecnaToPlatformState()
    {
        return poprecnaToPlatformState;
    }
}
