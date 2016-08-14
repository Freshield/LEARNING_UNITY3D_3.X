using UnityEngine;
using System.Collections;

public class c03 : MonoBehaviour {

    int keyFrame = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKeyDown)
        {
            keyFrame = 0;
            Debug.Log("pressed");
        }
        if (Input.anyKey)
        {
            keyFrame++;
            Debug.Log("press continue " + keyFrame + " frames");
        }

    }
}
