using UnityEngine;
using System;
using System.Collections.Generic;


//unity world position + time
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
        if (time.totalTime > VecToCompare.time.totalTime)
        {
            return 1;
        }
        else if (time.totalTime < VecToCompare.time.totalTime)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    ////////////////////////////static function/////////////////////////////
    ///////////////////static function///////////////////////////
    public static List<VecTime> filling(List<VecTime> positions)
    {
        positions.Sort();

        List<VecTime> filling = new List<VecTime>();

        VecTime before = positions[0];
        for (int i = 0; i < positions.Count; i++)
        {
            //get the first one
            if (i == 0)
            {
                before = positions[0];
                continue;
            }
            //calculate the difference
            float timeDif = positions[i].time.totalTime - before.time.totalTime;
            Vector3 vectorDif = positions[i].worldPosition - before.worldPosition;
            //vector difference per time
            vectorDif /= timeDif;
            //generate filling
            VecTime fillTemp = new VecTime(before.worldPosition, before.time);
            for (int j = 0; j < (int)timeDif - 1; j++)
            {
                fillTemp.worldPosition += vectorDif;
                fillTemp.time.setTime(fillTemp.time.totalTime + 1);
                VecTime fill = new VecTime(fillTemp.worldPosition, fillTemp.time);
                filling.Add(fill);
            }
            //prepare for next loop
            before = positions[i];
        }
        positions.AddRange(filling);
        positions.Sort();

        return positions;
    }

}
