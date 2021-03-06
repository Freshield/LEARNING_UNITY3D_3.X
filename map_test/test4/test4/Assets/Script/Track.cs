﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class Track{
    public string name;
    public float avgLat = 0;
    public float avgLon = 0;
    //for the real world position
    public Position firstPosition;
    public Position lastPosition;
    public List<Position> positions;
    //for the 3d world position
    public VecTime WfirstPosition;
    public VecTime WlastPosition;
    public List<VecTime> worldPositions;

    //init
    public Track(string name)
    {
        this.name = name;
        positions = new List<Position>();
        worldPositions = new List<VecTime>();
    }

    //calculate this track's average latitude and lontitude, 
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
        foreach (Position position in positions)
        {
            worldPositions.Add(position2world(position,center,fullLat,fullLon,objPrefab));
        }

        worldPositions.Sort();
        WfirstPosition = worldPositions[0];
        WlastPosition = worldPositions[worldPositions.Count - 1];
        
    }

    //release the track's resources
    public void clearSelf()
    {
        name = null;
        firstPosition = null;
        lastPosition = null;
        positions.Clear();
        positions = null;
        WfirstPosition = null;
        WlastPosition = null;
        worldPositions.Clear();
        worldPositions = null;
    }
    

    /////////////////////////static function/////////////////////////////

    //transfer position to world position
    public static VecTime position2world(Position position, Position center, float fullLat, float fullLon, GameObject objPrefab)
    {
        float radius = objPrefab.transform.localScale.x;
        float x = (position.lontitute - center.lontitute) * 10 / fullLon;
        float z = (position.latitute - center.latitute) * 10 / fullLat;
        VecTime temp = new VecTime(new Vector3(-x, radius, -z), new PTime(position.time.totalTime));
        return temp;
    }


    //transfer position to world position
    public static Position world2position(VecTime worldPosition, Position center, float fullLat, float fullLon)
    {
        float lontitute = (((-worldPosition.worldPosition.x) * fullLon) / 10.0f) + center.lontitute;
        float latitute = (((-worldPosition.worldPosition.z) * fullLat) / 10.0f) + center.latitute;
        Position position = new Position(latitute, lontitute, new PTime(worldPosition.time.totalTime));
        return position;
    }

    //use regular expression to read index
    //create drawer name and companion pair
    public static Dictionary<string,List<int>> LoadIndex(string path, string name)
    {
        TextAsset ta = Resources.Load<TextAsset>(path + "/" + name);
        StringReader reader = new StringReader(ta.text);


        string line;

        Dictionary<string, List<int>> companions = new Dictionary<string, List<int>>();


        Regex regName = new Regex(@"\((.+)\[");
        Regex regValue = new Regex(@"\[(.+)\]\)");
        //for each companion line
        while ((line = reader.ReadLine()) != null)
        {
            
            Match match = regName.Match(line);
            string value = match.Groups[1].Value;
            MatchCollection mc = Regex.Matches(value, @"(\d+),");
            //add the track's name
            List<string> drawers = new List<string>();
            foreach (Match m in mc)
            {
                drawers.Add("T" + m.Groups[1].Value);
            }
            
            match = regValue.Match(line);
            value = match.Groups[1].Value;
            string[] results = value.Split(',');
            //add the times
            List<int> times = new List<int>();
            foreach (string result in results)
            {
                times.Add(int.Parse(result));
            }
            try
            {
                foreach (string drawer in drawers)
                {
                    if (!companions.ContainsKey(drawer))
                    {
                        companions.Add(drawer, times);
                    }
                    else
                    {
                        companions[drawer] = companions[drawer].Union(times).ToList();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            //release
            Array.Clear(results, 0, results.Length);
        }
        
        foreach (string drawer in companions.Keys)
        {
            companions[drawer].Sort();
        }
        
        return companions;

    }

    //use regular expression to read index
    //create drawer name and companion pair
    //this one to create line between the companion
    public static Dictionary<int, List<List<string>>> LoadIndexForCompanionLine(string path, string name)
    {
        TextAsset ta = Resources.Load<TextAsset>(path + "/" + name);
        StringReader reader = new StringReader(ta.text);


        string line;

        Dictionary<int, List<List<string>>> companionLines = new Dictionary<int, List<List<string>>>();


        Regex regName = new Regex(@"\((.+)\[");
        Regex regValue = new Regex(@"\[(.+)\]\)");
        //for each companion line
        while ((line = reader.ReadLine()) != null)
        {

            Match match = regName.Match(line);
            string value = match.Groups[1].Value;
            MatchCollection mc = Regex.Matches(value, @"(\d+),");
            //add the track's name
            List<string> drawers = new List<string>();
            foreach (Match m in mc)
            {
                drawers.Add("T" + m.Groups[1].Value);
            }

            match = regValue.Match(line);
            value = match.Groups[1].Value;
            string[] results = value.Split(',');
            //add the times
            List<int> times = new List<int>();
            foreach (string result in results)
            {
                times.Add(int.Parse(result));
            }
            try
            {
                foreach (int time in times)
                {
                    if (!companionLines.ContainsKey(time))
                    {
                        companionLines.Add(time, new List<List<string>>());
                        companionLines[time].Add(drawers);
                    }
                    else
                    {
                        companionLines[time].Add(drawers);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            //release
            Array.Clear(results, 0, results.Length);
        }
        
        return companionLines;

    }


    //to read titles
    public static Track LoadTargetFileLine(string path, string name, float lineNumber)
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
        return track;
    }


    //to read file
    public static List<Track> LoadFile(string path, string name)
    {
        TextAsset ta = Resources.Load<TextAsset>(path+"/"+name);
        StringReader reader = new StringReader(ta.text);
        
        string line;
        Track temp = new Track("temp");

        List<Track> tracks = new List<Track>();

        CoordinatorChange coo = new CoordinatorChange();

        while ((line = reader.ReadLine()) != null)
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
                //release
                Array.Clear(result, 0, result.Length);
            }
        }
        temp.calculAvg();//last one to calculate
        
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
            Position avgPosition = new Position((float)avgLat, (float)avgLon, new PTime(0, 0));
            
            return new Position[] { avgPosition, firstPosition, lastPosition };
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

}
