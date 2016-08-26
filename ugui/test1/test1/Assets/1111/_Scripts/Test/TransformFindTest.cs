using UnityEngine;
using System.Collections;

public class TransformFindTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		print(transform.childCount);
		print (transform.FindChild("B").gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
