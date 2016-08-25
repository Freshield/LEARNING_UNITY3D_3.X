using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class Drawer{

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
    

    public void drawLine(bool isPlaying)
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
            positions.Add(myPosition);
            count -= 0.03f;
        }
        tweener.Goto(timeNow);
        obj.transform.position = myPosition;

        obj.GetComponent<LineRenderer>().SetVertexCount(positions.Count);
        obj.GetComponent<LineRenderer>().SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));


        button = true;

        if (isPlaying)
        {
            tweener.Play();
        }

        //release
        positions.Clear();
        positions = null;

    }

    //////////////////////////static function////////////////////////////
    public static float getDuration(float beginTime, float endTime)
    {
        return (endTime - beginTime) / (60.0f * playRadio);
    }
}
