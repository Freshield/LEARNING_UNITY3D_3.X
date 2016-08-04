using UnityEngine;
using System.Collections;

public class getMap : MonoBehaviour {

    public GameObject[] planes = new GameObject[9];
    public Position centerPoint = new Position(45.49506f, -73.57801f,0);
    public int size = 512;
    public int zoom = 10;
    public int scale = 2;

    public float ratio;
    public float halfLon;
    public float fullLon;
    public float halfLat;
    public float fullLat;
    public float fiveperLat;
    public float fiveperLon;
    public Position[] points = new Position[9];
    

    // Use this for initialization
    void Start () {
        

    }

    public void Refresh()
    {
        ratio = Mathf.Cos(Mathf.Deg2Rad * centerPoint.latitute);
        float onesecond = ((360 * 3600) / (size * Mathf.Pow(2, zoom)));
        halfLon = (onesecond * size / 3600);
        fullLon = halfLon * 2;
        halfLat = halfLon * ratio;
        fullLat = halfLat * 2;
        fiveperLat = 0.005f * 10 / fullLat;
        fiveperLon = 0.005f * 10 / fullLon;

        for (int i = 0; i < 9; i++)
        {
            planes[i] = GameObject.Find("Plane" + i);
            
        }
        
        Debug.Log("refresh");
        

        Position leftUp = new Position(centerPoint.latitute + fullLat, centerPoint.lontitute - fullLon,0);
        Position cenUp = new Position(centerPoint.latitute + fullLat, centerPoint.lontitute, 0);
        Position rightUp = new Position(centerPoint.latitute + fullLat, centerPoint.lontitute + fullLon, 0);
        Position leftCen = new Position(centerPoint.latitute, centerPoint.lontitute - fullLon, 0);
        Position rightCen = new Position(centerPoint.latitute, centerPoint.lontitute + fullLon, 0);
        Position leftDown = new Position(centerPoint.latitute - fullLat, centerPoint.lontitute - fullLon, 0);
        Position cenDown = new Position(centerPoint.latitute - fullLat, centerPoint.lontitute, 0);
        Position rightDown = new Position(centerPoint.latitute - fullLat, centerPoint.lontitute + fullLon, 0);

        Position[] temp = { leftUp, cenUp, rightUp, leftCen,centerPoint, rightCen, leftDown , cenDown , rightDown };
        points = temp;
        
    }



    public IEnumerator _Refresh(GameObject plane, Position center) {

        Debug.Log("gettexture");
        string url = "https://maps.googleapis.com/maps/api/staticmap?";
        string qs = "";
        qs += "center=" + HTTP.URL.Encode(string.Format("{0},{1}", center.latitute, center.lontitute));
        qs += "&zoom=" + zoom.ToString();
        qs += "&size=" + HTTP.URL.Encode(string.Format("{0}x{0}", size));
        qs += "&scale=2";
        qs += "&maptype=roadmap";

        qs += "&markers=color:red|label:Y|";
        qs += "|" + string.Format("{0},{1}", center.latitute, center.lontitute);
        qs += "|" + string.Format("{0},{1}", center.latitute + halfLat, center.lontitute);
        qs += "|" + string.Format("{0},{1}", center.latitute - halfLat, center.lontitute);
        qs += "|" + string.Format("{0},{1}", center.latitute, center.lontitute + halfLon);
        qs += "|" + string.Format("{0},{1}", center.latitute, center.lontitute - halfLon);
        qs += "&key=AIzaSyAWzOOJz0eZ8bs294s_PJdfOs8nz-s9xKc";

        var req = new HTTP.Request("GET", url + qs, true);
        Debug.Log(url + qs);
        req.Send();
        while (!req.isDone)
            yield return null;
        if (req.exception == null)
        {
            var tex = new Texture2D(size, size);
            tex.LoadImage(req.response.Bytes);
            plane.GetComponent<Renderer>().material.mainTexture = tex;
        }
    }
}

/*
[System.Serializable]
public class GMPoint
{
    public float latitute;
    public float lontitute;
    public int time;

    public GMPoint(float latitute, float lontitute)
    {
        this.latitute = latitute;
        this.lontitute = lontitute;
        time = 0;
    }

    
}*/
