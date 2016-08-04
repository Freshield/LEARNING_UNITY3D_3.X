using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    getMap map;
    public GameObject prefab;
    ArrayList tracks;
    Position center;

    int button = 0;
    int number = 0;

    // Use this for initialization
    void Start () {

        //get location
        track_test file = new track_test();
        tracks = file.LoadFile(Application.dataPath, "TrajectoryStream.txt");

        center = file.calculTracks(tracks);

        Debug.Log("latitute,longtitute: " + center.latitute + "," + center.lontitute);



        //get map
        //Position center = new Position(36.52408f,117.2098f,0);

        map = GameObject.Find("Directional light").GetComponent<getMap>();
        map.centerPoint = center;
        map.Refresh();
        for (int i = 0; i < 25; i++)
        {
            StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
        }
        

        //put ball
        locateBall lb = new locateBall(center, map.fullLat, map.fullLon,"parent");
        //Debug.Log("fullat,fullon" + lb.fullLat + " " + lb.fullLon);
        ArrayList locations = new ArrayList(map.points);
        ArrayList balls = lb.addBall(prefab, locations.Count);
        lb.putBall(balls, locations);


    }
	
	// Update is called once per frame
	void Update () {

        switch (button)
        {
            case 0:
                
                for (int i = 0; i < 9; i++)
                {

                    GameObject plane = GameObject.Find("Plane" + 0);
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
                    locateBall lb = new locateBall(center, map.fullLat, map.fullLon, "o" + number);
                    ArrayList pos = track.positions;
                    ArrayList balls = lb.addBall(prefab, pos.Count);
                    lb.putBall(balls, pos);
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
        /*
        Track o1 = (Track)tracks[0];
        locateBall lb1 = new locateBall(center, map.fullLat, map.fullLon, "o1");
        ArrayList o1Pos = o1.positions;
        ArrayList balls = lb1.addBall(prefab, o1Pos.Count);
        lb1.putBall(balls, o1Pos);
        */


        /*for (int i = 0; i < 9; i++)
        {
            if (map.planes[i].GetComponent<Renderer>().material.mainTexture == null)
            {
                Debug.Log(i);
                StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
            }
            
        }*/

    }
}
