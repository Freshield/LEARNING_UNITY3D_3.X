using UnityEngine;
using System.Collections.Generic;
using System;

public class test_crystal : MonoBehaviour {

    List<Crystal> crystals;

    List<Solutions> solutions;

	// Use this for initialization
	void Start () {

        crystals = new List<Crystal>();

        solutions = new List<Solutions>();

        solutions.Add(new Solutions(48, 24, 9));
        solutions.Add(new Solutions(40, 32, 12));
        solutions.Add(new Solutions(38, 36, 10));

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                for (int m = 0; m < 6; m++)
                {
                    Crystal temp = new Crystal(i, j, m);
                    crystals.Add(temp);
                }
            }
        }

        foreach (Solutions solution in solutions)
        {
            Debug.Log("NOW the situation is :red " + solution.redAmount + ",green " + solution.greenAmount + ",blue "+solution.blueAmount);


            foreach (Crystal crystal in crystals)
            {
                crystal.calculate(solution.redAmount, solution.greenAmount, solution.blueAmount);

            }

        }

        foreach (Crystal crystal in crystals)
        {
            crystal.averageCalaulate();
            if (crystal.red == 5 && crystal.green == 5 && crystal.blue == 0)
            {
                foreach (float beforeResult in crystal.beforeResults)
                {
                    Debug.Log("The crystal is: red " + crystal.red + ",green " + crystal.green + ",blue " +
                crystal.blue + ",the beforeResult is: " + beforeResult);
                    
                }
            }
        }

        crystals.Sort();

        for (int i = 0; i < 5; i++)
        {
            Debug.Log("The crystal is: red " + crystals[i].red + ",green " + crystals[i].green + ",blue " +
                crystals[i].blue + ",the result is: " + crystals[i].result);
        }

    }

    // Update is called once per frame
    void Update () {
	
	}
}
