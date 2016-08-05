using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class track_test : MonoBehaviour {

	// Use this for initialization
	void Start () {

       // ArrayList tracks = LoadFile(Application.dataPath, "TrajectoryStream.txt");

       // Position center = calculTracks(tracks);

       // Debug.Log("latitute,longtitute: " + center.latitute + "," + center.lontitute);
        

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public Position calculTracks(ArrayList tracks)
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
                        Debug.Log("name: " + track.name + "lat,lon" + track.avgLat + "," + track.avgLon + " now avg " + avgLat + "," + avgLon + " counter " + counter);
                    }
                    
                }
                
            }
            Debug.Log("before is " + avgLat + "," + avgLon + " counter " + counter);
            avgLat /= counter;
            avgLon /= counter;
            Debug.Log("after is " + avgLat + "," + avgLon + " counter " + counter);
            Position result = new Position((float)avgLat, (float)avgLon, 0);

            return result;
        }
        else
        {
            return null;
        }
        
        
    }


    public ArrayList LoadFile(string path, string name)
    {
        StreamReader sr = null;
        Debug.Log(path + "/" + name);
        
        try
        {
        sr = File.OpenText(path + "//" + name);

        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }

        string line;
        Track temp = new Track("temp");
        int pos;
        
        ArrayList tracks = new ArrayList();

        coordianator coo = new coordianator();

        while ((line = sr.ReadLine()) != null)
        {
            if ((pos = line.IndexOf(":")) != -1)
            {
                temp.calculAvg();
                Track track = new Track(line.Remove(pos));
                temp = track;
                tracks.Add(track);
            }
            else if (line.Contains(","))
            {
                string[] result = line.Split(',');
                Position position = coo.wgs2gcj(new Position(float.Parse(result[0]), float.Parse(result[1]), int.Parse(result[2])));
                temp.positions.Add(position);
            }
        }
        temp.calculAvg();//last one to calculate

        sr.Close();
        sr.Dispose();
        return tracks;

    }
}
public class Track
{
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
        Debug.Log(name + " " + positions.Count);
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

public class Position
{
    public float latitute;
    public float lontitute;
    public int time;

    public Position(float latitute,float lontitute,int time)
    {
        this.latitute = latitute;
        this.lontitute = lontitute;
        this.time = time;
    }
}
