using UnityEngine;
using System.Collections;

public class GUI18 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		GUI.Label(new Rect(1, 50, 100, 30),"C#中文测试");

	}
}
