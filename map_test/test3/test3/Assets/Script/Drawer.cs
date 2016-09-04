using UnityEngine;
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
        if (false)
        {
            if(isPlaying)
            {
                tweener.Pause();
            }

            ////////////////////////drawline part///////////////////////////////

            //get the time now
            float timeNow = tweener.fullPosition;
            //get how many companion before time now
            int numberOfCompanionBefore = getNumberOfCompanionBefore(timeNow);
            //temp save positions
            ArrayList positions = new ArrayList();
            //if do not have companion before
            if (numberOfCompanionBefore == 0)
            {
                drawOneLine(obj.transform.FindChild("line0").gameObject, normalMaterial, 0, timeNow);
                
            }
            else
            {
                int lineNumber = 0;
                for (int i = 0; i < numberOfCompanionBefore; i++)
                {
                    //for the first companion
                    if (i == 0)
                    {
                        //if only have one companion
                        if (numberOfCompanionBefore == 1)
                        {
                            //if timenow bigger than the only companion's endtime
                            if (timeNow > companionTimes.First().Value.endTime)
                            {
                                if (WfirstPosition.time.totalTime == companionTimes.First().Key)
                                {
                                    drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, companionTimes.First().Key, companionTimes.First().Value.endTime);
                                    lineNumber++;
                                    drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, companionTimes.First().Value.endTime, timeNow);
                                    lineNumber++;
                                }
                                else
                                {
                                    drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, WfirstPosition.time.totalTime, companionTimes.First().Key);
                                    lineNumber++;
                                    drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, companionTimes.First().Key, companionTimes.First().Value.endTime);
                                    lineNumber++;
                                    drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, companionTimes.First().Value.endTime, timeNow);
                                    lineNumber++;
                                }
                            }//if in the only companion time
                            else
                            {
                                if (WfirstPosition.time.totalTime == companionTimes.First().Key)
                                {
                                    drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, companionTimes.First().Key, timeNow);
                                    lineNumber++;
                                }
                                else
                                {
                                    drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, WfirstPosition.time.totalTime, companionTimes.First().Key);
                                    lineNumber++;
                                    drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, companionTimes.First().Key, timeNow);
                                    lineNumber++;
                                }
                            }
                            
                        }//if companion number bigger than one
                        else
                        {
                            
                            if (WfirstPosition.time.totalTime == companionTimes.First().Key)
                            {
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[0].beginTime, listCompanionTimes[0].endTime);
                                lineNumber++;
                            }
                            else
                            {
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, normalMaterial, WfirstPosition.time.totalTime, listCompanionTimes[0].beginTime);
                                lineNumber++;
                                drawOneLine(obj.transform.FindChild("line" + lineNumber).gameObject, companionMaterial, listCompanionTimes[0].beginTime, listCompanionTimes[0].endTime);
                                lineNumber++;
                            }
                        }
                        
                    }
                    
                }
            }
            
            ////////////////////////drawline part///////////////////////////////

            tweener.Goto(timeNow);
            if (isPlaying)
            {
                tweener.Play();
            }

            //release
            positions.Clear();
            positions = null;
        }
        else
        {
            if (isPlaying)
            {
                tweener.Pause();
            }
            
            float timeNow = tweener.fullPosition;

            drawOneLine(obj.transform.FindChild("line0").gameObject, normalMaterial, WfirstPosition.time.totalTime, timeNow);
            
            if (isPlaying)
            {
                tweener.Play();
            }
        }
    }

    public void drawOneLine(GameObject lineObj, Material material, float beginTime, float endTime)
    {
        ArrayList positions = new ArrayList();

        float count = endTime;

        float trueBeginTime = beginTime - WfirstPosition.time.totalTime;

        while (count > trueBeginTime)
        {
            tweener.Goto(count);
            //let line near the ground
            positions.Add(myPosition - new Vector3(0, 0.48f, 0));
            //1 hour equal 2 seconds, 1 hour have 15 points, 1 point equal 0.13s
            count -= 0.13f;
        }
        lineObj.GetComponent<LineRenderer>().material = material;
        lineObj.GetComponent<LineRenderer>().SetVertexCount(positions.Count);
        lineObj.GetComponent<LineRenderer>().SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));

        //release
        positions.Clear();
        positions = null;
        tweener.Goto(endTime);
    }

    public int getNumberOfCompanionBefore(float totaltime)
    {
        int number = 0;
        foreach (float key in companionTimes.Keys)
        {
            if (totaltime > key)
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
}
