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
    Title title;
    List<object> titleResult;
    Dictionary<string, Title> dicTitles;
    Dictionary<string, List<string>> dicLevel_9;
    Dictionary<string, List<string>> dicLevel_11;
    Dictionary<string, List<string>> dicLevel_13;

    public int lastLevel_9 = -1;
    public int[] lastLevel_11 = new int[2] { -1, -1 };
    public int[] lastLevel_13 = new int[3] { -1, -1, -1 };
    

    // Use this for initialization
    void Start () {
        dicTitles = new Dictionary<string, Title>();
        dicLevel_9 = new Dictionary<string, List<string>>();
        dicLevel_11 = new Dictionary<string, List<string>>();
        dicLevel_13 = new Dictionary<string, List<string>>();
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
        map.Refresh(center);

    }
	
	// Update is called once per frame
	void Update () {

        switch (flow)
        {
            //read file and prepare dictionaries
            case 0:
                titleResult = Title.getTitle("files", "Trajectory_1", lineNumber);
                lineNumber = (float)titleResult[1];
                if (lineNumber != -1)
                {
                    title = (Title)titleResult[0];
                    title.getLevelPosition(center, map.fullLat, map.fullLon);
                    dicTitles.Add(title.name, title);
                    Debug.Log(title.name);

                    Debug.Log(title.level_9);
                    Debug.Log(title.level_11);
                    Debug.Log(title.level_13);
                    //for level_9
                    if (!dicLevel_9.ContainsKey(title.level_9))
                    {
                        dicLevel_9.Add(title.level_9, new List<string>());
                        dicLevel_9[title.level_9].Add(title.name);
                    }
                    else
                    {
                        dicLevel_9[title.level_9].Add(title.name);
                    }
                    //for level_11
                    if (!dicLevel_11.ContainsKey(title.level_11))
                    {
                        dicLevel_11.Add(title.level_11, new List<string>());
                        dicLevel_11[title.level_11].Add(title.name);
                    }
                    else
                    {
                        dicLevel_11[title.level_11].Add(title.name);
                    }
                    //for level_13
                    if (!dicLevel_13.ContainsKey(title.level_13))
                    {
                        dicLevel_13.Add(title.level_13, new List<string>());
                        dicLevel_13[title.level_13].Add(title.name);
                    }
                    else
                    {
                        dicLevel_13[title.level_13].Add(title.name);
                    }
                }
                else
                {
                    Debug.Log("count"+dicLevel_11.Keys.Count);
                    foreach (string key in dicLevel_11.Keys)
                    {
                        Debug.Log("keys:" + key);
                    }
                    flow = 1;
                    break;
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
                    for (int i = 0; i < 16; i++)
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
                switch (map.zoom)
                {
                    case 9:
                        int temp_9 = 0;
                        for (int i = 0; i < 16; i++)
                        {
                            temp_9 = i;
                            if (dicLevel_9.ContainsKey(temp_9.ToString()))
                            {
                                map.planes[i].transform.FindChild("Area").GetComponent<TextMesh>().text = dicLevel_9[temp_9.ToString()].Count.ToString();
                            }
                            else
                            {
                                map.planes[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "0";
                            }
                            
                        }
                        break;
                    case 11:
                        int[] temp_11 = new int[2];
                        temp_11[0] = lastLevel_9;
                        for (int i = 0; i < 16; i++)
                        {
                            temp_11[1] = i;
                            Debug.Log("plane" + i + ",temp_11:" + temp_11[0] + "," + temp_11[1]);
                            if (dicLevel_11.ContainsKey(temp_11[0]+""+temp_11[1]))
                            {
                                map.planes[i].transform.FindChild("Area").GetComponent<TextMesh>().text = dicLevel_11[temp_11[0] + "" + temp_11[1]].Count.ToString();
                            }
                            else
                            {
                                map.planes[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "0";
                            }
                            
                        }
                        break;
                    case 13:
                        int[] temp_13 = new int[3];
                        temp_13[0] = lastLevel_9;
                        temp_13[1] = lastLevel_11[1];
                        for (int i = 0; i < 16; i++)
                        {
                            temp_13[2] = i;
                            if (dicLevel_13.ContainsKey(temp_13[0] + "" + temp_13[1]+""+temp_13[2]))
                            {
                                map.planes[i].transform.FindChild("Area").GetComponent<TextMesh>().text = dicLevel_13[temp_13[0] + "" + temp_13[1] + "" + temp_13[2]].Count.ToString();
                            }
                            else
                            {
                                map.planes[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "0";
                            }
                            
                        }
                        break;
                    case 15:
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

                map.zoom += 2;
                for (int i = 0; i < map.planes.Length; i++)
                {
                    map.planes[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "";
                }
                
                flow = 233;
                break;
            case 4:
                mouseTest.mouseLock = false;
                map.zoom = 9;
                center = new Position(39.99631f, 116.3291f, new PTime(0));
                for (int i = 0; i < map.planes.Length; i++)
                {
                    map.planes[i].transform.FindChild("Area").GetComponent<TextMesh>().text = "";
                }
                flow = 233;
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
