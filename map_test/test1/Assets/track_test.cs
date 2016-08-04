using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class track_test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ArrayList tracks = LoadFile(Application.dataPath, "TrajectoryStream.txt");

        Position center = calculTracks(tracks);

        Debug.Log("latitute,longtitute: " + center.latitute + "," + center.lontitute);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    Position calculTracks(ArrayList tracks)
    {
        if (tracks.Count > 0)
        {
            Track temp = (Track)tracks[0];
            float avgLat = temp.avgLat;
            float avgLon = temp.avgLon;
            foreach (Track track in tracks)
            {
                avgLat = (avgLat + track.avgLat) / 2;
                avgLon = (avgLon + track.avgLon) / 2;
                Debug.Log("name: " + track.name + "lat,lon" + avgLat + "," + avgLon);
            }
            Position result = new Position(avgLat, avgLon, 0);

            return result;
        }
        else
        {
            return null;
        }
        
        
    }
    ArrayList LoadFile(string path, string name)
    {
        StreamReader sr = null;
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
                Position position = new Position(float.Parse(result[0]), float.Parse(result[1]), int.Parse(result[2]));
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
    public float avgLat;
    public float avgLon;

    public Track(string name)
    {
        this.name = name;
        positions = new ArrayList();
    }

    public void calculAvg()
    {
        if (positions.Count > 0)
        {
            Position temp = (Position)positions[0];
            float avgLat = temp.latitute;
            float avgLon = temp.lontitute;
            foreach (Position position in positions)
            {
                avgLat = (avgLat + position.latitute) / 2;
                avgLon = (avgLon + position.lontitute) / 2;
            }
            this.avgLat = avgLat;
            this.avgLon = avgLon;
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
