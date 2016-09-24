using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class Main : MonoBehaviour
{
    //for map and track
    public Map map;
    public GameObject objPrefab;
    List<Track> tracks;
    public Position center;
    Position firstPosition;
    Position lastPosition;
    VecTime WfirstPosition;
    VecTime WlastPosition;
    GameObject drawTracks;

    //for flow
    public int flow = 0;
    int fileFlow = 0;
    int mapFlow = 0;
    int drawerFlow = 0;
    int waitFlow = 0;
    int number = 0;

    //for loading
    List<Sprite> anim;
    int nowFram = 0;
    bool isLoading = true;
    GameObject loadingImage;
    GameObject backImage;
    Tweener loadingTweener;
    float alphaValue = 255;

    //for dotween
    public List<Drawer> drawers;
    public float hSliderValue = 0;
    public float duration = 0;
    bool isPlaying = false;
    Tweener wholeTime;
    List<GameObject> objs;

    //for companion
    public bool companionPrepared = false;
    //to the drawer which was companion
    public Dictionary<string, List<int>> index;
    public GameObject linePrefab;
    public Texture2D normalTexture;
    public Texture2D companionTexture;
    public Material normalMaterial;
    public Material companionMaterial;
    public Material focusNormalMaterial;
    public Material focusCompanionMaterial;
    //to the line between the companion pair
    public Dictionary<int, List<List<string>>> companionLinesIndex;
    public GameObject companionLines;
    public GameObject companionLinePrefab;

    //for wait time
    float planeWaitTime = 1;
    float loadingWaitTime = 0.02f;

    //for test web player
    public GameObject testText;

    //for board
    public Texture boardNormalTexture;
    public Texture boardCompanionTexture;

    ///////////////////////////////////for maintest/////////////////////////////

    bool firstTime = true;
    MouseControllor mouseControllor;
    GameObject monitor_parent;

    //for plane
    List<Texture> planeTextures;
    public GameObject monitorPrefab;
    public GameObject[] monitors;

    //for flow
    public int Mflow = 0;
    int titleFlow = 0;
    public bool mainLock = false;

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
    




    // Use this for initialization
    void Start()
    {
        //for companion
        Drawer.normalMaterial = normalMaterial;
        Drawer.companionMaterial = companionMaterial;
        Drawer.focusNormalMaterial = focusNormalMaterial;
        Drawer.focusCompanionMaterial = focusCompanionMaterial;

        //for loading
        loadingImage = GameObject.Find("LoadingImage");
        anim = new List<Sprite>();
        for (int i = 1; i < 131; i++)
        {
            Sprite temp;
            if (i < 10)
            {
                temp = Resources.Load<Sprite>("loading/loading00" + i);
            }
            else if (i < 100)
            {
                temp = Resources.Load<Sprite>("loading/loading0" + i);
            }
            else
            {
                temp = Resources.Load<Sprite>("loading/loading" + i);
            }
            anim.Add(temp);
        }

    }

    // Update is called once per frame
    void Update()
    {

        switch (flow)
        {

            ////////////////////////about load file and prepare///////////////////////
            case 0:
                switch (fileFlow)
                {
                    //simple init site
                    case 0:
                        testText = GameObject.Find("TestText");
                        backImage = GameObject.Find("BackImage");

                        companionLines = GameObject.Find("CompanionLines");

                        drawers = new List<Drawer>();

                        drawTracks = new GameObject("drawTracks");

                        index = new Dictionary<string, List<int>>();

                        tracks = new List<Track>();
                        fileFlow = 2;
                        break;
                    //get index
                    case 2:
                        index = Track.LoadIndex("files", "fixed_index");
                        fileFlow = 3;
                        break;
                    //get companion line index
                    case 3:
                        companionLinesIndex = Track.LoadIndexForCompanionLine("files", "fixed_index");
                        fileFlow = 4;
                        break;
                    //create the companion lines object
                    case 4:
                        int biggest = Map.getTheObjNumber(companionLinesIndex);
                        Map.getCompanionLineObj(companionLines, companionLinePrefab, biggest);
                        fileFlow = 5;
                        break;
                    /////////////////////////for maintest///////////////////////
                    //for init sites
                    case 5:
                        titles = new List<Title>();
                        dicLevel_0 = new Dictionary<string, List<Title>>();
                        dicLevel_1 = new Dictionary<string, List<Title>>();
                        dicLevel_2 = new Dictionary<string, List<Title>>();
                        mouseControllor = GameObject.Find("Main Camera").GetComponent<MouseControllor>();
                        planeTextures = new List<Texture>();
                        fileFlow = 6;
                        break;
                    //create planes
                    case 6:
                        map = GameObject.Find("Directional light").GetComponent<Map>();
                        map.getPlanes(4, 4);
                        for (int i = 0; i < map.planes.Length; i++)
                        {
                            planeTextures.Add(map.planes[i].GetComponent<Renderer>().material.mainTexture);
                        }
                        basicZoom = map.zoom;
                        fileFlow = 7;
                        break;
                    //create monitor
                    case 7:
                        monitors = map.monitorCreator(new Vector3(0, 0.05f, 0), 8, 8, 5, monitorPrefab);
                        monitor_parent = GameObject.Find("monitor_parent");
                        fileFlow = 8;
                        break;
                    //create title
                    case 8:
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
                                map.Refresh(center, 4, 4);
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
                                    titleFlow = 0;
                                    fileFlow = 0;
                                    flow = 1;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            ////////////////////////////////////into maintest/////////////////////////
            case 1:
                switch (Mflow)
                {
                    //about map
                    case 0:
                        switch (mapFlow)
                        {
                            //refresh map
                            case 0:
                                map.Refresh(center, 4, 4);
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
                                    Mflow = 1;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    //wait and show monitor
                    case 1:
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
                                mouseControllor.mouseLock = true;
                                waitFlow = 0;
                                if (firstTime)
                                {
                                    waitFlow = 3;
                                }
                                else
                                {
                                    if (levelNow != 2)
                                    {
                                        //just wait
                                        Mflow = 99;
                                    }
                                    else
                                    {
                                        //show tracks
                                        Mflow = 100;
                                    }
                                }
                                break;
                            /////////////////////////////only for the first time/////////////////
                            //for done text
                            case 3:
                                loadingTweener = testText.GetComponent<Text>().DOText("DONE", 2, true).SetAutoKill(false).SetEase(Ease.Linear);
                                waitFlow = 4;
                                break;
                            //for enjoy text
                            case 4:
                                if (loadingTweener.IsComplete())
                                {
                                    loadingTweener.Kill();
                                    loadingTweener = testText.GetComponent<Text>().DOText("THEN ENJOY", 2, true).SetAutoKill(false).SetEase(Ease.Linear);
                                    waitFlow = 5;
                                }
                                break;
                            //for loading image disappear
                            case 5:
                                if (loadingTweener.IsComplete())
                                {
                                    loadingTweener.Kill();
                                    loadingTweener = DOTween.To(x => alphaValue = x, 1, 0, 2).SetAutoKill(false).SetEase(Ease.Linear);
                                    waitFlow = 6;
                                }
                                break;
                            //for back image disappear
                            case 6:
                                if (loadingTweener.IsComplete())
                                {
                                    loadingTweener.Kill();
                                    loadingTweener = DOTween.To(x => alphaValue = x, 1, 0, 2).SetAutoKill(false).SetEase(Ease.Linear);
                                    testText.GetComponent<Text>().DOText("", 2);
                                    waitFlow = 7;
                                }
                                else
                                {
                                    loadingImage.GetComponent<Image>().color = new Color(1, 1, 1, alphaValue);
                                }
                                break;
                            //prepare to go to the true scene
                            case 7:
                                if (loadingTweener.IsComplete())
                                {
                                    //release resources
                                    isLoading = false;
                                    loadingTweener.Kill();
                                    anim.Clear();
                                    anim = null;
                                    Destroy(loadingImage);
                                    Destroy(backImage);
                                    GC.Collect();

                                    firstTime = false;
                                    waitFlow = 0;
                                    Mflow = 99;
                                }
                                else
                                {
                                    backImage.GetComponent<RawImage>().color = new Color(1, 1, 1, alphaValue);
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    //just wait
                    case 99:

                        break;
                    //prepare to create drawer
                    case 100:
                        foreach (Title title in dicLevel_1[lastLevel_0 + "" + lastLevel_1])
                        {
                            tracks.Add(Track.LoadTargetFileLine("files", "Trajectory_1", title.lineNumber));
                            Debug.Log(title.name);
                        }
                        //disable monitor
                        monitor_parent.SetActive(false);
                        Mflow = 0;
                        mouseControllor.hited = null;
                        mouseControllor.mouseFlow = 1;
                        flow = 100;
                        break;
                    default:
                        break;
                }
                break;
            /////////////////get the center, firstposition and lastposition//////////////
            //some maintest work can put here
            case 100:
                Position[] result = Track.calculTracks(tracks);
                firstPosition = result[1];
                lastPosition = result[2];
                //release
                Array.Clear(result, 0, result.Length);
                result = null;
                flow = 3;
                break;
            ////////////////////////about drawer///////////////////////////////////////
            case 3:
                switch (drawerFlow)
                {
                    //generate the world position for each track
                    case 0:
                        Track.generateWorldPosition(tracks, center, map.fullLat, map.fullLon, objPrefab);
                        drawerFlow = 1;
                        break;
                    //create first and last wolrd position
                    case 1:
                        WfirstPosition = Track.position2world(firstPosition, center, map.fullLat, map.fullLon, objPrefab);
                        WlastPosition = Track.position2world(lastPosition, center, map.fullLat, map.fullLon, objPrefab);
                        drawerFlow = 2;
                        break;
                    //create the time bar value and its drawer
                    case 2:
                        duration = Drawer.getDuration(WfirstPosition.time.totalTime, WlastPosition.time.totalTime);
                        hSliderValue = WfirstPosition.time.totalTime;
                        wholeTime = DOTween.To(x => hSliderValue = x, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, duration);
                        wholeTime.SetAutoKill(false).SetEase(Ease.Linear).Pause();
                        drawerFlow = 3;
                        break;
                    //generate the objects and their drawer
                    case 3:
                        Track getTrack = tracks[number];
                        if (getTrack.positions.Count > 0)
                        {
                            GameObject obj = Instantiate(objPrefab);
                            obj.SetActive(false);
                            obj.name = getTrack.name;
                            obj.transform.FindChild("board").transform.FindChild("ID").GetComponent<TextMesh>().text = obj.name;
                            Drawer drawer = new Drawer(obj, getTrack, Drawer.getDuration(getTrack.WfirstPosition.time.totalTime, getTrack.WlastPosition.time.totalTime));
                            drawers.Add(drawer);
                            drawer.obj.transform.parent = drawTracks.transform;
                        }
                        number++;
                        //release
                        getTrack.clearSelf();
                        getTrack = null;
                        if (number >= tracks.Count)
                        {
                            //release
                            tracks.Clear();
                            tracks = null;
                            //back number
                            number = 0;
                            GC.Collect();

                            drawerFlow = 4;
                        }
                        break;
                    //for companions
                    //check their companion situation
                    case 4:
                        if (index.ContainsKey(drawers[number].obj.name))
                        {
                            drawers[number].getCompanionTimes(index[drawers[number].obj.name]);
                            drawers[number].isCompanion = true;
                        }
                        number++;
                        if (number >= drawers.Count)
                        {
                            number = 0;
                            drawerFlow = 5;
                        }
                        break;
                    //create empty child gameobject for objects to create lines later
                    case 5:
                        int lineNumber = drawers[number].getObjectNumber();
                        for (int i = 0; i < lineNumber; i++)
                        {
                            GameObject lineObj = Instantiate(linePrefab);
                            lineObj.name = "line" + i;
                            lineObj.transform.parent = drawers[number].obj.transform;
                            drawers[number].lineObjects.Add(lineObj);
                        }
                        number++;
                        if (number >= drawers.Count)
                        {
                            number = 0;
                            drawerFlow = 0;
                            flow = 5;
                        }
                        break;
                    default:
                        break;
                }
                break;
            //play ground
            case 5:
                if (isPlaying)
                {
                    wholeTime.PlayForward();
                    //for companion lines
                    dealWithCompanionLines((int)hSliderValue / 60);
                    //for drawers
                    dealWithDrawers(false);
                }
                break;
            default:
                break;
        }
    }

    void OnGUI()
    {
        if (flow == 5)
        {
            //for key control
            if (Input.GetKey(KeyCode.RightArrow))
            {
                hSliderValue += 0.2f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                hSliderValue -= 0.2f;
            }
            //show time
            if (GUILayout.Button("TIME NOW: " + (int)hSliderValue / 60 + ":" + (int)hSliderValue % 60, GUILayout.Height(50)))
            {

            }
            //play and pause control
            if (GUILayout.Button("play", GUILayout.Height(50)))
            {
                isPlaying = true;
            }
            if (GUILayout.Button("pause", GUILayout.Height(50)))
            {
                isPlaying = false;

                wholeTime.Pause();
                foreach (Drawer drawer in drawers)
                {
                    drawer.tweener.Pause();
                }
            }

            //for drag bar
            if (isPlaying)
            {
                GUILayout.HorizontalSlider(hSliderValue, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, GUILayout.Width(200));

                if (hSliderValue == WlastPosition.time.totalTime)
                {
                    isPlaying = false;
                }
            }
            else
            {
                hSliderValue = GUILayout.HorizontalSlider(hSliderValue, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, GUILayout.Width(200));
                if (hSliderValue != wholeTime.fullPosition)
                {
                    wholeTime.Goto(Drawer.getDuration(WfirstPosition.time.totalTime, hSliderValue), false);
                    //for companion lines
                    dealWithCompanionLines((int)hSliderValue / 60);
                    //for drawers
                    dealWithDrawers(true);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isLoading)
        {
            loadingWaitTime -= Time.deltaTime;
            if (loadingWaitTime < 0)
            {
                loadingWaitTime = 0.02f;
                loadingImage.GetComponent<Image>().sprite = anim[nowFram];
                nowFram++;
                if (nowFram >= anim.Count)
                {
                    nowFram = 0;
                }
            }
        }
    }

    void dealWithCompanionLines(int timeIndex)
    {
        if (companionLinesIndex.ContainsKey(timeIndex))
        {
            //clear last one
            foreach (Transform child in companionLines.transform)
            {
                child.GetComponent<LineRenderer>().SetVertexCount(0);
            }

            List<GameObject> tempObjs = new List<GameObject>();
            GameObject tempObj;
            int lineObjectCount = 0;

            foreach (List<string> names in companionLinesIndex[timeIndex])
            {
                //to get the companion objects
                foreach (string name in names)
                {
                    tempObj = GameObject.Find(name);
                    if (tempObj != null)
                    {
                        if (tempObj.activeSelf)
                        {
                            tempObjs.Add(tempObj);
                        }
                        else
                        {
                            //cancel the loop
                            tempObjs.Clear();
                            break;
                        }
                    }
                    else
                    {
                        tempObjs.Clear();
                        break;
                    }
                }

                //to create lines between objects
                //x objects need x lines
                if (tempObjs.Count > 1)
                {
                    companionLines.transform.FindChild("companionLine" + lineObjectCount).GetComponent<LineRenderer>().SetVertexCount(tempObjs.Count);

                    Vector3[] positions = new Vector3[tempObjs.Count];

                    for (int i = 0; i < tempObjs.Count; i++)
                    {
                        positions[i] = tempObjs[i].transform.position;
                    }
                    companionLines.transform.FindChild("companionLine" + lineObjectCount).GetComponent<LineRenderer>().SetPositions(positions);

                    lineObjectCount++;

                    //release
                    Array.Clear(positions, 0, positions.Length);
                    tempObjs.Clear();
                }
            }
        }
    }

    void dealWithDrawers(bool isSlider)
    {
        foreach (Drawer drawer in drawers)
        {
            if (hSliderValue < drawer.WfirstPosition.time.totalTime)
            {
                drawer.obj.SetActive(false);
            }
            else if (hSliderValue < drawer.WlastPosition.time.totalTime)
            {
                if (isSlider)
                {
                    drawer.tweener.Goto(Drawer.getDuration(drawer.WfirstPosition.time.totalTime, hSliderValue), false);
                }
                else
                {
                    drawer.tweener.PlayForward();
                }
                drawer.obj.SetActive(true);
                if (drawer.isFocus)
                {
                    drawer.obj.transform.position = drawer.myPosition + Drawer.objFocus;
                    HighlightableObject ho = drawer.obj.GetComponent<HighlightableObject>();
                    if (ho != null)
                    {
                        ho.ConstantOn(Color.red);
                    }
                    else
                    {
                        Debug.Log("ho is null");
                    }
                }
                else
                {
                    drawer.obj.transform.position = drawer.myPosition;
                }
                //?
                if (drawer.isCompanion)
                {
                    foreach (Transform child in drawer.obj.transform)
                    {
                        if (child.name.Contains("line"))
                        {
                            child.GetComponent<LineRenderer>().SetVertexCount(0);
                        }
                    }
                }
                drawer.drawLine(isPlaying);
            }
            else
            {
                drawer.obj.SetActive(false);
            }

            //for companion
            if (drawer.isCompanion)
            {
                if (drawer.moveTimes.ContainsKey(((int)hSliderValue / 60)))
                {
                    drawer.obj.GetComponent<Renderer>().material.mainTexture = companionTexture;
                    drawer.obj.transform.FindChild("board").GetComponent<Renderer>().material.mainTexture = boardCompanionTexture;
                }
                else
                {
                    drawer.obj.GetComponent<Renderer>().material.mainTexture = normalTexture;
                    drawer.obj.transform.FindChild("board").GetComponent<Renderer>().material.mainTexture = boardNormalTexture;
                }
            }

        }

    }
}
