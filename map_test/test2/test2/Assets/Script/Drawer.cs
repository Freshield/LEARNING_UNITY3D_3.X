﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class Drawer : MonoBehaviour {

    Track track;

    GameObject obj;
    
    public Tweener tweener;

    float duration = 24;
    
    public float hSliderValue = 0;

    // Use this for initialization
    public Drawer(GameObject objPrefab, Track track, float duration)
    {
        obj = Instantiate(objPrefab);
        //send obj to its first place
        obj.transform.position = track.WfirstPosition.worldPosition;

        this.duration = duration;

        List<Vector3> positions = new List<Vector3>();

        foreach (VecTime position in track.worldPositions)
        {
            Debug.Log(track.name + " " + position.worldPosition+position.time);
            positions.Add(position.worldPosition);
        }
        
        tweener = obj.transform.DOPath(positions.ToArray(), duration, PathType.CatmullRom, PathMode.Full3D, 5, null);

        tweener.SetAutoKill(false).SetEase(Ease.Linear);
    }

    public void drawLine()
    {
        float timeNow = tweener.fullPosition;
        float percent = timeNow / duration;
        Vector3 nowPosition = tweener.PathGetPoint(percent);

        ArrayList positions = new ArrayList();
        
        float count = timeNow;

        while (count > 0)
        {
            float per = count / duration;
            positions.Add(tweener.PathGetPoint(per));
            count -= 0.02f;
        }

        obj.GetComponent<LineRenderer>().SetVertexCount(positions.Count);
        obj.GetComponent<LineRenderer>().SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));

    }

    //////////////////////////static function////////////////////////////
    public static float getDuration(float beginTime, float endTime)
    {
        return (endTime - beginTime) / 60.0f;
    }
}
