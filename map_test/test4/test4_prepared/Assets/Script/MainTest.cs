using UnityEngine;
using System.Collections.Generic;

public class MainTest : MonoBehaviour {

    List<Texture> planeTextures;
    public GameObject[] planes;
    public Position center;
    public Map map;
    int number = 0;
    float lineNumber = 0;

    public int flow = 0;
    float planeWaitTime = 1;

    public bool mainLock = false;


    public GameObject monitorPrefab;

    MouseTest mouseTest;

	// Use this for initialization
	void Start () {
        mouseTest = GameObject.Find("Main Camera").GetComponent<MouseTest>();
        planes = new GameObject[16];
        planeTextures = new List<Texture>();
        for (int i = 0; i < 16; i++)
        {
            planes[i] = GameObject.Find("Plane" + i);
        }
        center = new Position(39.99631f, 116.3291f, new PTime(0));
        map = GameObject.Find("Directional light").GetComponent<Map>();
        map.planes = planes;
        map.monitorCreator(new Vector3(0,0.05f,0), 8, 8, 5, monitorPrefab);
        for (int i = 0; i < map.planes.Length; i++)
        {
            planeTextures.Add(map.planes[i].GetComponent<Renderer>().material.mainTexture);
        }
    }
	
	// Update is called once per frame
	void Update () {

        switch (flow)
        {
            //get refresh the map
            case 0:
                map.Refresh(center);
                flow = 233;
                break;
            //read file
            case 233:
                Debug.Log(lineNumber);
                List<object> temp = Title.getTitle("files", "Trajectory_1", lineNumber);
                Title title = (Title)temp[0];
                Debug.Log(title.name + "," + title.lineNumber);
                lineNumber = (float)temp[1];
                Debug.Log(lineNumber);

                temp = Title.getTitle("files", "Trajectory_1", lineNumber);
                title = (Title)temp[0];
                Debug.Log(title.name + "," + title.lineNumber);
                lineNumber = (float)temp[1];
                Debug.Log(lineNumber);

                temp = Title.getTitle("files", "Trajectory_1", lineNumber);
                title = (Title)temp[0];
                Debug.Log(title.name + "," + title.lineNumber);
                lineNumber = (float)temp[1];
                Debug.Log(lineNumber);

                flow = 1;
                break;
            //refresh the plane's texture
            case 1:
                StartCoroutine(map._Refresh(map.planes[number], map.points[number]));
                number++;
                if (number >= map.planes.Length)
                {
                    number = 0;
                    flow = 2;
                }
                break;
            //figure out if all plane's texture renew done
            case 2:
                planeWaitTime -= Time.deltaTime;

                if (planeWaitTime < 0)
                {
                    //to delay some time
                    planeWaitTime = 1;
                    for (int i = 0; i < 16; i++)
                    {
                        if (map.planes[i].GetComponent<Renderer>().material.mainTexture != planeTextures[i])
                        {
                            flow = 998;
                        }
                        else
                        {
                            flow = 2;
                            StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                            break;
                        }
                    }
                }
                break;
            //when change
            case 3:
                mouseTest.mouseLock = false;
                map.zoom += 2;
                flow = 0;
                break;
            case 4:
                mouseTest.mouseLock = false;
                map.zoom -= 2;
                flow = 0;
                break;
            //renew the cache texture and unlock the mouselock
            case 998:
                for (int i = 0; i < map.planes.Length; i++)
                {
                    planeTextures[i] = map.planes[i].GetComponent<Renderer>().material.mainTexture;
                }
                mouseTest.mouseLock = true;
                flow = 999;
                break;
            case 999:
                break;
            default:
                break;
        }

    }
}
