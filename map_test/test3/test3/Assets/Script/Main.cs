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
    GameObject drawTracks;

    int flow = 0;
    int number = 0;
    
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
    
    //for companion
    public List<List<Drawer>> companions;
    public bool companionPrepared = false;
    public Dictionary<string, List<int>> index;
    public GameObject linePrefab;


    // Use this for initialization
    void Start()
    {
        //prepare
        anim = Resources.LoadAll<Texture2D>("loading");
        loadingCount = anim.Length;
        loadingPlane = GameObject.Find("LoadingPlane");

        drawers = new List<Drawer>();
        
        companions = new List<List<Drawer>>();

        for (int i = 0; i < 3; i++)
        {
            List<Drawer> companion = new List<Drawer>();
            companions.Add(companion);
        }

        drawTracks = new GameObject("drawTracks");

        index = new Dictionary<string, List<int>>();

    }

    // Update is called once per frame
    void Update()
    {
        
        switch (flow)
        {
            //load file and prepare
            case 0:
                //get location
                tracks = Track.LoadFile(Application.streamingAssetsPath, "new_data.txt");
                flow = 233;
                break;

            case 233:
                //get index
                index = Track.LoadIndex(Application.streamingAssetsPath, "index.txt");
                flow = 1;
                foreach (string drawer in index.Keys)
                {
                    Debug.Log(drawer);
                    foreach (int time in index[drawer])
                    {
                        Debug.Log(time);
                    }
                }
                break;

            case 1:
                //get the center, firstposition and lastposition
                Position[] result = Track.calculTracks(tracks);
                center = result[0];
                firstPosition = result[1];
                lastPosition = result[2];
                //release
                Array.Clear(result,0,result.Length);
                result = null;
                flow = 2;
                break;

            case 2:
                //get map
                map = GameObject.Find("Directional light").GetComponent<Map>();
                map.Refresh(center);
                for (int i = 0; i < map.planes.Length; i++)
                {
                    StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                }
                flow = 3;
                break;

            case 3:
                //generate the world position for each track
                Track.generateWorldPosition(tracks, center, map.fullLat, map.fullLon, objPrefab);
                flow = 4;
                break;

            case 4:
                //transfer first and last position to world position
                WfirstPosition = Track.position2world(firstPosition, center, map.fullLat, map.fullLon, objPrefab);
                WlastPosition = Track.position2world(lastPosition, center, map.fullLat, map.fullLon, objPrefab);

                //create the time bar value
                duration = Drawer.getDuration(WfirstPosition.time.totalTime, WlastPosition.time.totalTime);
                hSliderValue = WfirstPosition.time.totalTime;
                wholeTime = DOTween.To(x => hSliderValue = x, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, duration);
                wholeTime.SetAutoKill(false).SetEase(Ease.Linear).Pause();

                flow = 5;
                break;

            case 5:
                Track getTrack = tracks[number];
                if (getTrack.positions.Count > 0)
                {
                    GameObject obj = Instantiate(objPrefab);
                    obj.name = getTrack.name;
                    obj.transform.FindChild("board").transform.FindChild("ID").GetComponent<TextMesh>().text = obj.name;

                    Drawer drawer = new Drawer(obj, getTrack, Drawer.getDuration(getTrack.WfirstPosition.time.totalTime,getTrack.WlastPosition.time.totalTime));
                    drawers.Add(drawer);
                    drawer.obj.transform.parent = drawTracks.transform;
                    
                    //release
                    drawer = null;
                }
                
                number++;

                //release
                getTrack.clearSelf();
                getTrack = null;

                if (number == tracks.Count)
                {
                    //release
                    tracks.Clear();
                    tracks = null;

                    flow = 82;
                    //back number
                    number = 0;
                    GC.Collect();
                }
                break;

            case 82:
                foreach (Drawer drawer in drawers)
                {
                    if (index.ContainsKey(drawer.obj.name))
                    {
                        //Debug.Log(drawer.obj.name);
                        drawer.getCompanionTimes(index[drawer.obj.name]);
                        drawer.isCompanion = true;
                        
                        //foreach (float time in drawer.companionTimes.Keys)
                        //{
                        //    Debug.Log(time);
                        //}
                    }
                }
                flow = 83;
                break;

            case 83:
                foreach (Drawer drawer in drawers)
                {
                    //Debug.Log(drawer.obj.name + ":" + drawer.getObjectNumber());
                    int lineNumber = drawer.getObjectNumber();
                    for (int i = 0; i < lineNumber; i++)
                    {
                        GameObject lineObj = Instantiate(linePrefab);
                        lineObj.name = "line" + i;
                        lineObj.transform.parent = drawer.obj.transform;
                        drawer.lineObjects.Add(lineObj);
                    }
                }
                flow = 6;
                break;

            //create plane
            case 6:

                for (int i = 0; i < map.planes.Length; i++)
                {
                    GameObject plane = GameObject.Find("plane" + i);
                    if (plane.GetComponent<Renderer>().material.mainTexture != map.planePrefab.GetComponent<Renderer>().sharedMaterial.mainTexture)
                    {
                        flow = 7;
                    }
                    else
                    {
                        flow = 6;
                        //StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                        break;
                    }
                }
                if (flow == 7)
                {
                    //clean loading
                    isLoading = false;
                    Array.Clear(anim, 0, anim.Length);
                    anim = null;
                    Destroy(loadingPlane);
                    //release map
                    map.clearSelf();
                    map = null;
                    GC.Collect();
                }
                break;
            case 7:
                if (isPlaying)
                {
                    wholeTime.PlayForward();
                    foreach (Drawer drawer in drawers)
                    {
                        if (hSliderValue >= drawer.WfirstPosition.time.totalTime)
                        {
                            drawer.tweener.PlayForward();
                            drawer.drawLine(isPlaying);
                        }
                        if (hSliderValue >= drawer.WlastPosition.time.totalTime)
                        {
                            
                        }

                    }
                }
                if (isPause)
                {
                    wholeTime.Pause();
                    foreach (Drawer drawer in drawers)
                    {
                       drawer.tweener.Pause();
                    }
                }
                break;

            default:
                break;
        }
    }

    void OnGUI()
    {
        if (flow == 7)
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
                            
                        }
                        else if (hSliderValue < drawer.WlastPosition.time.totalTime)
                        {
                            drawer.tweener.Goto(Drawer.getDuration(drawer.WfirstPosition.time.totalTime, hSliderValue), false);
                            
                        }
                        else
                        {
                            drawer.tweener.Goto(drawer.duration, false);
                            
                        }
                        drawer.drawLine(isPlaying);
                    }
                }
            }
        }
    }
    
    void FixedUpdate()
    {
        if (isLoading)
        {
            loadingPlane.GetComponent<Renderer>().material.mainTexture = anim[nowFram];
            nowFram++;
            if (nowFram >= loadingCount)
            {
                nowFram = 0;
            }
        }
    }
}
