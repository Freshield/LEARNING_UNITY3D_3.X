using UnityEngine;
using System.Collections;

public class c06 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("press left on " + Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("press right on " + Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(2))
        {
            Debug.Log("press middle on " + Input.mousePosition);
        }


    }
}
