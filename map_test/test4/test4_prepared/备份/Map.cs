using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;

public class Map : MonoBehaviour {
    public GameObject[] planes;
    public GameObject planePrefab;
    public Position centerPoint;// = new Position(45.49506f, -73.57801f, new PTime(0, 0));
    public int size = 512;
    public int zoom = 9;
    public int scale = 2;
    public float mapRadio = -1;

    public float fullLon;
    public float fullLat;
    public Position[] points;

    public void clearSelf()
    {
        Array.Clear(planes, 0, planes.Length);
        planePrefab = null;
        centerPoint = null;
        Array.Clear(points, 0, points.Length);
    }
    

    public void Refresh(Position centerPoint)
    {
        this.centerPoint = centerPoint;

        if (mapRadio == -1)
        {
            mapRadio = Mathf.Cos(Mathf.Deg2Rad * centerPoint.latitute);
        }
        
        float onesecond = ((360 * 3600) / (size * Mathf.Pow(2, zoom)));
        fullLon = (onesecond * size / 3600) * 2;
        fullLat = fullLon * mapRadio;
        
        //planes = PlaneCreator(new Vector3(0, 0, 0), 6, 4, 10, planePrefab);

        points = Position.PositionCreator(centerPoint, 4, 4, fullLat, fullLon);
        
    }


    //get the map image
    public IEnumerator _Refresh(GameObject plane, Position center)
    {

        StringBuilder url = new StringBuilder("http://maps.googleapis.com/maps/api/staticmap?");
        
        url.Append("center=").Append(WWW.UnEscapeURL(string.Format("{0},{1}", center.latitute, center.lontitute)));
        url.Append("&zoom=").Append(zoom);
        url.Append("&size=").Append(WWW.UnEscapeURL(string.Format("{0}x{0}", size)));
        url.Append("&scale=2");
        url.Append("&maptype=terrain&key=AIzaSyAWzOOJz0eZ8bs294s_PJdfOs8nz-s9xKc");
        
        var req = new WWW(url.ToString());
        yield return req;
        if (!string.IsNullOrEmpty(req.error))
        {
            Debug.Log(req.error);
        }
            
        plane.GetComponent<Renderer>().material.mainTexture = req.texture;
    }

    //cache the map
    public IEnumerator cacheMap(GameObject plane, Position center, string name)
    {
        Stream outStream = File.Create(Application.streamingAssetsPath + "/" + name + ".png");

        StringBuilder url = new StringBuilder("http://maps.googleapis.com/maps/api/staticmap?");

        url.Append("center=").Append(WWW.UnEscapeURL(string.Format("{0},{1}", center.latitute, center.lontitute)));
        url.Append("&zoom=").Append(zoom);
        url.Append("&size=").Append(WWW.UnEscapeURL(string.Format("{0}x{0}", size)));
        url.Append("&scale=2");
        url.Append("&maptype=terrain&key=AIzaSyAWzOOJz0eZ8bs294s_PJdfOs8nz-s9xKc");

        WWW www = new WWW(url.ToString());
        yield return www;
        byte[] buffer = www.bytes;
        outStream.Write(buffer, 0, buffer.Length);
        outStream.Close();
    }

    //create plane gameobject
    public GameObject[] PlaneCreator(Vector3 center, int x, int z, float width, GameObject planePrefab)
    {
        GameObject[] planes = new GameObject[x * z];

        GameObject plane_parent = new GameObject("plane_parent");

        for (int i = 0; i < z; i++)
        {
            for (int j = 0; j < x; j++)
            {
                GameObject plane = Instantiate(planePrefab);
                plane.name = "plane" + (x * i + j);
                plane.transform.parent = plane_parent.transform;
                float hor = (((x - 1) * width) / 2) - (width * j);
                float ver = (((-z + 1) * width) / 2) + (width * i);
                plane.transform.position = new Vector3(hor + center.x, center.y, ver + center.z);
                planes[x * i + j] = plane;
            }
        }

        return planes;

    }

    //create plane gameobject
    public GameObject[] monitorCreator(Vector3 center, int x, int z, float width, GameObject monitorPrefab)
    {
        GameObject[] monitors = new GameObject[x * z];

        GameObject monitor_parent = new GameObject("monitor_parent");

        for (int i = 0; i < z; i++)
        {
            for (int j = 0; j < x; j++)
            {
                GameObject monitor = Instantiate(monitorPrefab);
                monitor.name = "monitor" + (x * i + j);
                monitor.transform.parent = monitor_parent.transform;
                float hor = (((x - 1) * width) / 2) - (width * j);
                float ver = (((-z + 1) * width) / 2) + (width * i);
                monitor.transform.position = new Vector3(hor + center.x, center.y, ver + center.z);
                monitors[x * i + j] = monitor;
            }
        }

        return monitors;

    }

    /////////////////////for the companion line/////////////////////

    //for get the biggest number
    public static int getTheObjNumber(Dictionary<int, List<List<string>>> index)
    {
        int biggest = 0;
        foreach (List<List<string>> value in index.Values)
        {
            if (value.Count >= biggest)
            {
                biggest = value.Count;
            }
        }
        return biggest;
    }

    //for create empty object for companionlines
    public static void getCompanionLineObj(GameObject parent, GameObject prefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject temp = Instantiate(prefab);
            temp.name = "companionLine" + i;
            temp.transform.parent = parent.transform;
        }
    }

    //for 8x8 find the position in 4x4
    public static int monitor88Backplane44(int number)
    {
        return ((number / 8 / 2) * 4 + (number % 8 / 2));
    }
}
