using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    

	// Use this for initialization
	void Start () {

        GMPoint center = new GMPoint(45.49506f, -73.57801f);
        getMap getMap = GameObject.Find("Directional light").GetComponent<getMap>();
        getMap.centerPoint = center;
        getMap.Refresh();
        for (int i = 0; i < 9; i++)
        {
            Debug.Log(i);
            StartCoroutine(GameObject.Find("Directional light").GetComponent<getMap>()._Refresh(getMap.planes[i], getMap.points[i]));
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
