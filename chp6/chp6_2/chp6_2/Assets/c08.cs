using UnityEngine;
using System.Collections;

public class c08 : MonoBehaviour {

    private TrailRenderer trialRender;

	// Use this for initialization
	void Start () {

        trialRender = gameObject.GetComponent<TrailRenderer>();
	
	}
	
	// Update is called once per frame
	void OnGUI () {

        if (GUILayout.Button("add width",GUILayout.Height(50)))
        {
            trialRender.startWidth += 1;
            trialRender.endWidth += 1;
        }

        if (GUILayout.Button("show path", GUILayout.Height(50)))
        {
            trialRender.enabled = true;
        }


        if (GUILayout.Button("hide path", GUILayout.Height(50)))
        {
            trialRender.enabled = false;
        }

    }
}
