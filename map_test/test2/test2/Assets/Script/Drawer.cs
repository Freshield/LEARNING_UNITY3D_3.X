﻿using UnityEngine;
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
    public Vector3 myPosition;

    public bool button = true;

    public bool isCompanion = false;

    //static value
    public static float playRadio = 0.5f;

    public static Vector3 companionHeight = new Vector3(0, 10, 0);
    
    // Use this for initialization
    public Drawer(GameObject objPrefab, Track track, float duration, bool isCompanion)
    {
        obj = objPrefab;
        //send obj to its first place
        obj.transform.position = track.WfirstPosition.worldPosition;

        if (isCompanion)
        {
            obj.transform.position += companionHeight;
        }

        myPosition = obj.transform.position;
        //obj.GetComponent<Renderer>().enabled = false;
        //this.track = track;

        this.duration = duration;

        WfirstPosition = track.WfirstPosition;

        if (isCompanion)
        {
            //WfirstPosition.worldPosition += companionHeight;
        }

        WlastPosition = track.WlastPosition;

        if (isCompanion)
        {
           // WlastPosition.worldPosition += companionHeight;
        }

        List<Vector3> positions = new List<Vector3>();
        List<float> durations = new List<float>();

        //avoid first duration time
        durations.Add(0.01f);
        for (int i = 0; i < track.worldPositions.Count; i++)
        {
            if (isCompanion)
            {
                positions.Add(track.worldPositions[i].worldPosition + companionHeight);
            }
            else
            {
                positions.Add(track.worldPositions[i].worldPosition);
            }
            
            if (i != 0)
            {
                durations.Add(getDuration(track.worldPositions[i - 1].time.totalTime, track.worldPositions[i].time.totalTime));
                //Debug.Log(track.name + ",firstPosition " + track.worldPositions[i - 1].worldPosition + "," +
                //    track.worldPositions[i - 1].time + ",lastPosition " +
                //    track.worldPositions[i].worldPosition + "," + track.worldPositions[i].time+
                //    ",true time is "+ getDuration(track.worldPositions[i - 1].time.totalTime, track.worldPositions[i].time.totalTime));

            }
        }

        tweener = DOTween.ToArray(() => myPosition, x => myPosition = x, positions.ToArray(), durations.ToArray());

        //tweener = obj.transform.DOPath(positions.ToArray(), duration, PathType.CatmullRom, PathMode.Full3D, 5, null);

        tweener.SetAutoKill(false).SetEase(Ease.Linear);

        if (isCompanion)
        {
            obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1,0,0, 0));
            obj.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", new Color(3f/255f,225f / 255f, 115f / 255f, 0));
        }

        //release
        positions.Clear();
        positions = null;
        durations.Clear();
        durations = null;

        //this.track.positions.Clear();
        //this.track.positions = null;
        //this.track.worldPositions.Clear();
        //this.track.worldPositions = null;
    }
    

    public void drawLine(bool isPlaying)
    {
        if (isPlaying)
        {
            tweener.Pause();
        }

        button = false;
        
        float timeNow = tweener.fullPosition;
        //float percent = timeNow / duration;
        //Vector3 nowPosition = tweener.PathGetPoint(percent);

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
