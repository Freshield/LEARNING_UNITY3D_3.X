using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

public class test_regularExpression : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string line = "ADDR=1234;NAME=ZHANG;PHONE=6789";
        Regex reg = new Regex("NAME=(.+);");
        Match match = reg.Match(line);
        string value = match.Groups[1].Value;
        Debug.Log(value);
        
        string test1 = "(9784,14909,[54, 55, 56, 48, 44, 34, 36, 69, 63, 45])";
        string test2 = "(6602,9789,14914,[67, 61, 57, 70, 32, 36, 56, 72, 53, 71, 49, 31, 62, 48])";
        Regex reg1 = new Regex(@"\((.+)\[");
        match = reg1.Match(test1);
        value = match.Groups[1].Value;
        Debug.Log(value);
        MatchCollection mc = Regex.Matches(value, @"(\d+),");
        foreach (Match m in mc)
        {
            Debug.Log(m.Groups[1].Value);
        }
        Regex reg2 = new Regex(@"\[(.+)\]\)");
        match = reg2.Match(test1);
        value = match.Groups[1].Value;
        Debug.Log(value);
        string[] results = value.Split(',');
        foreach (string result in results)
        {
            
            Debug.Log(int.Parse(result));
        }

        reg1 = new Regex(@"\((.+)\[");
        match = reg1.Match(test2);
        value = match.Groups[1].Value;
        Debug.Log(value);
        mc = Regex.Matches(value, @"(\d+),");
        foreach (Match m in mc)
        {
            Debug.Log(m.Groups[1].Value);
        }
        reg2 = new Regex(@"\[(.+)\]\)");
        match = reg2.Match(test2);
        value = match.Groups[1].Value;
        Debug.Log(value);
        results = value.Split(',');
        foreach (string result in results)
        {

            Debug.Log(int.Parse(result));
        }

        Dictionary<string, List<Student>> students = new Dictionary<string, List<Student>>();

        try
        {
            for (int i = 0; i < 10; i++)
            {
                if (!students.ContainsKey(i.ToString()))
                {
                    students.Add(i.ToString(), new List<Student>() { new Student(i) });
                }
                else
                {
                    students[i.ToString()].Add(new Student(i+20));
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (!students.ContainsKey(i.ToString()))
                {
                    students.Add(i.ToString(), new List<Student>() { new Student(i) });
                }
                else
                {
                    students[i.ToString()].Add(new Student(i + 20));
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

        foreach (string key in students.Keys)
        {
            Debug.Log("key is " + key);
            foreach (Student student in students[key])
            {
                Debug.Log("values are " + student.id);
            }
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class Student
{
    public int id;
    public Student(int id)
    {
        this.id = id;
    }
}
