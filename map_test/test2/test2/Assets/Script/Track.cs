﻿using System.Collections;
using System.IO;
using System;

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

    public static ArrayList LoadFile(string path, string name)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);

        }
        catch (Exception e)
        {
            return null;
        }

        string line;
        Track temp = new Track("temp");

        ArrayList tracks = new ArrayList();

        CoordinatorChange coo = new CoordinatorChange();

        while ((line = sr.ReadLine()) != null)
        {
            //if ((pos = line.IndexOf("T")) != -1)
            if (line.Contains("T"))
            {
                temp.calculAvg();
                //Track track = new Track(line.Remove(pos));
                Track track = new Track(line);
                temp = track;
                tracks.Add(track);
            }
            else if (line.Contains(","))
            {
                string[] result = line.Split(',');
                Position position = coo.wgs2gcj(new Position(float.Parse(result[0]), float.Parse(result[1]), new PTime(result[2])));
                temp.positions.Add(position);
            }
        }
        temp.calculAvg();//last one to calculate
        sr.Close();
        sr.Dispose();
        return tracks;
    }

    public static Position calculTracks(ArrayList tracks)
    {
        if (tracks.Count > 0)
        {
            Track temp = (Track)tracks[0];
            double avgLat = 0;
            double avgLon = 0;
            int counter = 0;
            foreach (Track track in tracks)
            {
                if (track.positions.Count > 0)
                {
                    //only for beijing project
                    if (track.avgLat < 40.1f && track.avgLat > 39.8f && track.avgLon < 116.4 && track.avgLon > 116.2)
                    {
                        counter++;
                        avgLat += track.avgLat;
                        avgLon += track.avgLon;
                    }

                }

            }
            avgLat /= counter;
            avgLon /= counter;
            Position result = new Position((float)avgLat, (float)avgLon, new PTime(0, 0));
            return result;
        }
        else
        {
            return null;
        }
    }
}
