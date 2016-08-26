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
    public VecTime WfirstPosition;
    public VecTime WlastPosition;
    public List<VecTime> worldPositions;

    public Track(string name)
    {
        this.name = name;
        positions = new List<Position>();
        worldPositions = new List<VecTime>();
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
    
    //save the world position
    public void getWorldPosition(Position center, float fullLat, float fullLon, GameObject objPrefab)
    {
        float radius = objPrefab.transform.localScale.x / 2;

        foreach (Position position in positions)
        {
            worldPositions.Add(position2world(position,center,fullLat,fullLon,objPrefab));
        }

        worldPositions.Sort();
        WfirstPosition = worldPositions[0];
        WlastPosition = worldPositions[worldPositions.Count - 1];
        
    }

    //track do filling between firstposition and lastposition
    public void trackFilling(VecTime WfirstPosition, VecTime WlastPosition)
    {
        //figure out if track have first or last position
        if (this.WfirstPosition.time.totalTime != WfirstPosition.time.totalTime)
        {
            VecTime temp = new VecTime(this.WfirstPosition.worldPosition, WfirstPosition.time);
            worldPositions.Add(temp);
            //release
            temp = null;
        }
        if (this.WlastPosition.time.totalTime != WlastPosition.time.totalTime)
        {
            VecTime temp = new VecTime(this.WlastPosition.worldPosition, WlastPosition.time);
            worldPositions.Add(temp);
            //release
            temp = null;
        }
        
        VecTime.filling(worldPositions);
    }


    /////////////////////////static function/////////////////////////////

    //transfer position to world position
    public static VecTime position2world(Position position, Position center, float fullLat, float fullLon, GameObject objPrefab)
    {
        float radius = objPrefab.transform.localScale.x / 2;
        float x = (position.lontitute - center.lontitute) * 10 / fullLon;
        float y = (position.latitute - center.latitute) * 10 / fullLat;
        VecTime temp = new VecTime(new Vector3(x, y, -radius), new PTime(position.time.totalTime));
        return temp;
    }

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
                //release
                track = null;
            }
            else if (line.Contains(","))
            {
                string[] result = line.Split(',');
                Position position = coo.wgs2gcj(new Position(float.Parse(result[0]), float.Parse(result[1]), new PTime(result[2])));
                temp.positions.Add(position);
                //release
                position = null;
                Array.Clear(result, 0, result.Length);
            }
        }
        temp.calculAvg();//last one to calculate
        sr.Close();
        sr.Dispose();
        //release
        temp = null;
        coo = null;
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

    //give every track in tracks their world position
    public static void generateWorldPosition(List<Track> tracks, Position center, float fullLat, float fullLon, GameObject objPrefab)
    {

        foreach (Track track in tracks)
        {
            track.getWorldPosition(center, fullLat, fullLon, objPrefab);
        }

    }

    //whole tracks do filling between firstposition and lastposition
    public static void wholeTracksFilling(VecTime WfirstPostion, VecTime WlastPosition, List<Track> tracks)
    {
        foreach (Track track in tracks)
        {
            //old version, filling whole first and last position,
            //seem it is not working for dotween
            //track.trackFilling(WfirstPostion, WlastPosition);
            track.trackFilling(track.WfirstPosition, track.WlastPosition);
        }
    }
}
