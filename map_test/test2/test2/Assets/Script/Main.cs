using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    Map map;
    public GameObject objPrefab;
    List<Track> tracks;
    Position center;
    Position firstPosition;
    Position lastPosition;
    VecTime WfirstPosition;
    VecTime WlastPosition;
    MouseControllor mouseControllor;
    GameObject drawTracks;

    int button = 0;
    int number = 0;

    int frame = 0;

    //for loading
    Texture2D[] anim;
    int nowFram = 0;
    int loadingCount = 0;
    bool isLoading = true;
    GameObject loadingPlane;

    //for dotween
    public List<Drawer> drawers;
    public float hSliderValue = 0;
    public float duration = 0;
    bool isPlaying = false;
    bool isPause = true;
    Tweener wholeTime;
    List<GameObject> objs;

    //for toggle
    public Toggle Companion1, Companion2, Companion3;

    //for companion
    public List<List<Drawer>> companions;
    public bool companionPrepared = false;


    // Use this for initialization
    void Start()
    {
        //prepare
        anim = Resources.LoadAll<Texture2D>("loading");
        loadingCount = anim.Length;
        loadingPlane = GameObject.Find("LoadingPlane");
        drawers = new List<Drawer>();
        //not auto play
        DOTween.defaultAutoPlay = AutoPlay.None;
        mouseControllor = GameObject.Find("Main Camera").GetComponent<MouseControllor>();

        //for companions
        Companion1.onValueChanged.AddListener(Companion0Changed);
        Companion2.onValueChanged.AddListener(Companion1Changed);
        Companion3.onValueChanged.AddListener(Companion2Changed);

        companions = new List<List<Drawer>>();
        for (int i = 0; i < 3; i++)
        {
            List<Drawer> companion = new List<Drawer>();
            companions.Add(companion);
        }

        drawTracks = new GameObject("drawTracks");
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (button)
        {
            //load file and prepare
            case 0:
                //get location
                tracks = Track.LoadFile(Application.streamingAssetsPath, "new_data.txt");

                //get the center, firstposition and lastposition
                Position[] result = Track.calculTracks(tracks);
                center = result[0];
                firstPosition = result[1];
                lastPosition = result[2];
                //clean
                Array.Clear(result,0,result.Length);
                result = null;

                //get map
                map = GameObject.Find("Directional light").GetComponent<Map>();
                map.Refresh(center);
                for (int i = 0; i < map.planes.Length; i++)
                {
                    StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                }

                //generate the world position for each track
                Track.generateWorldPosition(tracks, center, map.fullLat, map.fullLon, objPrefab);

                //transfer first and last position to world position
                WfirstPosition = Track.position2world(firstPosition, center, map.fullLat, map.fullLon, objPrefab);
                WlastPosition = Track.position2world(lastPosition, center, map.fullLat, map.fullLon, objPrefab);

                //create the time bar value
                duration = Drawer.getDuration(WfirstPosition.time.totalTime, WlastPosition.time.totalTime);
                hSliderValue = WfirstPosition.time.totalTime;
                wholeTime = DOTween.To(x => hSliderValue = x, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, duration);
                wholeTime.SetAutoKill(false).SetEase(Ease.Linear);

                button = 2;
                break;
            //not used part cause filling cost too much resources
            //also we can use only dotween now
            //filling
            /*
            case 1:
                //here like the locate ball, filling each ball one update
                Track fillingTemp = tracks[number];
                if (fillingTemp.positions.Count > 0)
                {
                    //do filling
                    //new version, cause dotween face problem
                    fillingTemp.trackFilling(fillingTemp.WfirstPosition, fillingTemp.WlastPosition);
                }
                number++;
                if (number == tracks.Count)
                {
                    button = 2;
                    number = 0;
                    
                }
                //release
                fillingTemp = null;
                GC.Collect();
                break;
            */

            case 2:
                Track getTrack = tracks[number];
                if (getTrack.positions.Count > 0)
                {
                    //Debug.Log(getTrack.name + ",firstPosition " + getTrack.firstPosition.latitute + "," +
                    //    getTrack.firstPosition.lontitute + "," + getTrack.firstPosition.time + ",lastPosition " +
                    //    getTrack.lastPosition.latitute + "," +
                    //    getTrack.lastPosition.lontitute + "," + getTrack.lastPosition.time);
                    GameObject obj = Instantiate(objPrefab);
                    obj.name = getTrack.name;
                    Drawer drawer = new Drawer(obj, getTrack, Drawer.getDuration(getTrack.WfirstPosition.time.totalTime,getTrack.WlastPosition.time.totalTime),false);
                    drawers.Add(drawer);
                    drawer.obj.transform.parent = drawTracks.transform;

                    //for companions
                    //old version, for 3d show
                    /*
                    if (getTrack.name.Contains("6602") || getTrack.name.Contains("9789") || getTrack.name.Contains("14914"))
                    {
                        GameObject objC = Instantiate(objPrefab);
                        objC.name = getTrack.name;
                        companions[0].Add(new Drawer(objC, getTrack, Drawer.getDuration(getTrack.WfirstPosition.time.totalTime, getTrack.WlastPosition.time.totalTime), true));
                    }
                    else if (getTrack.name.Contains("7459") || getTrack.name.Contains("7585"))
                    {
                        GameObject objC = Instantiate(objPrefab);
                        objC.name = getTrack.name;
                        companions[1].Add(new Drawer(objC, getTrack, Drawer.getDuration(getTrack.WfirstPosition.time.totalTime, getTrack.WlastPosition.time.totalTime), true));
                    }
                    else if (getTrack.name.Contains("13423") || getTrack.name.Contains("13426"))
                    {
                        GameObject objC = Instantiate(objPrefab);
                        objC.name = getTrack.name;
                        companions[2].Add(new Drawer(objC, getTrack, Drawer.getDuration(getTrack.WfirstPosition.time.totalTime, getTrack.WlastPosition.time.totalTime), true));
                    }
                    */
                    //new version
                    if (drawer.obj.name.Contains("6602") || drawer.obj.name.Contains("9789") || drawer.obj.name.Contains("14914"))
                    {
                        drawer.isCompanion = true;
                        companions[0].Add(drawer);
                    }
                    else if (drawer.obj.name.Contains("7459") || drawer.obj.name.Contains("7585"))
                    {
                        drawer.isCompanion = true;
                        companions[1].Add(drawer);
                    }
                    else if (drawer.obj.name.Contains("13423") || drawer.obj.name.Contains("13426"))
                    {
                        drawer.isCompanion = true;
                        companions[2].Add(drawer);
                    }

                    //release
                    drawer = null;
                }
                number++;
                if (number == tracks.Count)
                {
                    //release
                    tracks.Clear();
                    tracks = null;

                    button = 3;
                    number = 0;
                    GC.Collect();
                }
                //release
                getTrack = null;
                break;

            //create plane
            case 3:

                for (int i = 0; i < map.planes.Length; i++)
                {
                    GameObject plane = GameObject.Find("plane" + i);
                    if (plane.GetComponent<Renderer>().material.mainTexture != null)
                    {
                        button = 4;
                    }
                    else
                    {
                        button = 3;
                        StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                        break;
                    }
                }
                if (button == 4)
                {
                    //clean loading
                    isLoading = false;
                    Array.Clear(anim, 0, anim.Length);
                    anim = null;
                    Destroy(loadingPlane);
                    //release map
                    map = null;
                    GC.Collect();
                }
                break;
            case 4:
                if (isPlaying)
                {
                    wholeTime.PlayForward();
                    foreach (Drawer drawer in drawers)
                    {
                        if (hSliderValue >= drawer.WfirstPosition.time.totalTime)
                        {
                            drawer.tweener.PlayForward();
                            drawer.drawLine(isPlaying);
                            if (drawer.isCompanion)
                            {
                                drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 234f/255f, 1, 1));
                                drawer.obj.GetComponent<LineRenderer>().material.SetColor("_OutlineColor", new Color(0, 234f / 255f, 1, 1));
                            }
                        }
                        if (hSliderValue >= drawer.WlastPosition.time.totalTime)
                        {
                            if (drawer.isCompanion)
                            {
                                drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
                            }
                        }

                    }
                    if (companionPrepared)
                    {
                        foreach (Drawer drawer in mouseControllor.targetCompanion[mouseControllor.companionNumber])
                        {
                            if (hSliderValue >= drawer.WfirstPosition.time.totalTime)
                            {
                                drawer.tweener.PlayForward();
                                drawer.drawLine(isPlaying);
                                if (drawer.isCompanion)
                                {
                                    drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 234f / 255f, 1, 1));
                                    drawer.obj.GetComponent<LineRenderer>().material.SetColor("_OutlineColor", new Color(0, 234f / 255f, 1, 1));
                                }
                            }
                        }
                    }
                }
                if (isPause)
                {
                    wholeTime.Pause();
                    foreach (Drawer drawer in drawers)
                    {
                        
                        if (hSliderValue >= drawer.WfirstPosition.time.totalTime && hSliderValue < drawer.WlastPosition.time.totalTime)
                        {
                            drawer.tweener.Pause();
                            /*
                            if (drawer.isCompanion)
                            {
                                drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 234f / 255f, 1, 1));
                                drawer.obj.GetComponent<LineRenderer>().material.SetColor("_OutlineColor", new Color(0, 234f / 255f, 1, 1));
                            }
                            */
                        }

                    }
                    if (companionPrepared)
                    {
                        foreach (Drawer drawer in mouseControllor.targetCompanion[mouseControllor.companionNumber])
                        {
                            if (hSliderValue >= drawer.WfirstPosition.time.totalTime)
                            {
                                drawer.tweener.Pause();
                            }
                        }
                    }
                }
                break;
            //not used part
            //create ballsS
            /*
            case 99:
                Track track = tracks[number];
                if (track.positions.Count > 0)
                {
                    Locator lb = new Locator(center, map.fullLat, map.fullLon, track.name);
                    //List<Position> pos = track.positions;
                    //lb.locateObject(objPrefab, pos);
                    lb.worldLocate(objPrefab, track.worldPositions);
                }
                number++;
                if (number == tracks.Count)
                {
                    button = 4;
                }
                break;
                */

            default:
                break;
        }
    }

    void OnGUI()
    {
        if (button == 4)
        {
            if (GUILayout.Button("TIME NOW: " + (int)hSliderValue / 60 + ":" + (int)hSliderValue % 60,GUILayout.Height(50)))
            {
                
            }

            if (GUILayout.Button("play", GUILayout.Height(50)))
            {
                isPlaying = true;
                isPause = false;
            }

            if (GUILayout.Button("pause", GUILayout.Height(50)))
            {
                isPause = true;
                isPlaying = false;
            }
            
            
            if (isPlaying)
            {
                GUILayout.HorizontalSlider(hSliderValue, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, GUILayout.Width(200));
                
                if (hSliderValue == WlastPosition.time.totalTime)
                {
                    isPause = true;
                    isPlaying = false;
                }
                

            }
            else
            {
                hSliderValue = GUILayout.HorizontalSlider(hSliderValue, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, GUILayout.Width(200));

                if (hSliderValue != wholeTime.fullPosition)
                {
                    wholeTime.Goto(Drawer.getDuration(WfirstPosition.time.totalTime, hSliderValue), false);
                    
                    foreach (Drawer drawer in drawers)
                    {
                        if (hSliderValue < drawer.WfirstPosition.time.totalTime)
                        {
                            drawer.tweener.Goto(0, false);
                            drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
                        }
                        else if (hSliderValue < drawer.WlastPosition.time.totalTime)
                        {
                            drawer.tweener.Goto(Drawer.getDuration(drawer.WfirstPosition.time.totalTime, hSliderValue), false);
                            if (drawer.isCompanion)
                            {
                                drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 234f / 255f, 1, 1));
                                drawer.obj.GetComponent<LineRenderer>().material.SetColor("_OutlineColor", new Color(0, 234f / 255f, 1, 1));
                            }
                        }
                        else
                        {
                            drawer.tweener.Goto(drawer.duration, false);
                            if (drawer.isCompanion)
                            {
                                drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
                            }
                        }
                        drawer.drawLine(isPlaying);
                    }

                    if (companionPrepared)
                    {
                        foreach (Drawer drawer in mouseControllor.targetCompanion[mouseControllor.companionNumber])
                        {
                            if (hSliderValue < drawer.WfirstPosition.time.totalTime)
                            {
                                drawer.tweener.Goto(0, false);
                                if (drawer.isCompanion)
                                {
                                    drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
                                }
                            }
                            else if (hSliderValue < drawer.WlastPosition.time.totalTime)
                            {
                                drawer.tweener.Goto(Drawer.getDuration(drawer.WfirstPosition.time.totalTime, hSliderValue), false);
                                drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0, 234f / 255f, 1, 1));
                            }
                            else
                            {
                                drawer.tweener.Goto(drawer.duration, false);
                                if (drawer.isCompanion)
                                {
                                    drawer.obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
                                }
                            }
                            drawer.drawLine(isPlaying);
                        }
                    }
                }

            }

            if (companionPrepared)
            {
                for (int i = 0; i < mouseControllor.targetCompanion[mouseControllor.companionNumber].Count; i++)
                {
                    mouseControllor.cylinders[i].transform.position = mouseControllor.targetCompanion[mouseControllor.companionNumber][i].obj.transform.position + new Vector3(0, -5, 0);
                }
            }
        }
    }

    public void Companion0Changed(bool check)
    {
        if (check)
        {
            mouseControllor.button = 3;
            mouseControllor.companionNumber = 0;
        }
    }
    public void Companion1Changed(bool check)
    {
        if (check)
        {
            mouseControllor.button = 3;
            mouseControllor.companionNumber = 1;
        }
    }
    public void Companion2Changed(bool check)
    {
        if (check)
        {
            mouseControllor.button = 3;
            mouseControllor.companionNumber = 2;
        }
    }

    void FixedUpdate()
    {
        if (isLoading)
        {
            loadingPlane.GetComponent<Renderer>().material.mainTexture = anim[nowFram];
            nowFram++;
            if (nowFram == loadingCount)
            {
                nowFram = 0;
            }
        }
    }
}
