using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class test_filling : MonoBehaviour{

    Vector3 v0 = new Vector3(1, 7, 2);
    Vector3 v1 = new Vector3(4, 9, 3);
    Vector3 v2 = new Vector3(6, 0, 4);
    Vector3 v3 = new Vector3(10, 4, 7);
    Vector3 v4 = new Vector3(10, 7, 12);

    void Start()
    {
        VT vt0 = new VT(v0, 1);
        VT vt1 = new VT(v1, 4);
        VT vt2 = new VT(v2, 6);
        VT vt3 = new VT(v3, 10);
        VT vt4 = new VT(v4, 15);
        VT vt5 = new VT(v4, 6);

        List<VT> vts = new List<VT>();
        vts.Add(vt2);
        vts.Add(vt0);
        vts.Add(vt3);
        vts.Add(vt1);
        vts.Add(vt4);
        vts.Add(vt5);

        Debug.Log("before sort");
        foreach (VT vt in vts)
        {
            
            Debug.Log(vt.time);
        }

        vts.Sort();

        Debug.Log("after sort");
        foreach (VT vt in vts)
        {
            
            Debug.Log(vt.time);
        }

        VT.filling(vts);

        Debug.Log("final");
        foreach (VT vt in vts)
        {
            
            Debug.Log(vt.position+","+vt.time);
        }
    }
}



public class VT : IComparable<VT>
{
    public Vector3 position;
    public float time;

    public VT(Vector3 position, float time)
    {
        this.position = position;
        this.time = time;
    }

    public int CompareTo(VT VTToCompare)
    {
        if (this.time > VTToCompare.time)
        {
            return 1;
        }
        else if(this.time < VTToCompare.time)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    ///////////////////static function///////////////////////////
    public static List<VT> filling(List<VT> vts)
    {
        vts.Sort();

        List<VT> filling = new List<VT>();

        VT before = vts[0];
        for (int i = 0; i < vts.Count; i++)
        {
            //get the first one
            if (i == 0)
            {
                before = vts[0];
                continue;
            }
            //calculate the difference
            float timeDif = vts[i].time - before.time;
            Vector3 vectorDif = vts[i].position - before.position;
            //vector difference per time
            vectorDif /= timeDif;
            //generate filling
            VT fillTemp = new VT(before.position, before.time);
            for (int j = 0; j < (int)timeDif - 1; j++)
            {
                fillTemp.position += vectorDif;
                fillTemp.time += 1;
                VT fill = new VT(fillTemp.position, fillTemp.time);
                filling.Add(fill);
            }
            //prepare for next loop
            before = vts[i];
        }
        vts.AddRange(filling);
        vts.Sort();

        return vts;
    }
}