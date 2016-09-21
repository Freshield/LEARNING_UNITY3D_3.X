using UnityEngine;
using System.Collections;

public class MainTest : MonoBehaviour {

    public GameObject[] planes;
    Position center;
    Map map;

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
        for (int i = 0; i < map.planes.Length; i++)
        {
            StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
        }
        Debug.Log(map.fullLat + "," + map.fullLon);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
