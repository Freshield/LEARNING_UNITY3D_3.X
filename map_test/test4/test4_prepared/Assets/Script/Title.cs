using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class Title{
    public string name;
    public float latitute;
    public float lontitute;
    public float lineNumber;
    public string level_1 = "";
    public string level_0 = "";
    public string level_2 = "";

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
    //create zoom 10,13,16 level
    //for monitor
    public void getMonitorLevelPosition(Position center, float fullLat, float fullLon)
    {
        //for zoom9
        //need minus value
        float x = (lontitute - center.lontitute) * -10 / fullLon;
        float z = (latitute - center.latitute) * -10 / fullLat;
        //change to a new coordinator, x from 0 to 40, z from 0 to 40
        float x0 = (x - 20) * (-1);
        float z0 = z + 20;
        if (x0 < 40 && x0 > 0 && z0 < 40 && z0 > 0)
        {
            int temp_0 = (((int)x0 / 5) + (((int)z0 / 5) * 8));
            level_0 = temp_0.ToString();
            float leftPoint0x = (temp_0 % 8) * 5;
            float upPoint0z = (temp_0 / 8) * 5;

            //for zoom1
            float x1 = (x0 - leftPoint0x) * 8;
            float z1 = (z0 - upPoint0z) * 8;
            int temp_1 = (((int)x1 / 5) + (((int)z1 / 5) * 8));
            float leftPoint1x = (temp_1 % 8) * 5;
            float upPoint1z = (temp_1 / 8) * 5;
            level_1 = temp_0 + "" + temp_1;

            //for zoom2
            float x2 = (x1 - leftPoint1x) * 8;
            float z2 = (z1 - upPoint1z) * 8;
            int temp_2 = (((int)x2 / 5) + (((int)z2 / 5) * 8));
            level_2 = temp_0 + "" + temp_1 + "" + temp_2;

        }
        else
        {
            level_0 = "-1";
            level_1 = "-1-1";
            level_2 = "-1-1-1";
        }

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
