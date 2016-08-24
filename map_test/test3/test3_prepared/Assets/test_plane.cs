using UnityEngine;
using System.Collections;

public class test_plane : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.gameObject.transform);
        transform.Rotate(new Vector3(90, 0, 0));
    }
}
