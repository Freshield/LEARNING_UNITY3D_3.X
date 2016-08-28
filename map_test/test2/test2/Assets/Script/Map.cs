using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {
    public GameObject[] planes;
    public GameObject planePrefab;
    public Position centerPoint = new Position(45.49506f, -73.57801f, new PTime(0, 0));
    public int size = 512;
    public int zoom = 13;
    public int scale = 2;

    public float ratio;
    public float halfLon;
    public float fullLon;
    public float halfLat;
    public float fullLat;
    public Position[] points;
    

    public void Refresh(Position centerPoint)
    {
        this.centerPoint = centerPoint;

        ratio = Mathf.Cos(Mathf.Deg2Rad * centerPoint.latitute);
        float onesecond = ((360 * 3600) / (size * Mathf.Pow(2, zoom)));
        halfLon = (onesecond * size / 3600);
        fullLon = halfLon * 2;
        halfLat = halfLon * ratio;
        fullLat = halfLat * 2;
        
        PlaneCreator pc = new PlaneCreator(new Vector3(0, 0, 0), 6, 4, 10, planePrefab);

        planes = pc.planes;
        
        points = Position.PositionCreator(centerPoint, 6, 4, fullLat, fullLon);
        
    }



    public IEnumerator _Refresh(GameObject plane, Position center)
    {
        
        string url = "http://maps.googleapis.com/maps/api/staticmap?";
        string qs = "";
        qs += "center=" + HTTP.URL.Encode(string.Format("{0},{1}", center.latitute, center.lontitute));
        qs += "&zoom=" + zoom.ToString();
        qs += "&size=" + HTTP.URL.Encode(string.Format("{0}x{0}", size));
        qs += "&scale=2";
        qs += "&maptype=terrain";
        
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
