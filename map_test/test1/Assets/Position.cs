using UnityEngine;
using System.Collections;

public class Position
{
    public float latitute;
    public float lontitute;
    public int time;

    public Position(float latitute, float lontitute, int time)
    {
        this.latitute = latitute;
        this.lontitute = lontitute;
        this.time = time;
    }
}
