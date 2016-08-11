using UnityEngine;
using System.Collections;

public class test_start : MonoBehaviour {

    public GameObject prefab;

	// Use this for initialization
	void Start () {

        PlaneCreator pc = new PlaneCreator(new Vector3(0,0,0) ,2, 2, 10, prefab);

        Position[] positions = Position.PositionCreator(new Position(0, 0, 0), 2, 2, 10, 10);
        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log("position"+ i + " lon,lat = " + positions[i].lontitute + "," + positions[i].latitute);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
