using UnityEngine;
using System.Collections;
using System.IO;

public class write_test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        CreateFile(Application.dataPath, "FileName.txt", "TestInfo0233");
        CreateFile(Application.dataPath, "FileName.txt", "TestInfo1233");
        CreateFile(Application.dataPath, "FileName.txt", "TestInfo2233");

    }

    void CreateFile(string path, string name, string info)
    {
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }

        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
