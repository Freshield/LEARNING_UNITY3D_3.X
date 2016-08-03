using UnityEngine;
using System.Collections;

public class test_get : MonoBehaviour
{
    public enum MapType
    {
        RoadMap,
        Satellite,
        Terrain,
        Hybrid
    }
    public bool loadOnStart = true;
    public CGMapLocation centerLocation;
    public int zoom = 13;
    public MapType mapType;
    public int size = 512;

    void Start()
    {
        if (loadOnStart) Refresh();
    }

    public void Refresh()
    {
        
        StartCoroutine(_Refresh());
    }

    IEnumerator _Refresh()
    {
        float latitude = 45.495061f;
        float longtitude = -73.578007f;
        float ratio = Mathf.Cos(Mathf.Deg2Rad * latitude);
        float onesecond = ((360 * 3600) / (512 * Mathf.Pow(2, 13)));
        float halfimagedegree = (onesecond * 512 / 3600);
        float fullimagedegree = halfimagedegree * 2;
        float halfla = halfimagedegree * ratio;
        float fullla = halfla * 2;

        Debug.Log("centra latitude = " + centerLocation.latitude);
        centerLocation.latitude += fullla;
        Debug.Log("centra latitude = " + centerLocation.latitude);


        string url = "https://maps.googleapis.com/maps/api/staticmap?";
        string qs = "";
        qs += "center=" + HTTP.URL.Encode(string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude));
        qs += "&zoom=" + zoom.ToString();
        qs += "&size=" + HTTP.URL.Encode(string.Format("{0}x{0}", size));
        qs += "&scale=2";
        qs += "&maptype=" + mapType.ToString().ToLower();
        qs += "&markers=color:red|label:Y|";
        qs += "|" + string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude);
        qs += "|" + string.Format("{0},{1}", centerLocation.latitude + halfla, centerLocation.longitude);
        qs += "|" + string.Format("{0},{1}", centerLocation.latitude - halfla, centerLocation.longitude);
        qs += "|" + string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude + halfimagedegree);
        qs += "|" + string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude - halfimagedegree);

        qs += "&key=AIzaSyAWzOOJz0eZ8bs294s_PJdfOs8nz-s9xKc";

        //qs = "https://maps.googleapis.com/maps/api/staticmap?center=45.495061,-73.578007&zoom=13&size=512x512&scale=2&maptype=roadmap&markers=color:red%7Clabel:Y%7C45.495061,-73.578007&markers=color:blue%7Clabel:Y%7C45.495061,-73.622007&markers=color:green%7Clabel:Y%7C45.495061,-73.534007&markers=color:blue%7Clabel:Y%7C45.525905,-73.578007&markers=color:yellow%7Clabel:Y%7C45.464217,-73.578007&key=AIzaSyAWzOOJz0eZ8bs294s_PJdfOs8nz-s9xKc";
        var req = new HTTP.Request("GET", url + qs, true);
        Debug.Log(url + qs);
        req.Send();
        while (!req.isDone)
            yield return null;
        if (req.exception == null)
        {
            var tex = new Texture2D(size, size);
            tex.LoadImage(req.response.Bytes);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }


}


[System.Serializable]
public class CGMapLocation
{
    public string address;
    public float latitude;
    public float longitude;
}

[System.Serializable]
public class CGMapMarker
{
    public enum GoogleMapMarkerSize
    {
        Tiny,
        Small,
        Mid
    }
    public GoogleMapMarkerSize size;
    public GoogleMapColor color;
    public string label;
    public GoogleMapLocation[] locations;

}
