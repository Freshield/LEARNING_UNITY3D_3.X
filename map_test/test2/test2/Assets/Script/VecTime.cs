using UnityEngine;
using System;
using System.Collections.Generic;


public class VecTime : IComparable<VecTime>
{
    public Vector3 worldPosition;
    public PTime time;

    public VecTime(Vector3 worldPosition, PTime time)
    {
        this.worldPosition = worldPosition;
        this.time = time;
    }

    public int CompareTo(VecTime VecToCompare)
    {
        if (this.time.totalTime > VecToCompare.time.totalTime)
        {
            return 1;
        }
        else if (this.time.totalTime < VecToCompare.time.totalTime)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

}
