using UnityEngine;
using UnityEditor;
using System.Collections;

[AddComponentMenu("new script/rotate auto")]
public class c03 : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(0.0f, Time.deltaTime * 200, 0.0f);
	
	}
}
