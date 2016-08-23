﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Main : MonoBehaviour {
    Map map;
    public GameObject objPrefab;
    List<Track> tracks;
    List<Drawer> drawers;
    Position center;
    Position firstPosition;
    Position lastPosition;
    VecTime WfirstPosition;
    VecTime WlastPosition;
    public float hSliderValue = 0;

    int button = 0;
    int number = 0;

    int frame = 0;

    //for loading
    Texture2D[] anim;
    int nowFram = 0;
    int loadingCount = 0;
    bool isLoading = true;
    GameObject loadingPlane;


    // Use this for initialization
    void Start()
    {
        anim = Resources.LoadAll<Texture2D>("loading");
        loadingCount = anim.Length;
        loadingPlane = GameObject.Find("LoadingPlane");
        drawers = new List<Drawer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (button)
        {
            //load file and prepare
            case 0:
                //get location
                tracks = Track.LoadFile(Application.dataPath, "new_data.txt");

                //get the center, firstposition and lastposition
                Position[] result = Track.calculTracks(tracks);
                center = result[0];
                firstPosition = result[1];
                lastPosition = result[2];
                //clean
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

                button = 1;
                break;
            //filling
            case 1:
                Track fillingTemp = tracks[number];
                if (fillingTemp.positions.Count > 0)
                {
                    //do filling
                    fillingTemp.trackFilling(WfirstPosition, WlastPosition);
                }
                number++;
                if (number == tracks.Count)
                {
                    button = 5;
                    number = 0;
                    
                }
                break;
            //create plane
            case 2:

                for (int i = 0; i < map.planes.Length; i++)
                {
                    GameObject plane = GameObject.Find("plane" + i);
                    if (plane.GetComponent<Renderer>().material.mainTexture != null)
                    {
                        button = 4;
                    }
                    else
                    {
                        button = 2;
                        StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                        break;
                    }
                }
                break;
            //create ballsS
            case 3:
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

            case 5:
                foreach (Track getTrack in tracks)
                {
                    Drawer drawer = new Drawer(objPrefab, getTrack, 10);
                    drawers.Add(drawer);
                }

                //clean loading
                isLoading = false;
                Destroy(loadingPlane);
                button = 4;
                break;

            default:
                break;
        }
    }

    void OnGUI()
    {
        if (button == 4)
        {
            if (GUILayout.Button("play", GUILayout.Height(50)))
            {
                foreach (Drawer drawer in drawers)
                {
                    drawer.tweener.PlayForward();
                }
                
            }

            if (GUILayout.Button("pause", GUILayout.Height(50)))
            {
                foreach (Drawer drawer in drawers)
                {
                    drawer.tweener.Pause();
                }
            }

            foreach (Drawer drawer in drawers)
            {
                if (drawer.tweener.IsPlaying())
                {
                    GUILayout.HorizontalSlider(drawer.tweener.fullPosition, 0, 10, GUILayout.Width(200));
                    hSliderValue = drawer.tweener.fullPosition;
                    drawer.drawLine();
                }
                else
                {
                    hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0, 10, GUILayout.Width(200));

                    drawer.tweener.Goto(hSliderValue, false);

                    drawer.drawLine();
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
