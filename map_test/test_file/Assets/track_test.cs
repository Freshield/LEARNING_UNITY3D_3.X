using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class track_test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
        Track temp;
        int pos;
        
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            if ((pos = line.IndexOf(":")) != -1)
            {
                Track track = new Track(line.Remove(pos));
                temp = track;
            }
            else if (true)
            {

            }
        }
        sr.Close();
        sr.Dispose();
        return arrlist;

    }
}

public class Track
{
    public string name;
    public Position[] positions;

    public Track(string name)
    {
        this.name = name;
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
