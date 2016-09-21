using UnityEngine;
using System.Collections;

public class MainTest : MonoBehaviour {

    public GameObject[] planes;
    Position center;
    Map map;


    public GameObject monitorPrefab;

	// Use this for initialization
	void Start () {
        planes = new GameObject[16];
        for (int i = 0; i < 16; i++)
        {
            planes[i] = GameObject.Find("Plane" + i);
        }
        center = new Position(39.99631f, 116.3291f, new PTime(0));
        map = GameObject.Find("Directional light").GetComponent<Map>();
        map.planes = planes;
        map.Refresh(center);
        map.monitorCreator(new Vector3(0,0.05f,0), 8, 8, 5, monitorPrefab);
        for (int i = 0; i < map.planes.Length; i++)
        {
            StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
        }
        for (int i = 0; i < map.planes.Length; i++)
        {
            Debug.Log(map.points[i].latitute+","+map.points[i].lontitute);
            Position temp = Track.world2position(new VecTime(map.planes[i].transform.position, new PTime(0)), center, map.fullLat, map.fullLon);
            Debug.Log(temp.latitute+","+temp.lontitute);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
