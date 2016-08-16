using UnityEngine;
using System.Collections;

public class Track{
    public string name;
    public ArrayList positions;
    public float avgLat = 0;
    public float avgLon = 0;

    public Track(string name)
    {
        this.name = name;
        positions = new ArrayList();
    }

    public void calculAvg()
    {
        //Debug.Log(name + " " + positions.Count);
        if (positions.Count > 0)
        {
            Position temp = (Position)positions[0];
            double avgLat = 0;
            double avgLon = 0;
            foreach (Position position in positions)
            {
                avgLat += position.latitute;
                avgLon += position.lontitute;
            }
            this.avgLat = (float)(avgLat / positions.Count);
            this.avgLon = (float)(avgLon / positions.Count);
        }

    }
}
