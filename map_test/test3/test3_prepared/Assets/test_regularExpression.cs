using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class test_regularExpression : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string line = "ADDR=1234;NAME=ZHANG;PHONE=6789";
        Regex reg = new Regex("NAME=(.+);");
        Match match = reg.Match(line);
        string value = match.Groups[1].Value;

        Dictionary<string, List<int>> companions = new Dictionary<string, List<int>>();
        string test1 = "(9784,14909,[54, 55, 56, 48, 44, 34, 36, 69, 63, 45])";

        Regex reg1 = new Regex(@"\((.+)\[");
        match = reg1.Match(test1);
        value = match.Groups[1].Value;
        MatchCollection mc = Regex.Matches(value, @"(\d+),");
        List<string> drawers = new List<string>();
        foreach (Match m in mc)
        {
            
            drawers.Add("T" + m.Groups[1].Value);
        }
        Regex reg2 = new Regex(@"\[(.+)\]\)");
        match = reg2.Match(test1);
        value = match.Groups[1].Value;
        string[] results = value.Split(',');
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

        foreach (string drawer in companions.Keys)
        {
            Debug.Log(drawer);
            foreach (int time in companions[drawer])
            {
                Debug.Log(time);
            }
        }


        string test2 = "(6602,9784,14914,[67, 61, 57, 70, 32, 36, 56, 72, 53, 71, 49, 31, 62, 48])";
        reg1 = new Regex(@"\((.+)\[");
        match = reg1.Match(test2);
        value = match.Groups[1].Value;
        mc = Regex.Matches(value, @"(\d+),");
        drawers = new List<string>();
        foreach (Match m in mc)
        {
            
            drawers.Add("T" + m.Groups[1].Value);
        }
        reg2 = new Regex(@"\[(.+)\]\)");
        match = reg2.Match(test2);
        value = match.Groups[1].Value;
        results = value.Split(',');
        times = new List<int>();
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

        foreach (string drawer in companions.Keys)
        {
            Debug.Log(drawer);
            companions[drawer].Sort();
            foreach (int time in companions[drawer])
            {
                Debug.Log(time);
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
