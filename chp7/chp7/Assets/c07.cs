using UnityEngine;
using System.Collections;

public class c07 : MonoBehaviour {

    int MouseFrame = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            MouseFrame++;
            Debug.Log("press left on " + MouseFrame + " frame");
        }

        if (Input.GetMouseButton(1))
        {
            MouseFrame++;
            Debug.Log("press right on " + MouseFrame + " frame");
        }

        if (Input.GetMouseButton(2))
        {
            MouseFrame++;
            Debug.Log("press middle on " + MouseFrame + " frame");
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseFrame = 0;
        }

        if (Input.GetMouseButtonUp(1))
        {
            MouseFrame = 0;
        }

        if (Input.GetMouseButtonUp(2))
        {
            MouseFrame = 0;
        }

    }
}
