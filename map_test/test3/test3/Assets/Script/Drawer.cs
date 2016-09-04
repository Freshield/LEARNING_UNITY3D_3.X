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

    public bool button = true;

    public bool isCompanion = false;

    //for companion
    public Dictionary<float, CPTime> companionTimes = new Dictionary<float, CPTime>();
    public List<GameObject> lineObjects = new List<GameObject>();
    public Dictionary<int, bool> moveTimes = new Dictionary<int, bool>();

    //static value
    public static float playRadio = 0.5f;
    
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
        if (false)//isCompanion)
        {

        }
        else
        {
            if (isPlaying)
            {
                tweener.Pause();
            }

            button = false;

            float timeNow = tweener.fullPosition;

            ArrayList positions = new ArrayList();

            float count = timeNow;

            while (count > 0)
            {
                tweener.Goto(count);
                //let line near the ground
                positions.Add(myPosition - new Vector3(0, 0.48f, 0));
                //1 hour equal 2 seconds, 1 hour have 15 points, 1 point equal 0.13s
                count -= 0.13f;
            }
            tweener.Goto(timeNow);
            obj.transform.position = myPosition;

            obj.transform.FindChild("line0").GetComponent<LineRenderer>().SetVertexCount(positions.Count);
            obj.transform.FindChild("line0").GetComponent<LineRenderer>().SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));


            button = true;

            if (isPlaying)
            {
                tweener.Play();
            }

            //release
            positions.Clear();
            positions = null;
        }
        

    }

    //////////////////////////static function////////////////////////////
    public static float getDuration(float beginTime, float endTime)
    {
        return (endTime - beginTime) / (60.0f * playRadio);
    }
}
