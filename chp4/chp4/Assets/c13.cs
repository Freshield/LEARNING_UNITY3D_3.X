using UnityEngine;
using System.Collections;

public class c13 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
        
		GUILayout.Label ("game time now " + Time.time);
		GUILayout.Label ("last delta use time " + Time.deltaTime);
		GUILayout.Label ("fixed time " + Time.fixedTime);
		GUILayout.Label ("last delta use fixed time " + Time.fixedDeltaTime);
	
	}
}
