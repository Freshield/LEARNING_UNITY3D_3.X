using UnityEngine;
using System.Collections;

public class getMap : MonoBehaviour {

    public GameObject[] planes;
    public GMPoint centerPoint;
    public int size;
    public int zoom;
    public int scale;

    float ratio;
    float halfLon;
    float fullLon;
    float halfLat;
    float fullLat;

    // Use this for initialization
    void Start () {
        Debug.Log("start");

        Refresh();
        

    }

    public void Refresh()
    {
        Debug.Log("refresh");
        float latitude = centerPoint.latitute;
        float longtitude = centerPoint.lontitute;
        ratio = Mathf.Cos(Mathf.Deg2Rad * latitude);
        float onesecond = ((360 * 3600) / (size * Mathf.Pow(2, zoom)));
        halfLon = (onesecond * size / 3600);
        fullLon = halfLon * 2;
        halfLat = halfLon * ratio;
        fullLat = halfLat * 2;

        GMPoint leftUp = new GMPoint(centerPoint.latitute + fullLat, centerPoint.lontitute - fullLat);
        GMPoint cenUp = new GMPoint(centerPoint.latitute + fullLat, centerPoint.lontitute);
        GMPoint rightUp = new GMPoint(centerPoint.latitute + fullLat, centerPoint.lontitute + fullLat);
        GMPoint leftCen = new GMPoint(centerPoint.latitute, centerPoint.lontitute - fullLat);
        GMPoint rightCen = new GMPoint(centerPoint.latitute, centerPoint.lontitute + fullLat);
        GMPoint leftDown = new GMPoint(centerPoint.latitute - fullLat, centerPoint.lontitute - fullLat);
        GMPoint cenDown = new GMPoint(centerPoint.latitute - fullLat, centerPoint.lontitute);
        GMPoint rightDown = new GMPoint(centerPoint.latitute - fullLat, centerPoint.lontitute + fullLat);

        
        getTexture(planes[4],centerPoint);
    }

    

    void getTexture (GameObject plane, GMPoint center) {

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
        qs += "&key=AIzaSyAWzOOJz0eZ8bs294s_PJdfOs8nz-s9xKc";

        var req = new HTTP.Request("GET", url + qs, true);
        Debug.Log(url + qs);
        req.Send();
        while (!req.isDone)
        { }
        if (req.exception == null)
        {
            var tex = new Texture2D(size, size);
            tex.LoadImage(req.response.Bytes);
            plane.GetComponent<Renderer>().material.mainTexture = tex;
        }
    }
}

[System.Serializable]
public class GMPoint
{
    public float latitute;
    public float lontitute;

    public GMPoint(float latitute, float lontitute)
    {
        this.latitute = latitute;
        this.lontitute = lontitute;
    }

    
}
