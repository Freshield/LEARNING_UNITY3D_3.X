using UnityEngine;
using System.Collections;

public class c14 : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {

        Debug.Log("begin wait " + Time.time);
        yield return new WaitForSeconds(2);
        Debug.Log("end wait " + Time.time);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
