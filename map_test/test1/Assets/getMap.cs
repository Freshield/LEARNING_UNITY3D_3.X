using UnityEngine;
using System.Collections;

public class getMap : MonoBehaviour {

    public GameObject[] planes = new GameObject[9];
    public GMPoint centerPoint = new GMPoint(45.49506f, -73.57801f);
    public int size = 512;
    public int zoom = 13;
    public int scale = 2;

    public float ratio;
    public float halfLon;
    public float fullLon;
    public float halfLat;
    public float fullLat;
    public GMPoint[] points = new GMPoint[9];

    /*public getMap(GMPoint centerPoint)
    {
        this.centerPoint = centerPoint;

        
        ratio = Mathf.Cos(Mathf.Deg2Rad * centerPoint.latitute);
        float onesecond = ((360 * 3600) / (size * Mathf.Pow(2, zoom)));
        halfLon = (onesecond * size / 3600);
        fullLon = halfLon * 2;
        halfLat = halfLon * ratio;
        fullLat = halfLat * 2;
    }*/

    // Use this for initialization
    void Start () {
        //Debug.Log("start");

        //Refresh();
        

    }

    public void Refresh()
    {
        ratio = Mathf.Cos(Mathf.Deg2Rad * centerPoint.latitute);
        float onesecond = ((360 * 3600) / (size * Mathf.Pow(2, zoom)));
        halfLon = (onesecond * size / 3600);
        fullLon = halfLon * 2;
        halfLat = halfLon * ratio;
        fullLat = halfLat * 2;

        for (int i = 0; i < 9; i++)
        {
            planes[i] = GameObject.Find("Plane" + i);
            Debug.Log(planes[i].name);
        }

        for (int i = 0; i < planes.Length; i++)
        {

            Debug.Log(planes[i].transform.position);
        }

        Debug.Log("refresh");
        

        GMPoint leftUp = new GMPoint(centerPoint.latitute + fullLat, centerPoint.lontitute - fullLon);
        GMPoint cenUp = new GMPoint(centerPoint.latitute + fullLat, centerPoint.lontitute);
        GMPoint rightUp = new GMPoint(centerPoint.latitute + fullLat, centerPoint.lontitute + fullLon);
        GMPoint leftCen = new GMPoint(centerPoint.latitute, centerPoint.lontitute - fullLon);
        GMPoint rightCen = new GMPoint(centerPoint.latitute, centerPoint.lontitute + fullLon);
        GMPoint leftDown = new GMPoint(centerPoint.latitute - fullLat, centerPoint.lontitute - fullLon);
        GMPoint cenDown = new GMPoint(centerPoint.latitute - fullLat, centerPoint.lontitute);
        GMPoint rightDown = new GMPoint(centerPoint.latitute - fullLat, centerPoint.lontitute + fullLon);

        GMPoint[] temp = { leftUp, cenUp, rightUp, leftCen,centerPoint, rightCen, leftDown , cenDown , rightDown };
        points = temp;
        //for (int i = 0; i < points.Length; i++)
        //{
        //    Debug.Log(i);
        //    _Refresh(planes[i], points[i]);
        //}
        
        
    }



    public IEnumerator _Refresh(GameObject plane, GMPoint center) {

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
