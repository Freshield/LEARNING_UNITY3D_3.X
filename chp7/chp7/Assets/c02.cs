using UnityEngine;
using System.Collections;

public class c02 : MonoBehaviour {

    int keyFrame = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("press a");
        }
        if (Input.GetKey(KeyCode.A))
        {
            keyFrame++;
            Debug.Log("a press continue " + keyFrame + " frames");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            keyFrame = 0;
            Debug.Log("a up");
        }
	
	}
}
