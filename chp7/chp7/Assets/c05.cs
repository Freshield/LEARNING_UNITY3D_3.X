using UnityEngine;
using System.Collections;

public class c05 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("press left on " + Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("press right on " + Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("press middle on " + Input.mousePosition);
        }

    }
}
