using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class read_test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ArrayList info = LoadFile(Application.dataPath, "TrajectoryStream.txt");

        foreach (string str in info)
        {
            Debug.Log(str);
        }

        string test = "Hello!!!!!:";
        Debug.Log(test);
        int pos;

        if ((pos = test.IndexOf(":")) != -1)
        {
            Debug.Log(test.Remove(pos));
        }

        test = "39.984428,116.307126,360";
        Debug.Log(test);
        string[] result = test.Split(',');
        foreach (string item in result)
        {
            Debug.Log(item);
        }

        //for (int i = 0; i < 100; i++)
        //{
        //    Debug.Log(info.);
        //}

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
        ArrayList arrlist = new ArrayList();
        int number = 0;
        while ((line = sr.ReadLine()) != null)
        {
            arrlist.Add(line);
            number++;
            if (number > 20)
            {
                break;
            }
        }
        sr.Close();
        sr.Dispose();
        return arrlist;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
