using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    // Use this for initialization
    void Start()
    {

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

        Debug.Log("first,last " + WfirstPosition.worldPosition + WfirstPosition.time + "," + WlastPosition.worldPosition + WlastPosition.time);

    }

    // Update is called once per frame
    void Update()
    {
        switch (button)
        {
            case 0:

                for (int i = 0; i < map.planes.Length; i++)
                {
                    GameObject plane = GameObject.Find("plane" + i);
                    if (plane.GetComponent<Renderer>().material.mainTexture != null)
                    {
                        button = 1;
                    }
                    else
                    {
                        button = 0;
                        StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
                        break;
                    }
                }
                break;

            case 1:
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
                    button = 2;
                }
                break;

            default:
                break;
        }
    }
}
