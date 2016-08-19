using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class test_filling : MonoBehaviour{

    Vector3 v0 = new Vector3(1, 0, 0);
    Vector3 v1 = new Vector3(4, 0, 0);
    Vector3 v2 = new Vector3(6, 0, 0);
    Vector3 v3 = new Vector3(10, 0, 0);
    Vector3 v4 = new Vector3(15, 0, 0);

    void Start()
    {
        VT vt0 = new VT(v0, 1);
        VT vt1 = new VT(v1, 4);
        VT vt2 = new VT(v2, 6);
        VT vt3 = new VT(v3, 10);
        VT vt4 = new VT(v4, 15);

        List<VT> vts = new List<VT>();
        vts.Add(vt2);
        vts.Add(vt0);
        vts.Add(vt3);
        vts.Add(vt1);
        vts.Add(vt4);

        foreach (VT vt in vts)
        {
            Debug.Log(vt.time);
        }

        vts.Sort();

        foreach (VT vt in vts)
        {
            Debug.Log(vt.time);
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
}