using UnityEngine;
using System.Collections.Generic;

public class MainTest : MonoBehaviour {

    //for plane
    List<Texture> planeTextures;
    public GameObject[] planes;
    public Position center;
    public Map map;
    float planeWaitTime = 1;
    public GameObject monitorPrefab;
    public GameObject[] monitors;

    //for flow
    int number = 0;
    public int flow = 0;
    int fileFlow = 0;
    int mapFlow = 0;
    int titleFlow = 0;
    int waitFlow = 0;
    public bool mainLock = false;
    MouseTest mouseTest;

    //for read title
    List<object> titleResult;
    List<Title> titles;
    float lineNumber = 0;
    public int levelNow = 0;
    public int basicZoom = 0;

    Dictionary<string, List<Title>> dicLevel_0;
    Dictionary<string, List<Title>> dicLevel_1;
    Dictionary<string, List<Title>> dicLevel_2;

    public int lastLevel_0 = -1;
    public int lastLevel_1 = -1;
    public int lastLevel_2 = -1;


    //for center
    public float avgLat = 0;
    public float avgLon = 0;

    //for create drawer
    List<Track> tracks;


    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        switch (flow)
        {
            /////////////////////////about file loading///////////////////////////////
            case 0:
                switch (fileFlow)
                {
                    //for init sites
                    case 0:
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
                        //for create drawer
                        tracks = new List<Track>();
                        map = GameObject.Find("Directional light").GetComponent<Map>();
                        fileFlow = 1;
                        break;
                    //create monitor
                    case 1:
                        monitors = map.monitorCreator(new Vector3(0, 0.05f, 0), 8, 8, 5, monitorPrefab);
                        fileFlow = 2;
                        break;
                    //create planes
                    case 2:
                        map.planes = planes;
                        for (int i = 0; i < map.planes.Length; i++)
                        {
                            planeTextures.Add(map.planes[i].GetComponent<Renderer>().material.mainTexture);
                        }
                        basicZoom = map.zoom;
                        fileFlow = 0;
                        flow = 1;
                        break;
                    default:
                        break;
                }
                break;
            ////////////////////////////////////about title///////////////////////////
            case 1:
                switch (titleFlow)
                {
                    //load title file
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
                            titleFlow = 1;
                            break;
                        }
                        break;
                    //calculate the center
                    case 1:
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

                            titleFlow = 2;
                        }
                        break;
                    //create center and refresh map
                    case 2:
                        center = new Position(avgLat, avgLon, new PTime(0));
                        map.Refresh(center,4,4);
                        titleFlow = 3;
                        break;
                    //calculate the titles area
                    case 3:
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
                            flow = 2;
                            titleFlow = 0;
                        }
                        break;
                    default:
                        break;
                }
                break;
            ////////////////////////////////////about map//////////////////////////
            case 2:
                switch (mapFlow)
                {
                    //refresh map
                    case 0:
                        map.Refresh(center,4,4);
                        mapFlow = 1;
                        break;
                    //get planes
                    case 1:
                        StartCoroutine(map._Refresh(map.planes[number], map.points[number]));
                        number++;
                        if (number >= map.planes.Length)
                        {
                            number = 0;
                            mapFlow = 0;
                            flow = 3;
                        }
                        break;
                    default:
                        break;
                }
                break;
            //////////////////////////////////wait and show monitor///////////////////
            case 3:
                switch (waitFlow)
                {
                    //wait plane texture done
                    case 0:
                        planeWaitTime -= Time.deltaTime;

                        if (planeWaitTime < 0)
                        {
                            //to delay some time
                            planeWaitTime = 1;
                            for (int i = 0; i < map.planes.Length; i++)
                            {
                                if (map.planes[i].GetComponent<Renderer>().material.mainTexture != planeTextures[i])
                                {
                                    waitFlow = 1;
                                }
                                else
                                {
                                    waitFlow = 0;
                                    StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                                    break;
                                }
                            }
                        }
                        break;
                    //show the monitor area numbers
                    case 1:
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
                                    if (dicLevel_1.ContainsKey(lastLevel_0 + "" + temp_1))
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
                        waitFlow = 2;
                        break;
                    //renew the cache texture and unlock the mouselock
                    case 2:
                        for (int i = 0; i < map.planes.Length; i++)
                        {
                            planeTextures[i] = map.planes[i].GetComponent<Renderer>().material.mainTexture;
                        }
                        mouseTest.mouseLock = true;
                        waitFlow = 0;
                        if (levelNow != 2)
                        {
                            //just wait
                            flow = 999;
                        }
                        else
                        {
                            //show tracks
                            flow = 400;
                        }
                        break;
                    default:
                        break;
                }
                break;
            //////////////////////////show tracks//////////////////////////////////
            case 400:
                foreach (Title title in dicLevel_1[lastLevel_0 + "" + lastLevel_1])
                {
                    tracks.Add(Track.LoadTargetFileLine("files", "Trajectory_1", title.lineNumber));
                    Debug.Log(title.name);
                }
                flow = 999;
                break;
            ////////////////////////just wait////////////////////////////////
            case 999:
                break;
            default:
                break;
        }

    }
}
