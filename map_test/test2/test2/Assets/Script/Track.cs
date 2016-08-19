using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Track{
    public string name;
    public List<Position> positions;
    public float avgLat = 0;
    public float avgLon = 0;
    public Position firstPosition;
    public Position lastPosition;
    public List<VecTime> worldPositions;

    public Track(string name)
    {
        this.name = name;
        positions = new List<Position>();
    }

    //calculate the average latitude and lontitude, 
    //then find first position and last position
    public void calculAvg()
    {
        if (positions.Count > 0)
        {
            firstPosition = positions[0];
            lastPosition = positions[0];
            double avgLat = 0;
            double avgLon = 0;
            foreach (Position position in positions)
            {
                avgLat += position.latitute;
                avgLon += position.lontitute;
                //get the first position and last position
                if (firstPosition.time.totalTime >= position.time.totalTime)
                {
                    firstPosition = position;
                }
                if (lastPosition.time.totalTime <= position.time.totalTime)
                {
                    lastPosition = position;
                }
            }
            this.avgLat = (float)(avgLat / positions.Count);
            this.avgLon = (float)(avgLon / positions.Count);
        }

    }

    public void getWorldPosition(Position center, float fullLat, float fullLon, GameObject prefab)
    {
        float radius = prefab.transform.localScale.x / 2;

        foreach (Position position in positions)
        {
            float x = (position.lontitute - center.lontitute) * 10 / fullLon;
            float y = (position.latitute - center.latitute) * 10 / fullLat;

            VecTime temp = new VecTime(new Vector3(x, y, radius), position.time);
            worldPositions.Add(temp);
        }
    }


    //static function
    //to read file
    public static List<Track> LoadFile(string path, string name)
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

        List<Track> tracks = new List<Track>();

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

    //calculate the average position
    //and find the first position and last position
    //sequence is center,firstPosition,lastPosition
    public static Position[] calculTracks(List<Track> tracks)
    {
        if (tracks.Count > 0)
        {
            Position firstPosition;
            Position lastPosition;
            Track temp = tracks[0];
            firstPosition = temp.firstPosition;
            lastPosition = temp.lastPosition;
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
                        if (firstPosition.time.totalTime >= track.firstPosition.time.totalTime)
                        {
                            firstPosition = track.firstPosition;
                        }
                        if (lastPosition.time.totalTime <= track.lastPosition.time.totalTime)
                        {
                            lastPosition = track.lastPosition;
                        }
                    }

                }

            }
            avgLat /= counter;
            avgLon /= counter;
            Position result = new Position((float)avgLat, (float)avgLon, new PTime(0, 0));
            return new Position[] { result, firstPosition, lastPosition };
        }
        else
        {
            return null;
        }
    }
}
