using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    Map map;
    public GameObject objPrefab;
    ArrayList tracks;
    Position center;
    Position firstPosition;
    Position lastPosition;

    int button = 0;
    int number = 0;

    // Use this for initialization
    void Start()
    {

        //get location
        tracks = Track.LoadFile(Application.dataPath, "new_data.txt");

        Position[] result = Track.calculTracks(tracks);
        center = result[0];
        firstPosition = result[1];
        lastPosition = result[2];
        Debug.Log("first,last " + firstPosition.time + "," + lastPosition.time);
        //clean
        result = null;
        
        //get map
        map = GameObject.Find("Directional light").GetComponent<Map>();
        map.Refresh(center);
        for (int i = 0; i < map.planes.Length; i++)
        {
            StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
        }
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
                        break;
                    }
                }
                break;

            case 1:
                Track track = (Track)tracks[number];
                if (track.positions.Count > 0)
                {
                    Locator lb = new Locator(center, map.fullLat, map.fullLon, "o" + number);
                    ArrayList pos = track.positions;
                    lb.locateObject(objPrefab, pos);
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
