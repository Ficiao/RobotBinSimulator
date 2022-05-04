using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnProp : MonoBehaviour
{

    private int move=1;
    private int stop = 0;
    public int mekoca;
    public int klasa;

    private void Awake()
    {
        klasa = Random.Range(0, 9);
        Vector3 pos = this.transform.localScale;

        int privodabir = Random.Range(0, 8);
        switch (privodabir)
        {
            case 0:
            case 1:
            case 8:
                klasa = 0;
                break;
            case 2:
                klasa = 2;
                break;
            case 3:
                klasa = 6;
                break;
            case 4:
                klasa = 1;
                break;
            case 5:
                klasa = 3;
                break;
            case 7:
                klasa = 4;
                break;
            case 9:
                klasa = 5;
                break;
            case 10:
                klasa = 7;
                break;
            case 11:
                klasa = 8;
                break;
            default:
                klasa = 0;
                break;
        }

        if (klasa == 7 || klasa == 5 || klasa == 8)
        {
            klasa = 0;
        }
        switch (klasa)
        {
            case 0:
                pos.x = 0.5f;
                pos.y = 0.5f;
                break;

            case 1:
                pos.x = 1f;
                pos.y = 0.5f;
                break;

            case 2:
                pos.x = 1.5f;
                pos.y = 0.5f;
                break;

            case 3:
                pos.x = 0.5f;
                pos.y = 1f;
                break;

            case 4:
                pos.x = 1f;
                pos.y = 1f;
                break;

            case 5:
                pos.x = 1.5f;
                pos.y = 1f;
                break;

            case 6:
                pos.x = 0.5f;
                pos.y = 1.5f;
                break;

            case 7:
                pos.x = 1f;
                pos.y = 1.5f;
                break;

            case 8:
                pos.x = 1.5f;
                pos.y = 1.5f;
                break;
        }
        pos.z = 0.5f;
        this.transform.localScale = pos;
        this.GetComponent<Rigidbody>().freezeRotation = true;
        mekoca= Random.Range(0, 6);
        if (mekoca >= 0 && mekoca <= 2)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
            mekoca = 2;
        }
        else if (mekoca <= 4)
        {
            this.GetComponent<Renderer>().material.color = (Color.red*4 + Color.yellow*5) / 9;
            mekoca = 1;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.yellow;
            mekoca = 0;
        }
        this.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void Update()
    {
        if (klasa == 7 || klasa == 5 || klasa == 8)
        {
            klasa = 0;
        }
        if (move == 1)
        {
            Vector3 vel = this.GetComponent<Rigidbody>().velocity;
            vel.z = 4f;
            GetComponent<Rigidbody>().velocity = vel;
        }
        else if (stop==1){
            Vector3 vel = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().velocity = vel;
            stop = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("StopTrakaMain"))
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            move = 0;
            stop = 1;
        }
    }

}
