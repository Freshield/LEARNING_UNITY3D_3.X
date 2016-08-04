using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class track_test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ArrayList tracks = LoadFile(Application.dataPath, "TrajectoryStream.txt");

        foreach (Track track in tracks)
        {
            Debug.Log(track.name);
            if (track.positions.Count > 0)
            {
                Position position = (Position)track.positions[0];
                Debug.Log(position.latitute + "," + position.lontitute + "," + position.time);
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
	
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
        sr.Close();
        sr.Dispose();
        return tracks;

    }
}

public class Track
{
    public string name;
    public ArrayList positions;

    public Track(string name)
    {
        this.name = name;
        positions = new ArrayList();
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
