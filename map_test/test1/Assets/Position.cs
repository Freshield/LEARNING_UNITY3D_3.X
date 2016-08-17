using UnityEngine;
using System.Collections;

public class Position
{
    public float latitute;
    public float lontitute;
    public PTime time;

    public Position(float latitute, float lontitute, PTime time)
    {
        this.latitute = latitute;
        this.lontitute = lontitute;
        this.time = time;
    }

    public static Position[] PositionCreator(Position center,int x, int y, float fullat, float fullon)
    {
        Position[] positions = new Position[x * y];

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                
                float lontitute = (((-x+1) * fullon) / 2) + (fullon * j);
                float latitute = (((y-1) * fullat) / 2) - (fullat * i);
                Position position = new Position(latitute + center.latitute, lontitute + center.lontitute,new PTime(0,0));
                positions[x * i + j] = position;
                
            }
        }

        return positions;
    }
}
