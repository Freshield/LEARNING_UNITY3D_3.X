using UnityEngine;
using System.Collections;

public class c2 : MonoBehaviour {

	GameObject obj;

	// Use this for initialization
	void Start () {

		obj = GameObject.Find ("Cube");

		c1 script = obj.GetComponent<c1> ();

		script.Number = 100;

		Debug.Log (script.Number);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
