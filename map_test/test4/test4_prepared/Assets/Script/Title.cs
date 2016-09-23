using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class Title{
    public string name;
    public float latitute;
    public float lontitute;
    public float lineNumber;
    public int level_9;
    public int level_11;
    public int level_13;

    public Title(string name)
    {
        this.name = name;
    }

    public Title(string name, float latitute, float lontitute, float lineNumber)
    {
        this.name = name;
        this.latitute = latitute;
        this.lontitute = lontitute;
        this.lineNumber = lineNumber;
    }
    //create zoom 9,11,13,15 level
    public void getLevelPosition(Position center, float fullLat, float fullLon)
    {
        //for zoom9
        //need minus value
        float x = (lontitute - center.lontitute) * -10 / fullLon;
        float z = (latitute - center.latitute) * -10 / fullLat;
        Debug.Log("origin "+x + "," + z);
        //change to a new coordinator, x from 0 to 40, z from 0 to 40
        float x9 = (x - 20) * (-1);
        float z9 = z + 20;
        Debug.Log(x9 / 10);
        Debug.Log(z9 / 10);
        level_9 = (((int)x9 / 10) + (((int)z9 / 10) * 4));
        float leftPoint9x = (level_9 % 4) * 10;
        float upPoint9z = (level_9 / 4) * 10;
        Debug.Log("zoom9 " + x9 + "," + z9);
        Debug.Log("leftup " + leftPoint9x + "," + upPoint9z);

        //for zoom11
        float x11 = (x9 - leftPoint9x) * 4;
        float z11 = (z9 - upPoint9z) * 4;
        level_11 = (((int)x11 / 10) + (((int)z11 / 10) * 4));
        float leftPoint11x = (level_11 % 4) * 10;
        float upPoint11z = (level_11 / 4) * 10;
        Debug.Log("zoom11 " + x11 + "," + z11);
        Debug.Log("leftup " + leftPoint11x + "," + upPoint11z);

        //for zoom13
        float x13 = (x11 - leftPoint11x) * 4;
        float z13 = (z11 - upPoint11z) * 4;
        level_13 = (((int)x13 / 10) + (((int)z13 / 10) * 4));
        Debug.Log("zoom13 " + x13 + "," + z13);
    }

    ////////////////static function////////////////////
    //create title, list[0] is title, list[1] is next linenumber
    public static List<object> getTitle(string path, string name, float lineNumber)
    {
        TextAsset ta = Resources.Load<TextAsset>(path + "/" + name);
        StringReader reader = new StringReader(ta.text);

        string line;

        float beginNumber = 0;

        Track track = new Track("temp");
        
        CoordinatorChange coo = new CoordinatorChange();
        float count = lineNumber;
        //skip to the target line
        for (int i = 0; i < count; i++)
        {
            reader.ReadLine();
        }
        //for read the name
        while (true)
        {
            line = reader.ReadLine();
            if (line != null)
            {
                if (line.Contains("T"))
                {
                    track = new Track(line);
                    beginNumber = count;
                    count++;
                    break;
                }
                else
                {
                    Debug.Log("the line " + count + " do not have T, see the next line.");
                    count++;
                }
            }
            else
            {
                count = -1;
                break;
            }
            
        }

        if (count != -1)
        {
            //for create positions
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains(","))
                {
                    string[] result = line.Split(',');
                    Position position = coo.wgs2gcj(new Position(float.Parse(result[0]), float.Parse(result[1]), new PTime(result[2])));
                    track.positions.Add(position);
                    count++;
                    //release
                    Array.Clear(result, 0, result.Length);
                }
                else if (line.Contains("T"))
                {
                    break;
                }
                else
                {
                    Debug.Log("face some problem in line " + count);
                }
            }
            track.calculAvg();

        }
        

        Title title = new Title(track.name, track.avgLat, track.avgLon, beginNumber);

        List<object> temp = new List<object>();

        temp.Add(title);
        temp.Add(count);

        return temp;
    }
}
