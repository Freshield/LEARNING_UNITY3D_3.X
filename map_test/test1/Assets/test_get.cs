using UnityEngine;
using System.Collections;

public class test_get : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void Refresh()
    {
        
        StartCoroutine(_Refresh());
    }

    IEnumerator _Refresh()
    {
        
        string qs = "https://maps.googleapis.com/maps/api/staticmap?center=45.495061,-73.578007&zoom=13&size=512x512&scale=2&maptype=roadmap&markers=color:red%7Clabel:Y%7C45.495061,-73.578007&markers=color:blue%7Clabel:Y%7C45.495061,-73.622007&markers=color:green%7Clabel:Y%7C45.495061,-73.534007&markers=color:blue%7Clabel:Y%7C45.525905,-73.578007&markers=color:yellow%7Clabel:Y%7C45.464217,-73.578007&key=AIzaSyAWzOOJz0eZ8bs294s_PJdfOs8nz-s9xKc";
        var req = new HTTP.Request("GET", qs, true);
        req.Send();
        while (!req.isDone)
            yield return null;
        if (req.exception == null)
        {
            var tex = new Texture2D(512, 512);
            tex.LoadImage(req.response.Bytes);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }




// Update is called once per frame
void Update() {

}

}
