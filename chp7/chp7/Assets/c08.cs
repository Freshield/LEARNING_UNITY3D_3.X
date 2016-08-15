using UnityEngine;
using System.Collections;

public class c08 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("test"))
        {
            Debug.Log("press left on " + Input.mousePosition);
        }

        if (Input.GetButton("test"))
        {
            Debug.Log("press right on " + Input.mousePosition);
        }

        if (Input.GetButton("test"))
        {
            Debug.Log("press middle on " + Input.mousePosition);
        }
    }
}
