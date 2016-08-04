using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    getMap map;
    public GameObject prefab;

    // Use this for initialization
    void Start () {

        //get map
        Position center = new Position(36.52408f,117.2098f,0);

        map = GameObject.Find("Directional light").GetComponent<getMap>();
        map.centerPoint = center;
        map.Refresh();
        for (int i = 0; i < 9; i++)
        {
            
            StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
        }

        //put ball
        locateBall lb = new locateBall(center, map.fullLat, map.fullLon);
        Debug.Log("fullat,fullon" + lb.fullLat + " " + lb.fullLon);
        ArrayList locations = new ArrayList(map.points);
        ArrayList balls = new ArrayList();
        for (int i = 0; i < locations.Count; i++)
        {
            GameObject ball = Instantiate(prefab);
            balls.Add(ball);
        }
        lb.putBall(balls, locations);
        lb.locate(new GameObject("test"), new GameObject("father"), center);


    }
	
	// Update is called once per frame
	void Update () {

        /*for (int i = 0; i < 9; i++)
        {
            if (map.planes[i].GetComponent<Renderer>().material.mainTexture == null)
            {
                Debug.Log(i);
                StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
            }
            
        }*/

    }
}
