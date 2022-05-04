using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovementController : MonoBehaviour
{

    public Transform[] Waypoints = new Transform[8];
    private int destination;
    private Vector3 NextWaypoint;

    private string State;
    private string MovementState;
    private float speed;

    //Positions
    public Transform LeftScanWaypoint;
    public Transform RightScanWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        speed = Time.deltaTime * 25;
    }

    // Update is called once per frame
    void Update()
    {
        //Moving robot left to right to center for scan
        if (State == "ScanAcross")
        {
            MoveAcross();
        }

        //Move to given waypoint
        if(State == "MoveToPosition")
        {
            if (Vector3.Distance(transform.position, NextWaypoint) > 0.0001f)
            {

                transform.position = Vector3.MoveTowards(transform.position, NextWaypoint, speed);
            }
            else
            {
               State = "OnPosition";
            }
        }

        //Move to closest Waypoint
        if(State == "MoveToClosest")
        {
            if (Vector3.Distance(transform.position, Waypoints[destination].position) > 0.0001f)
            {

                transform.position = Vector3.MoveTowards(transform.position, Waypoints[destination].position, speed);
            }
            else
            {
                State = "OnClosestWaypoint";
            }
        }

        //After on one of waypoints, move to given one
        if (State == "OnClosestWaypoint")
        {
            
            if (Vector3.Distance(transform.position, Waypoints[destination].position) > 0.0001f)
            {

                transform.position = Vector3.MoveTowards(transform.position, Waypoints[destination].position, speed);
            }
            else if (Vector3.Distance(Waypoints[destination].position, NextWaypoint) > 0.0001f)
            {
                if (destination == 7)
                {
                    destination = 0;
                }

                else
                {
                    destination++;
                }
                //If on corner, rotate
                if(Waypoints[destination].tag == "Rotate")
                {
                    transform.Rotate(new Vector3(0, 90, 0));
                }
            }
            else
            {
                State = "OnWaypoint";
            }
        }



    }

    //Move across to scan
    public void MoveAcross()
    {
        if(MovementState == "MoveLeft")
        {
            if (Vector3.Distance(transform.position, LeftScanWaypoint.position) > 0.0001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, LeftScanWaypoint.position, speed);
            }
            else
            {
                MovementState = "MoveRight";
            }
        }

        if(MovementState == "MoveRight")
        {
            if (Vector3.Distance(transform.position, RightScanWaypoint.position) > 0.0001f)
            {

                transform.position = Vector3.MoveTowards(transform.position, RightScanWaypoint.position, speed);
            }
            else
            {
                MovementState = "MoveToCenter";
            }
        }

        if (MovementState == "MoveToCenter")
        {
            if (Vector3.Distance(transform.position, Waypoints[0].position) > 0.0001f)
            {

                transform.position = Vector3.MoveTowards(transform.position, Waypoints[0].position, speed);
            }
            else
            {
                State = "ScanAcrossDone";
            }
        }
    }

    //Move robot toward given position
    public void MoveToPosition(Vector3 dest)
    {
        State = "MoveToPosition";
        NextWaypoint = dest;
    }

    public void MoveToWaypoint(int index)
    {
        NextWaypoint = Waypoints[index].position;
        destination = ClosestWaypointIndex(transform.position);
        Debug.Log("MoveToWaypoint  Closest - " + destination);
        State = "MoveToClosest";
    }

    //Find the closest waypoint index
    private int ClosestWaypointIndex(Vector3 current)
    {
        int closest = 0;
        float distance = Vector3.Distance(current, Waypoints[0].position);
        for (int i = 0; i < 8; i++)
        {
            if (Vector3.Distance(current, Waypoints[i].position) < distance)
            {
                closest = i;
                distance = Vector3.Distance(current, Waypoints[i].position);
            }
        }
        return closest;
    }

    public void SetDestination(int n)
    {
        destination = n;
    }

    public string GetState()
    {
        return State;
    }

    public void SetState(string s)
    {
        State = s;
    }

    public void SetMovementState(string s)
    {
        Debug.Log("Set-"+s);
        MovementState = s;
    }

    public void DebugF()
    {
        Debug.Log("Movement script working");
    }
}
