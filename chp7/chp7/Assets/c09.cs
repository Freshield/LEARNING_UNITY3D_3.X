using UnityEngine;
using System.Collections;

public class c09 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float value = Input.GetAxis("test");
        Debug.Log("axis is " + value);
	
	}
}
