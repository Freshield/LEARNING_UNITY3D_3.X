using UnityEngine;
using System.Collections;

public class test_focus : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(obj.transform);
	}
}
