using UnityEngine;
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
    List<Drawer> drawers;
    public float hSliderValue = 0;
    bool isPlaying = false;
    Tweener wholeTime;


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

                //create the time bar value
                float duration = Drawer.getDuration(WfirstPosition.time.totalTime, WlastPosition.time.totalTime);
                wholeTime = DOTween.To(x => hSliderValue = x, WfirstPosition.time.totalTime, WlastPosition.time.totalTime, duration);

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
                break;

            case 2:
                Track getTrack = tracks[number];
                if (getTrack.positions.Count > 0)
                {
                    Drawer drawer = new Drawer(objPrefab, getTrack, 10);
                    drawers.Add(drawer);
                }
                number++;
                if (number == tracks.Count)
                {
                    
                    button = 3;
                    number = 0;
                }
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
                    Destroy(loadingPlane);
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
            if (GUILayout.Button("play", GUILayout.Height(50)))
            {
                foreach (Drawer drawer in drawers)
                {
                    drawer.tweener.PlayForward();
                }
                isPlaying = true;
            }

            if (GUILayout.Button("pause", GUILayout.Height(50)))
            {
                foreach (Drawer drawer in drawers)
                {
                    drawer.tweener.Pause();
                }
                isPlaying = false;
            }

            if (isPlaying)
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
