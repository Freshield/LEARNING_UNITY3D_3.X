using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class Drawer : MonoBehaviour {

    Track track;

    GameObject obj;
    
    Tweener tweener;
    
    public float hSliderValue = 0;

    // Use this for initialization
    public Drawer(GameObject objPrefab, Track track, float duration)
    {
        obj = Instantiate(objPrefab);

        List<Vector3> positions = new List<Vector3>();

        foreach (VecTime position in track.worldPositions)
        {
            positions.Add(position.worldPosition);
        }
        
        tweener = obj.transform.DOPath(positions.ToArray(), duration, PathType.CatmullRom, PathMode.Full3D, 5, null);

        tweener.SetAutoKill(false).SetEase(Ease.Linear);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
