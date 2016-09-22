using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class Title{
    public string name;
    public float latitute;
    public float lontitute;
    public float lineNumber;

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
