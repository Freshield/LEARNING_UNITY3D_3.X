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

    //for read title
    List<object> titleResult;
    List<Title> titles;

    public int levelNow = 0;
    int basicZoom = 0;

    Dictionary<string, List<Title>> dicLevel_0;
    Dictionary<string, List<Title>> dicLevel_1;
    Dictionary<string, List<Title>> dicLevel_2;

    public int lastLevel_0 = -1;
    public int lastLevel_1 = -1;
    public int lastLevel_2 = -1;

    GameObject[] monitors;

    //for center
    float avgLat = 0;
    float avgLon = 0;

    //for create drawer
    List<Track> tracks;


    // Use this for initialization
    void Start () {
        titles = new List<Title>();
        dicLevel_0 = new Dictionary<string, List<Title>>();
        dicLevel_1 = new Dictionary<string, List<Title>>();
        dicLevel_2 = new Dictionary<string, List<Title>>();
        mouseTest = GameObject.Find("Main Camera").GetComponent<MouseTest>();
        planes = new GameObject[16];
        planeTextures = new List<Texture>();
        for (int i = 0; i < 16; i++)
        {
            planes[i] = GameObject.Find("Plane" + i);
        }
        //create the map
        map = GameObject.Find("Directional light").GetComponent<Map>();
        map.planes = planes;
        monitors = map.monitorCreator(new Vector3(0,0.05f,0), 8, 8, 5, monitorPrefab);
        for (int i = 0; i < map.planes.Length; i++)
        {
            planeTextures.Add(map.planes[i].GetComponent<Renderer>().material.mainTexture);
        }
        basicZoom = map.zoom;
        //for create drawer
        tracks = new List<Track>();
    }
	
	// Update is called once per frame
	void Update () {

        switch (flow)
        {
            //read file and prepare title
            case 0:
                titleResult = Title.getTitle("files", "Trajectory_1", lineNumber);
                lineNumber = (float)titleResult[1];
                if (lineNumber != -1)
                {
                    if (((Title)titleResult[0]).latitute < 41.0041f && ((Title)titleResult[0]).latitute > 39.3573f && ((Title)titleResult[0]).lontitute < 117.6202f && ((Title)titleResult[0]).lontitute > 115.0552f)
                    {
                        titles.Add((Title)titleResult[0]);
                    }
                }
                else
                {
                    flow = 100;
                    break;
                }
                break;
            //calculate the center
            case 100:
                if (number < titles.Count)
                {
                    avgLat += titles[number].latitute;
                    avgLon += titles[number].lontitute;
                    number++;
                }
                else
                {
                    number = 0;
                    avgLat /= titles.Count;
                    avgLon /= titles.Count;
                    flow = 101;
                }
                break;
            //generate map
            case 101:
                center = new Position(avgLat, avgLon, new PTime(0));
                map.Refresh(center);
                flow = 102;
                break;
            //calculate the area
            case 102:
                if (number < titles.Count)
                {
                    titles[number].getMonitorLevelPosition(center, map.fullLat, map.fullLon);
                    //for level_0
                    if (!dicLevel_0.ContainsKey(titles[number].level_0))
                    {
                        dicLevel_0.Add(titles[number].level_0, new List<Title>());
                        dicLevel_0[titles[number].level_0].Add(titles[number]);
                    }
                    else
                    {
                        dicLevel_0[titles[number].level_0].Add(titles[number]);
                    }
                    //for level_1
                    if (!dicLevel_1.ContainsKey(titles[number].level_1))
                    {
                        dicLevel_1.Add(titles[number].level_1, new List<Title>());
                        dicLevel_1[titles[number].level_1].Add(titles[number]);
                    }
                    else
                    {
                        dicLevel_1[titles[number].level_1].Add(titles[number]);
                    }
                    //for level_2
                    if (!dicLevel_2.ContainsKey(titles[number].level_2))
                    {
                        dicLevel_2.Add(titles[number].level_2, new List<Title>());
                        dicLevel_2[titles[number].level_2].Add(titles[number]);
                    }
                    else
                    {
                        dicLevel_2[titles[number].level_2].Add(titles[number]);
                    }
                    number++;
                }
                else
                {
                    number = 0;
                    flow = 1;
                }
                break;
            //get refresh the map
            case 233:
                map.Refresh(center);
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
                    for (int i = 0; i < map.planes.Length; i++)
                    {
                        if (map.planes[i].GetComponent<Renderer>().material.mainTexture != planeTextures[i])
                        {
                            flow = 666;
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
            //show the area numbers
            case 666:
                switch (levelNow)
                {
                    case 0:
                        int temp_0 = 0;
                        for (int i = 0; i < monitors.Length; i++)
                        {
                            temp_0 = i;
                            if (dicLevel_0.ContainsKey(temp_0.ToString()))
                            {
                                monitors[i].transform.FindChild("Area").GetComponent<TextMesh>().text = dicLevel_0[temp_0.ToString()].Count.ToString();
                            }
                            else
                            {
                                monitors[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "0";
                            }
                            
                        }
                        break;
                    case 1:
                        int temp_1 = 0;
                        for (int i = 0; i < monitors.Length; i++)
                        {
                            temp_1 = i;
                            if (dicLevel_1.ContainsKey(lastLevel_0+""+ temp_1))
                            {
                                monitors[i].transform.FindChild("Area").GetComponent<TextMesh>().text = dicLevel_1[lastLevel_0 + "" + temp_1].Count.ToString();
                            }
                            else
                            {
                                monitors[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "0";
                            }
                            
                        }
                        break;
                    //in levelnow = 2
                    case 2:
                        break;
                    default:
                        Debug.Log("map's zoom level error");
                        break;
                }
                
                flow = 998;
                break;
            //when change
            case 3:
                mouseTest.mouseLock = false;

                map.zoom += 3;
                levelNow++;
                for (int i = 0; i < monitors.Length; i++)
                {
                    monitors[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "";
                }
                
                flow = 233;
                break;
            case 4:
                mouseTest.mouseLock = false;
                map.zoom = basicZoom;
                levelNow = 0;
                center = new Position(avgLat, avgLon, new PTime(0));
                for (int i = 0; i < monitors.Length; i++)
                {
                    monitors[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "";
                }
                flow = 233;
                break;
            //create tracks
            case 400:
                foreach (Title title in dicLevel_1[lastLevel_0+""+lastLevel_1])
                {
                    tracks.Add(Track.LoadTargetFileLine("files", "Trajectory_1", title.lineNumber));
                }
                flow = 999;
                break;
            //renew the cache texture and unlock the mouselock
            case 998:
                for (int i = 0; i < map.planes.Length; i++)
                {
                    planeTextures[i] = map.planes[i].GetComponent<Renderer>().material.mainTexture;
                }
                mouseTest.mouseLock = true;
                if (levelNow != 2)
                {
                    flow = 999;
                }
                else
                {
                    flow = 400;
                }
                
                break;
            //wait position
            case 999:
                break;
            default:
                break;
        }

    }
}
