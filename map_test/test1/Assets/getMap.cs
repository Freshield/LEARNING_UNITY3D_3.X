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
        float latitude = centerPoint.latitute;
        float longtitude = centerPoint.lontitute;
        ratio = Mathf.Cos(Mathf.Deg2Rad * latitude);
        float onesecond = ((360 * 3600) / (size * Mathf.Pow(2, zoom)));
        halfLon = (onesecond * size / 3600);
        fullLon = halfLon * 2;
        halfLat = halfLon * ratio;
        fullLat = halfLat * 2;

        StartCoroutine(getTexture(planes[4], centerPoint));

    }

    // Update is called once per frame
    IEnumerator getTexture (GameObject plane, GMPoint center) {
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
            yield return null;
        if (req.exception == null)
        {
            var tex = new Texture2D(size, size);
            tex.LoadImage(req.response.Bytes);
            plane.GetComponent<Renderer>().material.mainTexture = tex;
        }
    }
}

public class GMPoint
{
    public float latitute;
    public float lontitute;
}
