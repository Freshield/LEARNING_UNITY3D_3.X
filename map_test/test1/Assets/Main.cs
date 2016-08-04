using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    getMap map;

	// Use this for initialization
	void Start () {

        GMPoint center = new GMPoint(45.49506f, -73.57801f);
        map = GameObject.Find("Directional light").GetComponent<getMap>();
        map.centerPoint = center;
        map.Refresh();
        for (int i = 0; i < 9; i++)
        {
            Debug.Log(i);
            StartCoroutine(map._Refresh(map.planes[i], map.points[i]));
        }


    }
	
	// Update is called once per frame
	void Update () {

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
