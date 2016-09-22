using UnityEngine;
using System.Collections;

public class MainTest : MonoBehaviour {

    public GameObject[] planes;
    public Position center;
    public Map map;
    int number = 0;

    public int flow = 0;


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
        map.monitorCreator(new Vector3(0,0.05f,0), 8, 8, 5, monitorPrefab);
        /*
        for (int i = 0; i < map.planes.Length; i++)
        {
            Debug.Log(map.points[i].latitute+","+map.points[i].lontitute);
            Position temp = Track.world2position(new VecTime(map.planes[i].transform.position, new PTime(0)), center, map.fullLat, map.fullLon);
            Debug.Log(temp.latitute+","+temp.lontitute);
        }
        */
    }
	
	// Update is called once per frame
	void Update () {

        switch (flow)
        {
            case 0:
                map.Refresh(center);
                flow = 1;
                break;
            case 1:
                StartCoroutine(map._Refresh(map.planes[number], map.points[number]));
                StartCoroutine(map.cacheMap(map.planes[number], map.points[number], "temp"+number));
                number++;
                if (number >= map.planes.Length)
                {
                    number = 0;
                    flow = 2;
                }
                break;
            case 2:
                break;
            //when change
            case 3:
                map.zoom += 2;
                flow = 0;
                break;
            case 4:
                map.zoom -= 2;
                flow = 0;
                break;
            default:
                break;
        }

    }
}
