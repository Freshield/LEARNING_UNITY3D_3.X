using UnityEngine;
using System.Collections;

public class c01 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("press w");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("press s");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("press a");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("press d");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("press space");
        }

        ///////////////////////////////////////////////////


        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("up w");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("up s");
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("up a");
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("up d");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("up space");
        }

    }
}
