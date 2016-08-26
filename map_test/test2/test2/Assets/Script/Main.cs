using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Main : MonoBehaviour {
    Map map;
    public GameObject objPrefab;
    List<Track> tracks;
    Position center;
    Position firstPosition;
    Position lastPosition;
    VecTime WfirstPosition;
    VecTime WlastPosition;

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
    public float barValue = 0;


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

                button = 1;
                break;
            //filling
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

            case 2:
                Track getTrack = tracks[number];
                if (getTrack.positions.Count > 0)
                {
                    GameObject obj = Instantiate(objPrefab);
                    Drawer drawer = new Drawer(obj, getTrack, Drawer.getDuration(getTrack.WfirstPosition.time.totalTime,getTrack.WlastPosition.time.totalTime));
                    drawers.Add(drawer);
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
                    //prepare
                    barValue = hSliderValue;
                }
                break;
            case 4:
                /*foreach (Drawer drawer in drawers)
                {
                    if (drawer.track != null)
                    {
                        Debug.Log("ok");
                    }
                    else
                    {
                        Debug.Log("not ok");
                    }
                    
                    if (drawer.track.WfirstPosition.time.totalTime > wholeTime.fullPosition)
                    {
                        drawer.obj.SetActive(true);
                    }
                    
                }*/
                if (isPlaying)
                {
                    foreach (Drawer drawer in drawers)
                    {
                        wholeTime.PlayForward();
                        if (hSliderValue >= drawer.track.WfirstPosition.time.totalTime)
                        {
                            drawer.tweener.PlayForward();
                        }

                    }
                }
                if (isPause)
                {
                    foreach (Drawer drawer in drawers)
                    {
                        wholeTime.Pause();
                        if (hSliderValue >= drawer.track.WfirstPosition.time.totalTime)
                        {
                            drawer.tweener.Pause();
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
            if (GUILayout.Button("TIME NOW: " + (int)barValue / 60 + ":" + (int)barValue % 60,GUILayout.Height(50)))
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

            if (hSliderValue == WlastPosition.time.totalTime)
            {
                isPause = true;
                isPlaying = false;
            }

            foreach (Drawer drawer in drawers)
            {
                    drawer.drawLine();
            }


            
            if (wholeTime.IsPlaying())
            {
                GUILayout.HorizontalSlider(hSliderValue, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, GUILayout.Width(200));
                barValue = hSliderValue;
            }
            else
            {
                barValue = GUILayout.HorizontalSlider(barValue, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, GUILayout.Width(200));

                wholeTime.Goto((barValue - WfirstPosition.time.totalTime) / 30.0f, false);

                foreach (Drawer drawer in drawers)
                {
                    if (barValue < drawer.track.WfirstPosition.time.totalTime)
                    {
                        drawer.tweener.Goto(0, false);
                    }
                    else if (barValue < drawer.track.WlastPosition.time.totalTime)
                    {
                        drawer.tweener.Goto((barValue - drawer.track.WfirstPosition.time.totalTime) / 30.0f, false);
                    }
                    else
                    {
                        drawer.tweener.Goto(drawer.duration,false);
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
            if (nowFram == loadingCount)
            {
                nowFram = 0;
            }
        }
    }
}
