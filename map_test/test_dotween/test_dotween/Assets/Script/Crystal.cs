using UnityEngine;
using System.Collections.Generic;
using System;

public class Crystal : IComparable<Crystal>
{

    public float red;
    public float green;
    public float blue;

    public float cost;

    public List<float> beforeResults;

    public float result;

    public Crystal(float red, float green, float blue)
    {
        this.red = red;
        this.green = green;
        this.blue = blue;
        cost = red + green + blue;
        beforeResults = new List<float>();
    }

    public float calculate(float redAmount, float greenAmount, float blueAmount)
    {
        float redIncome = red * 2 - cost;
        float greenIncome = green * 3 - cost;
        float blueIncome = blue * 6 - cost;

        float answer = redIncome * redAmount + greenIncome * greenAmount + blueIncome * blueAmount;

        beforeResults.Add(answer);

        return answer;
    }

    public float averageCalaulate()
    {
        float temp = 0;

        foreach (float beforeResult in beforeResults)
        {
            temp += beforeResult;
        }

        result = temp / beforeResults.Count;

        return result;
    }

    public int CompareTo(Crystal CrystalToCompare)
    {
        if (result > CrystalToCompare.result)
        {
            return -1;
        }
        else if (result < CrystalToCompare.result)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }


}
