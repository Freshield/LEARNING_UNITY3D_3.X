﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using System.Linq;

public class Drawer
{

    //public Track track;
    public VecTime WfirstPosition;

    public VecTime WlastPosition;

    public GameObject obj;
    
    public Tweener tweener;

    public float duration = 24;

    //new version
    //the tweener controlled postion
    public Vector3 myPosition;

    public bool isCompanion = false;

    //for companion
    public Dictionary<float, CPTime> companionTimes = new Dictionary<float, CPTime>();
    public List<GameObject> lineObjects = new List<GameObject>();
    public Dictionary<int, bool> moveTimes = new Dictionary<int, bool>();
    public List<CPTime> listCompanionTimes = new List<CPTime>();

    //static value
    public static float playRadio = 0.5f;
    public static Material normalMaterial;
    public static Material companionMaterial;

    // Use this for initialization
    public Drawer(GameObject objPrefab, Track track, float duration)
    {
        obj = objPrefab;
        //send obj to its first place
        obj.transform.position = track.WfirstPosition.worldPosition;
        myPosition = obj.transform.position;
        this.duration = duration;

        WfirstPosition = track.WfirstPosition;
        WlastPosition = track.WlastPosition;
        
        List<Vector3> positions = new List<Vector3>();
        List<float> durations = new List<float>();

        //avoid first duration time
        durations.Add(0.01f);
        for (int i = 0; i < track.worldPositions.Count; i++)
        {
                positions.Add(track.worldPositions[i].worldPosition);
            
            if (i != 0)
            {
                durations.Add(getDuration(track.worldPositions[i - 1].time.totalTime, track.worldPositions[i].time.totalTime));
            }
        }

        tweener = DOTween.ToArray(() => myPosition, x => myPosition = x, positions.ToArray(), durations.ToArray());
        
        tweener.SetAutoKill(false).SetEase(Ease.Linear).Pause();
        
        //release
        positions.Clear();
        positions = null;
        durations.Clear();
        durations = null;
        
    }

    //to merge the continue times
    public void getCompanionTimes(List<int> index)
    {
        index.Sort();
        int firstValue = index[0];
        int lastValue = index[0];
        CPTime temp = new CPTime();

        foreach (int value in index)
        {
            moveTimes.Add(value, true);
        }

        for (int i = 0; i <= index.Count; i++)
        {
            if (i == 0)
            {
                firstValue = index[i];
                lastValue = index[i];
            }
            else if (i == index.Count)
            {
                temp = new CPTime(firstValue, lastValue);
                companionTimes.Add(temp.beginTime, temp);
            }
            else
            {
                if (index[i] - index[i - 1] == 1)
                {
                    lastValue = index[i];
                }
                else
                {
                    temp = new CPTime(firstValue, lastValue);
                    companionTimes.Add(temp.beginTime, temp);
                    firstValue = index[i];
                    lastValue = index[i];
                }
            }
        }

        listCompanionTimes = companionTimes.Values.ToList();
    }

    public int getObjectNumber()
    {
        if (isCompanion)
        {
            int count = 0;
            if (WfirstPosition.time.totalTime != companionTimes[companionTimes.Keys.First()].beginTime)
            {
                count++;
            }
            if (WlastPosition.time.totalTime != companionTimes[companionTimes.Keys.Last()].endTime)
            {
                count++;
            }
            count += (2 * companionTimes.Keys.Count) - 1;
            return count;
        }
        else
        {
            return 1;
        }
    }
    

    public void drawLine(bool isPlaying)
    {
        if (isCompanion)
        {
            
            if(isPlaying)
            {
                tweener.Pause();
            }

            ////////////////////////drawline part///////////////////////////////

            //get the time now
            float timeNow = tweener.fullPosition;
            //change to worldTime
            float worldTimeNow = timeNow2worldTime(timeNow, WfirstPosition.time.totalTime);
            //get how many companion before time now
            int numberOfCompanionBefore = getNumberOfCompanionBefore(worldTimeNow);
            //temp save positions
            ArrayList positions = new ArrayList();
            //if do not have companion before
            if (numberOfCompanionBefore == 0)
            {
                drawOneLine(obj.transform.FindChild("line0").gameObject, normalMaterial, WfirstPosition.time.totalTime, worldTimeNow);
                
            }
            else
            {
                int lineNumber = 0;
                //if only have one companion
                if (numberOfCompanionBefore == 1)
                {
                    
                    //if timenow bigger than the only companion's endtime
                    if (worldTimeNow > listCompanionTimes[0].endTime)
                    {
                        
                        if (WfirstPosition.time.totalTime == listCompanionTimes[0].beginTime)
                        {
                            Debug.Log(obj.name + " 1 ");
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[0].beginTime, listCompanionTimes[0].endTime);
                            lineNumber++;
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, listCompanionTimes[0].endTime, worldTimeNow);
                            lineNumber++;
                        }
                        else
                        {
                            Debug.Log(obj.name + " 2 ");
                            
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, WfirstPosition.time.totalTime, listCompanionTimes[0].beginTime);
                            lineNumber++;
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[0].beginTime, listCompanionTimes[0].endTime);
                            lineNumber++;
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, listCompanionTimes[0].endTime, worldTimeNow);
                            lineNumber++;
                        }
                    }//if in the only companion time
                    else
                    {
                        if (WfirstPosition.time.totalTime == listCompanionTimes[0].beginTime)
                        {
                            Debug.Log(obj.name + " 3 ");
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[0].beginTime, worldTimeNow);
                            lineNumber++;
                        }
                        else
                        {
                            Debug.Log(obj.name + " 4 ");
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, WfirstPosition.time.totalTime, listCompanionTimes[0].beginTime);
                            lineNumber++;
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[0].beginTime, worldTimeNow);
                            lineNumber++;
                        }
                    }
                }//if companion number bigger than one
                else
                {
                    Debug.Log(obj.name + " 5 ");
                    

                    for (int i = 0; i < numberOfCompanionBefore; i++)
                    {
                        if (i == 0)
                        {
                            if (WfirstPosition.time.totalTime == companionTimes.First().Key)
                            {
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[0].beginTime, listCompanionTimes[0].endTime);
                                lineNumber++;
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, listCompanionTimes[0].endTime, listCompanionTimes[1].beginTime);
                                lineNumber++;
                            }
                            else
                            {
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, WfirstPosition.time.totalTime, listCompanionTimes[0].beginTime);
                                lineNumber++;
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[0].beginTime, listCompanionTimes[0].endTime);
                                lineNumber++;
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, listCompanionTimes[0].endTime, listCompanionTimes[1].beginTime);
                                lineNumber++;
                            }
                        }
                        else if (i != (numberOfCompanionBefore - 1))
                        {
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[i].beginTime, listCompanionTimes[i].endTime);
                            lineNumber++;
                            drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, listCompanionTimes[i].endTime, listCompanionTimes[i + 1].beginTime);
                            lineNumber++;
                        }
                        else
                        {
                            Debug.Log(obj.name + " 6 ");
                            Debug.Log(obj.name + " " + worldTimeNow + ":" + ":" + listCompanionTimes[i].beginTime + ":" + listCompanionTimes[i].endTime + ":" + WfirstPosition.time.totalTime);
                            //if timenow bigger then the endtime
                            if (worldTimeNow > listCompanionTimes[i].endTime)
                            {
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[i].beginTime, listCompanionTimes[i].endTime);
                                lineNumber++;
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, listCompanionTimes[i].endTime, worldTimeNow);
                                lineNumber++;
                            }
                            else
                            {
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[i].beginTime, worldTimeNow);
                                lineNumber++;
                            }
                        }
                    }
                    
                }
            }
            
            ////////////////////////drawline part///////////////////////////////
            
            if (isPlaying)
            {
                tweener.Play();
            }
        }
        else
        {
            if (isPlaying)
            {
                tweener.Pause();
            }
            
            float timeNow = tweener.fullPosition;
            float worldTimeNow = timeNow2worldTime(timeNow, WfirstPosition.time.totalTime);

            drawOneLine(obj.transform.FindChild("line0").gameObject, normalMaterial, WfirstPosition.time.totalTime, worldTimeNow);
            
            if (isPlaying)
            {
                tweener.Play();
            }
        }
    }

    //only use the absolute time
    public void drawOneLine(GameObject lineObj, Material material, float beginTime, float endTime)
    {
        ArrayList positions = new ArrayList();

        float realBeginTime = getDuration(WfirstPosition.time.totalTime, beginTime);
        float realEndTime = getDuration(WfirstPosition.time.totalTime, endTime);

        float count = realEndTime;
        
        while (count > realBeginTime)
        {
            tweener.Goto(count);
            //let line near the ground
            positions.Add(myPosition - new Vector3(0, 0.48f, 0));
            //1 hour equal 2 seconds, 1 hour have 15 points, 1 point equal 0.13s, 4 minute equal 4 points
            count -= 0.13f;
        }
        //clean linerenderer
        lineObj.GetComponent<LineRenderer>().SetVertexCount(0);

        lineObj.GetComponent<LineRenderer>().material = material;
        lineObj.GetComponent<LineRenderer>().SetVertexCount(positions.Count);
        lineObj.GetComponent<LineRenderer>().SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));

        tweener.Goto(realEndTime);
        //release
        positions.Clear();
        positions = null;
    }

    public int getNumberOfCompanionBefore(float totaltime)
    {
        int number = 0;
        foreach (CPTime time in listCompanionTimes)
        {
            if (totaltime > time.beginTime)
            {
                number++;
            }
        }
        return number;
    }
    

    //////////////////////////static function////////////////////////////
    public static float getDuration(float beginTime, float endTime)
    {
        return (endTime - beginTime) / (60.0f * playRadio);
    }

    public static float timeNow2worldTime(float timeNow, float beginTime)
    {
        return (timeNow * 60.0f * playRadio) + beginTime;
    }
    
}
