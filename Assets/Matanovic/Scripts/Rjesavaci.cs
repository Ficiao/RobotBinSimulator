using System;
using UnityEngine;

public class Rjesavaci
{
    public static Vector3 m2x2(int[,,] matrica, Transform cube, Transform pozicija)
    {
        Vector3 ret = pozicija.position;
        int[] kat = null;
        switch (cube.GetComponent<CubeSpawnProp>().mekoca)
        {
            case 0:
                kat = new int[3] { 2, 1, 0 };
                break;
            case 1:
                kat = new int[3] { 1, 2, 0 };
                break;
            case 2:
                kat = new int[3] { 0, 1, 2 };
                break;

        }

        int uzduzno = 0;
        int poprecno = 0;
        int test = 0;
        int i1, i2, j1, j2;
        foreach (int k in kat)
        {
            for (i1 = 0, j2 = 0; j2 < matrica.GetLength(2); i1++, j2++)
            {
                for (i2 = 0, j1 = 0; j1 < matrica.GetLength(2); i2++, j1++)
                {
                    if (i2 < matrica.GetLength(1))
                    {
                        if (matrica[k, i2, j2] == -1)
                        {
                            if (k == 0)
                            {
                                poprecno++;
                                if (poprecno == 2)
                                {
                                    try
                                    {
                                        if (matrica[k, i2, j2 + 1] == -1 && matrica[k, i2 - 1, j2 + 1] == -1)
                                        {
                                            test = 2;
                                        }
                                    }
                                    catch
                                    {
                                        poprecno = 1;
                                    }
                                }
                            }
                            else if (k == 1)
                            {
                                if (matrica[k - 1, i2, j2] >= 1)
                                {
                                    poprecno++;
                                    if (poprecno == 2)
                                    {
                                        try
                                        {
                                            if (matrica[k, i2, j2 + 1] == -1 && matrica[k, i2 - 1, j2 + 1] == -1
                                                && matrica[k - 1, i2, j2 + 1] >= 1 && matrica[k - 1, i2 - 1, j2 + 1] >= 1)
                                            {
                                                test = 2;
                                            }
                                        }
                                        catch
                                        {
                                            poprecno = 1;
                                        }
                                    }
                                }
                                else
                                {
                                    poprecno = 0;
                                }
                            }
                            else if (k == 2)
                            {
                                if (matrica[k - 1, i2, j2] >= 1 && matrica[k - 2, i2, j2] == 2)
                                {
                                    poprecno++;
                                    if (poprecno == 2)
                                    {
                                        try
                                        {
                                            if (matrica[k, i2, j2 + 1] == -1 && matrica[k, i2 - 1, j2 + 1] == -1
                                                && matrica[k - 1, i2, j2 + 1] >= 1 && matrica[k - 1, i2 - 1, j2 + 1] >= 1
                                                && matrica[k - 2, i2, j2 + 1] == 2 && matrica[k - 2, i2 - 1, j2 + 1] == 2)
                                            {
                                                test = 2;
                                            }
                                        }
                                        catch
                                        {
                                            poprecno = 1;
                                        }
                                    }
                                }
                                else
                                {
                                    poprecno = 0;
                                }
                            }
                        }
                        else
                        {
                            poprecno = 0;
                        }
                        if (test == 2)
                        {
                            matrica[k, i2 - 1, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i2, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i2 - 1, j2 + 1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i2, j2 + 1] = cube.GetComponent<CubeSpawnProp>().mekoca;

                            ret.x += (i2 * 0.5f + (i2 - 1) * 0.5f) / 2;
                            ret.z += (j2 * 0.5f + (j2 + 1) * 0.5f) / 2;
                            ret.y += k * 0.5f;
                            return ret;
                        }
                    }

                    if (i1 < matrica.GetLength(1))
                    {
                        if (matrica[k, i1, j1] == -1)
                        {
                            if (k == 0)
                            {
                                uzduzno++;
                                if (uzduzno == 2)
                                {
                                    try
                                    {
                                        if (matrica[k, i1 + 1, j1 - 1] == -1 && matrica[k, i1 + 1, j1] == -1)
                                        {
                                            test = 1;
                                        }
                                    }
                                    catch
                                    {
                                        uzduzno = 1;
                                    }
                                }
                            }
                            else if (k == 1)
                            {
                                if (matrica[k - 1, i1, j1] >= 1)
                                {
                                    uzduzno++;
                                    if (uzduzno == 2)
                                    {
                                        try
                                        {
                                            if (matrica[k, i1 + 1, j1 - 1] == -1 && matrica[k, i1 + 1, j1] == -1
                                                && matrica[k-1, i1 + 1, j1 - 1] >= 1 && matrica[k-1, i1 + 1, j1] >= 1)
                                            {
                                                test = 1;
                                            }
                                        }
                                        catch
                                        {
                                            uzduzno = 1;
                                        }
                                    }
                                }
                                else
                                {
                                    uzduzno = 0;
                                }
                            }
                            else if (k == 2)
                            {
                                if (matrica[k - 1, i1, j1] >= 1 && matrica[k - 2, i1, j1] == 2)
                                {
                                    uzduzno++;
                                    if (uzduzno == 2)
                                    {
                                        try
                                        {
                                            if (matrica[k, i1 + 1, j1 - 1] == -1 && matrica[k, i1 + 1, j1] == -1
                                                && matrica[k - 1, i1 + 1, j1 - 1] >= 1 && matrica[k - 1, i1 + 1, j1] >= 1
                                                && matrica[k - 2, i1 + 1, j1 - 1] == 2 && matrica[k - 2, i1 + 1, j1] == 2)
                                            {
                                                test = 1;
                                            }
                                        }
                                        catch
                                        {
                                            uzduzno = 1;
                                        }
                                    }
                                }
                                else
                                {
                                    uzduzno = 0;
                                }
                            }
                        }
                        else
                        {
                            uzduzno = 0;
                        }
                        if (test==1)
                        {
                            matrica[k, i1, j1 - 1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i1, j1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i1 + 1, j1 - 1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i1 + 1, j1] = cube.GetComponent<CubeSpawnProp>().mekoca;

                            ret.x += (i1 * 0.5f + (i1+1) * 0.5f) / 2;
                            ret.z += (j1 * 0.5f + (j1 - 1) * 0.5f) / 2;
                            ret.y += k * 0.5f;
                            return ret;
                        }
                    }

                }
            }
        }

        return new Vector3();
    }

    public static Vector3 m1x2(int[,,] matrica, Transform cube, int klasa, HookControl hook, Transform pozicija)
    {
        Vector3 ret = pozicija.position;
        int[] kat = null;
        switch (cube.GetComponent<CubeSpawnProp>().mekoca)
        {
            case 0:
                kat = new int[3] { 2, 1, 0 };
                break;
            case 1:
                kat = new int[3] { 1, 2, 0 };
                break;
            case 2:
                kat = new int[3] { 0, 1, 2 };
                break;

        }

        int uzduzno = 0;
        int poprecno = 0;

        int i1, i2, j1, j2;
        foreach (int k in kat)
        {
            for (i1 = 0, j2 = 0; j2 < matrica.GetLength(2); i1++, j2++)
            {
                uzduzno = 0;
                poprecno = 0;
                for (i2 = 0, j1 = 0; j1 < matrica.GetLength(2); i2++, j1++)
                {
                    if (i2 < matrica.GetLength(1))
                    {
                        if (matrica[k, i2, j2] == -1)
                        {
                            if (k == 0)
                            {
                                poprecno++;
                            }
                            else if (k == 1)
                            {
                                if (matrica[k - 1, i2, j2] >= 1)
                                {
                                    poprecno++;
                                }
                                else
                                {
                                    poprecno = 0;
                                }
                            }
                            else if (k == 2)
                            {
                                if (matrica[k - 1, i2, j2] >= 1 && matrica[k - 2, i2, j2] == 2)
                                {
                                    poprecno++;
                                }
                                else
                                {
                                    poprecno = 0;
                                }
                            }
                        }
                        else
                        {
                            poprecno = 0;
                        }
                        if (poprecno == 2)
                        {
                            matrica[k, i2-1, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i2, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            if (klasa == 3)
                            {
                                hook.toRotate = 1;
                            }
                            ret.x += ((i2-1) * 0.5f+ (i2) * 0.5f)/2;
                            ret.z += j2 * 0.5f;
                            ret.y += k * 0.5f;
                            return ret;
                        }
                    }


                    if (i1 < matrica.GetLength(1))
                    {
                        if (matrica[k, i1, j1] == -1)
                        {
                            if (k == 0)
                            {
                                uzduzno++;
                            }
                            else if (k == 1)
                            {
                                if (matrica[k - 1, i1, j1] >= 1)
                                {
                                    uzduzno++;
                                }
                                else
                                {
                                    uzduzno = 0;
                                }
                            }
                            else if (k == 2)
                            {
                                if (matrica[k - 1, i1, j1] >= 1 && matrica[k - 2, i1, j1] == 2)
                                {
                                    uzduzno++;
                                }
                                else
                                {
                                    uzduzno = 0;
                                }
                            }
                        }
                        else
                        {
                            uzduzno = 0;
                        }
                        if (uzduzno == 2)
                        {
                            matrica[k, i1, j1 - 1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i1, j1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            if (klasa == 1)
                            {
                                hook.toRotate = 1;
                            }
                            ret.x += i1 * 0.5f;
                            ret.z += ((j1-1) * 0.5f+j1*0.5f)/2;
                            ret.y += k * 0.5f;
                            return ret;
                        }
                    }

                }
            }
        }

        return new Vector3();
    }


    public static Vector3 m1x3(int[,,] matrica, Transform cube, int klasa,HookControl hook, Transform pozicija)
    {
        Vector3 ret = pozicija.position;
        int[] kat = null;
        switch (cube.GetComponent<CubeSpawnProp>().mekoca)
        {
            case 0:
                kat = new int[3] { 2, 1, 0 };
                break;
            case 1:
                kat = new int[3] { 1, 2, 0 };
                break;
            case 2:
                kat = new int[3] { 0, 1, 2 };
                break;

        }

        int uzduzno = 0;
        int poprecno = 0;

        int i1, i2, j1, j2;
        foreach (int k in kat)
        {
            for (i1 = 0, j2 = 0; j2 < matrica.GetLength(2); i1++, j2++)
            {
                uzduzno = 0;
                poprecno = 0;
                for (i2 = 0, j1 = 0; j1 < matrica.GetLength(2); i2++, j1++)
                {
                    if (i2 < matrica.GetLength(1))
                    {
                        if (matrica[k, i2, j2] == -1)
                        {
                            if (k == 0)
                            {
                                poprecno++;
                            }
                            else if (k == 1)
                            {
                                if (matrica[k - 1, i2, j2] >= 1)
                                {
                                    poprecno++;
                                }
                                else
                                {
                                    poprecno = 0;
                                }
                            }
                            else if (k == 2)
                            {
                                if (matrica[k - 1, i2, j2] >= 1 && matrica[k - 2, i2, j2] == 2)
                                {
                                    poprecno++;
                                }
                                else
                                {
                                    poprecno = 0;
                                }
                            }
                        }
                        else
                        {
                            poprecno = 0;
                        }
                        if (poprecno == 3)
                        {
                            matrica[k, i2-2, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i2-1, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i2, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            if (klasa == 6)
                            {
                                hook.toRotate = 1;
                            }
                            ret.x += (i2-1) * 0.5f;
                            ret.z += j2 * 0.5f;
                            ret.y += k * 0.5f;
                            return ret;                            
                        }
                    }
                      

                    if (i1 < matrica.GetLength(1))
                    {
                        if (matrica[k, i1, j1] == -1)
                        {
                            if (k == 0)
                            {
                                uzduzno++;
                            }
                            else if (k == 1)
                            {
                                if (matrica[k - 1, i1, j1] >= 1)
                                {
                                    uzduzno++;
                                }
                                else
                                {
                                    uzduzno = 0;
                                }
                            }
                            else if (k == 2)
                            {
                                if (matrica[k - 1, i1, j1] >= 1 && matrica[k - 2, i1, j1] == 2)
                                {
                                    uzduzno++;
                                }
                                else
                                {
                                    uzduzno = 0;
                                }
                            }
                        }
                        else
                        {
                            uzduzno = 0;
                        }
                        if (uzduzno == 3)
                        {
                            matrica[k, i1, j1 - 2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i1, j1 - 1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            matrica[k, i1, j1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                            if (klasa == 2)
                            {
                                hook.toRotate = 1;
                            }
                            ret.x += i1 * 0.5f;
                            ret.z += (j1 - 1) * 0.5f;
                            ret.y += k * 0.5f;
                            return ret;
                        }
                    }

                }
            }
        }

        return new Vector3();
    }

    public static Vector3 m1x1(int[,,] matrica, Transform cube, Transform pozicija)
    {
        Vector3 ret = pozicija.position;
        int[] kat=null;
        switch (cube.GetComponent<CubeSpawnProp>().mekoca)
        {
            case 0:
                kat = new int[3] { 2, 1, 0 };
                break;
            case 1:
                kat = new int[3] { 1, 2, 0 };
                break;
            case 2:
                kat = new int[3] { 0, 1, 2 };
                break;

        }

        int i1, i2, j1, j2;
        foreach (int k in kat)
        {
            for (i1 = 0,j2=0; j2 < matrica.GetLength(2); i1++,j2++)
            {
                for (i2 = 0, j1 = 0; j1 < matrica.GetLength(2); i2++, j1++)
                {
                    if (i2 < matrica.GetLength(1))
                    {
                        if (matrica[k, i2, j2] == -1)
                        {
                            if (k == 0)
                            {
                                matrica[k, i2, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                                ret.x += i2 * 0.5f;
                                ret.z += j2 * 0.5f;
                                ret.y += k * 0.5f;
                                return ret;
                            }
                            else if (k == 1)
                            {
                                if (matrica[k - 1, i2, j2] >= 1)
                                {
                                    matrica[k, i2, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                                    ret.x += i2 * 0.5f;
                                    ret.z += j2 * 0.5f;
                                    ret.y += k * 0.5f;
                                    return ret;
                                }
                            }
                            else if (k == 2)
                            {
                                if (matrica[k - 1, i2, j2] >= 1 && matrica[k - 2, i2, j2] == 2)
                                {
                                    matrica[k, i2, j2] = cube.GetComponent<CubeSpawnProp>().mekoca;
                                    ret.x += i2 * 0.5f;
                                    ret.z += j2 * 0.5f;
                                    ret.y += k * 0.5f;
                                    return ret;
                                }
                            }
                        }
                    }

                    if (i1 < matrica.GetLength(1))
                    {
                        if (matrica[k, i1, j1] == -1)
                        {
                            if (k == 0)
                            {
                                matrica[k, i1, j1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                                ret.x += i1 * 0.5f;
                                ret.z += j1 * 0.5f;
                                ret.y += k * 0.5f;
                                return ret;
                            }
                            else if (k == 1)
                            {
                                if (matrica[k - 1, i1, j1] >= 1)
                                {
                                    matrica[k, i1, j1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                                    ret.x += i1 * 0.5f;
                                    ret.z += j1 * 0.5f;
                                    ret.y += k * 0.5f;
                                    return ret;
                                }
                            }
                            else if (k == 2)
                            {
                                if (matrica[k - 1, i1, j1] >= 1 && matrica[k - 2, i1, j1] == 2)
                                {
                                    matrica[k, i1, j1] = cube.GetComponent<CubeSpawnProp>().mekoca;
                                    ret.x += i1 * 0.5f;
                                    ret.z += j1 * 0.5f;
                                    ret.y += k * 0.5f;
                                    return ret;
                                }
                            }
                        }
                    }

                }
            }
        }

        return new Vector3();
    }

}

