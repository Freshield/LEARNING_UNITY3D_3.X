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
    Map map;
    public GameObject objPrefab;
    List<Track> tracks;
    Position center;
    Position firstPosition;
    Position lastPosition;
    VecTime WfirstPosition;
    VecTime WlastPosition;
    GameObject drawTracks;

    //for flow
    int flow = 0;
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
                        flow = 1;
                        break;
                    default:
                        break;
                }
                break;               
            /////////////////get the center, firstposition and lastposition//////////////
            //some maintest work can put here
            case 1:
                tracks = Track.LoadFile("files", "new_data");
                Position[] result = Track.calculTracks(tracks);
                center = result[0];
                firstPosition = result[1];
                lastPosition = result[2];
                //release
                Array.Clear(result, 0, result.Length);
                result = null;
                flow = 2;
                break;
            ////////////////////////////about map//////////////////////////////////////
            case 2:
                switch (mapFlow)
                {
                    //get map
                    case 0:
                        map = GameObject.Find("Directional light").GetComponent<Map>();
                        map.getPlanes(4, 4);
                        map.Refresh(center, 4, 4);
                        mapFlow = 1;
                        break;
                    //get image
                    case 1:
                        StartCoroutine(map._Refresh(map.planes[number], map.points[number]));
                        number++;
                        if (number >= map.planes.Length)
                        {
                            number = 0;
                            flow = 128;
                        }
                        mapFlow = 2;
                        flow = 3;
                        break;
                    default:
                        break;
                }
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
                            drawerFlow = 6;
                            flow = 4;
                        }
                        break;
                    default:
                        break;
                }
                break;
            /////////////////////wait plane finished and done text/////////////////////
            case 4:
                switch (waitFlow)
                {
                    //wait for plane finished
                    case 0:
                        planeWaitTime -= Time.deltaTime;
                        if (planeWaitTime < 0)
                        {
                            //to delay some time
                            planeWaitTime = 1;
                            for (int i = 0; i < map.planes.Length; i++)
                            {
                                GameObject plane = GameObject.Find("plane" + i);
                                if (plane.GetComponent<Renderer>().material.mainTexture != map.planePrefab.GetComponent<Renderer>().sharedMaterial.mainTexture)
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
                    //for done text
                    case 1:
                        loadingTweener = testText.GetComponent<Text>().DOText("DONE", 2, true).SetAutoKill(false).SetEase(Ease.Linear);
                        waitFlow = 2;
                        break;
                    //for enjoy text
                    case 2:
                        if (loadingTweener.IsComplete())
                        {
                            loadingTweener.Kill();
                            loadingTweener = testText.GetComponent<Text>().DOText("THEN ENJOY", 2, true).SetAutoKill(false).SetEase(Ease.Linear);
                            waitFlow = 3;
                        }
                        break;
                    //for loading image disappear
                    case 3:
                        if (loadingTweener.IsComplete())
                        {
                            loadingTweener.Kill();
                            loadingTweener = DOTween.To(x => alphaValue = x, 1, 0, 2).SetAutoKill(false).SetEase(Ease.Linear);
                            waitFlow = 4;
                        }
                        break;
                    //for back image disappear
                    case 4:
                        if (loadingTweener.IsComplete())
                        {
                            loadingTweener.Kill();
                            loadingTweener = DOTween.To(x => alphaValue = x, 1, 0, 2).SetAutoKill(false).SetEase(Ease.Linear);
                            testText.GetComponent<Text>().DOText("", 2);
                            waitFlow = 5;
                        }
                        else
                        {
                            loadingImage.GetComponent<Image>().color = new Color(1, 1, 1, alphaValue);
                        }
                        break;
                    //prepare to go to the true scene
                    case 5:
                        if (loadingTweener.IsComplete())
                        {
                            isLoading = false;
                            loadingTweener.Kill();
                            anim.Clear();
                            anim = null;
                            Destroy(loadingImage);
                            Destroy(backImage);
                            //release map
                            map.clearSelf();
                            map = null;
                            GC.Collect();

                            waitFlow = 6;
                            flow = 5;
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
