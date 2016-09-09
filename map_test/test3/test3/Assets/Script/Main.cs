﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

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
    public List<List<Drawer>> companions;
    public bool companionPrepared = false;
    public Dictionary<string, List<int>> index;
    public GameObject linePrefab;
    public Texture2D normalTexture;
    public Texture2D companionTexture;
    public Material normalMaterial;
    public Material companionMaterial;
    public Material focusNormalMaterial;
    public Material focusCompanionMaterial;

    //for wait time
    float planeWaitTime = 1;
    float loadingWaitTime = 0.02f;

    //for test web player
    public GameObject testText;



    // Use this for initialization
    void Start()
    {
        testText = GameObject.Find("TestText");
        loadingImage = GameObject.Find("LoadingImage");
        backImage = GameObject.Find("BackImage");
        Drawer.normalMaterial = normalMaterial;
        Drawer.companionMaterial = companionMaterial;
        Drawer.focusNormalMaterial = focusNormalMaterial;
        Drawer.focusCompanionMaterial = focusCompanionMaterial;
        anim = new List<Sprite>();
        //prepare
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
                tracks = Track.LoadFile("files", "new_data");
                flow = 1;
                break;

            case 1:
                
                //get index
                index = Track.LoadIndex("files", "fixed_index");
                flow = 2;
                break;

            case 2:
                
                //get the center, firstposition and lastposition
                Position[] result = Track.calculTracks(tracks);
                center = result[0];
                firstPosition = result[1];
                lastPosition = result[2];
                //release
                Array.Clear(result,0,result.Length);
                result = null;
                flow = 3;
                break;

            case 3:
                //get map
                map = GameObject.Find("Directional light").GetComponent<Map>();
                map.Refresh(center);
                for (int i = 0; i < map.planes.Length; i++)
                {
                    StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                }
                
                flow = 4;
                break;

            case 4:
                //generate the world position for each track
                Track.generateWorldPosition(tracks, center, map.fullLat, map.fullLon, objPrefab);
                
                flow = 5;
                break;

            case 5:
                //transfer first and last position to world position
                WfirstPosition = Track.position2world(firstPosition, center, map.fullLat, map.fullLon, objPrefab);
                WlastPosition = Track.position2world(lastPosition, center, map.fullLat, map.fullLon, objPrefab);

                //create the time bar value
                duration = Drawer.getDuration(WfirstPosition.time.totalTime, WlastPosition.time.totalTime);
                hSliderValue = WfirstPosition.time.totalTime;
                wholeTime = DOTween.To(x => hSliderValue = x, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, duration);
                wholeTime.SetAutoKill(false).SetEase(Ease.Linear).Pause();
                
                flow = 6;
                break;

            case 6:
                Track getTrack = tracks[number];
                if (getTrack.positions.Count > 0)
                {
                    GameObject obj = Instantiate(objPrefab);
                    obj.SetActive(false);
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

                    flow = 7;
                    //back number
                    number = 0;
                    GC.Collect();
                }
                break;

            case 7:
                foreach (Drawer drawer in drawers)
                {
                    if (index.ContainsKey(drawer.obj.name))
                    {
                        drawer.getCompanionTimes(index[drawer.obj.name]);
                        drawer.isCompanion = true;
                    }
                }
                
                flow = 8;
                break;

            case 8:
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
                
                flow = 9;
                break;

            //create plane
            case 9:
                planeWaitTime -= Time.deltaTime;
                
                if (planeWaitTime < 0)
                {
                    planeWaitTime = 1;
                    for (int i = 0; i < map.planes.Length; i++)
                    {
                        GameObject plane = GameObject.Find("plane" + i);
                        if (plane.GetComponent<Renderer>().material.mainTexture != map.planePrefab.GetComponent<Renderer>().sharedMaterial.mainTexture)
                        {
                            flow = 233;
                        }
                        else
                        {
                            flow = 9;
                            StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                            break;
                        }
                    }
                }
                
                break;

            case 233:
                loadingTweener = testText.GetComponent<Text>().DOText("DONE", 2, true).SetAutoKill(false).SetEase(Ease.Linear);
                flow = 82;
                break;

            case 82:
                if (loadingTweener.IsComplete())
                {
                    loadingTweener = testText.GetComponent<Text>().DOText("THEN ENJOY", 2, true).SetAutoKill(false).SetEase(Ease.Linear);
                    flow = 83;
                }
                break;
            case 83:
                if (loadingTweener.IsComplete())
                {
                    loadingTweener = DOTween.To(x => alphaValue = x,1,0,2).SetAutoKill(false).SetEase(Ease.Linear);
                    flow = 85;
                }
                break;
            case 85:
                if (loadingTweener.IsComplete())
                {
                    loadingTweener = DOTween.To(x => alphaValue = x, 1, 0, 2).SetAutoKill(false).SetEase(Ease.Linear);
                    testText.GetComponent<Text>().DOText("", 2);
                    flow = 84;
                }
                else
                {
                    loadingImage.GetComponent<Image>().color = new Color(1, 1, 1, alphaValue);
                }
                break;
            case 84:
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
                    flow = 10;
                }
                else
                {
                    backImage.GetComponent<RawImage>().color = new Color(1, 1, 1, alphaValue);
                }
                break;

            case 10:
                
                if (isPlaying)
                {
                    wholeTime.PlayForward();
                    foreach (Drawer drawer in drawers)
                    {
                        if (hSliderValue < drawer.WfirstPosition.time.totalTime)
                        {
                            drawer.obj.SetActive(false);
                        }
                        else if (hSliderValue < drawer.WlastPosition.time.totalTime)
                        {
                            drawer.obj.SetActive(true);
                            drawer.tweener.PlayForward();
                            if (drawer.isFocus)
                            {
                                drawer.obj.transform.position = drawer.myPosition + Drawer.objFocus;
                            }
                            else
                            {
                                drawer.obj.transform.position = drawer.myPosition;
                            }
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
                            if (drawer.moveTimes.ContainsKey(((int)hSliderValue/60)))
                            {
                                drawer.obj.GetComponent<Renderer>().material.mainTexture = companionTexture;
                            }
                            else
                            {
                                drawer.obj.GetComponent<Renderer>().material.mainTexture = normalTexture;
                            }
                        }

                    }
                }
                break;

            default:
                break;
        }
    }

    void OnGUI()
    {
        if (flow == 10)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                hSliderValue += 0.2f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                hSliderValue -= 0.2f;
            }
            if (GUILayout.Button("TIME NOW: " + (int)hSliderValue / 60 + ":" + (int)hSliderValue % 60,GUILayout.Height(50)))
            {
                
            }

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
                    if (drawer.isFocus)
                    {
                        drawer.obj.transform.position = drawer.myPosition + Drawer.objFocus;
                    }
                    else
                    {
                        drawer.obj.transform.position = drawer.myPosition;
                    }
                }
            }

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
                    
                    foreach (Drawer drawer in drawers)
                    {
                        if (hSliderValue < drawer.WfirstPosition.time.totalTime)
                        {
                            drawer.tweener.Goto(0, false);
                            drawer.obj.SetActive(false);
                            
                        }
                        else if (hSliderValue < drawer.WlastPosition.time.totalTime)
                        {
                            drawer.tweener.Goto(Drawer.getDuration(drawer.WfirstPosition.time.totalTime, hSliderValue), false);
                            drawer.obj.SetActive(true);
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
                            if (drawer.isFocus)
                            {
                                drawer.obj.transform.position = drawer.myPosition + Drawer.objFocus;
                            }
                            else
                            {
                                drawer.obj.transform.position = drawer.myPosition;
                            }
                        }
                        else
                        {
                            drawer.tweener.Goto(drawer.duration, false);
                            drawer.obj.SetActive(false);
                            
                        }
                        //for companion
                        if (drawer.isCompanion)
                        {
                            if (drawer.moveTimes.ContainsKey(((int)hSliderValue / 60)))
                            {
                                drawer.obj.GetComponent<Renderer>().material.mainTexture = companionTexture;
                            }
                            else
                            {
                                drawer.obj.GetComponent<Renderer>().material.mainTexture = normalTexture;
                            }
                            
                        }

                        
                    }
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
    
}
