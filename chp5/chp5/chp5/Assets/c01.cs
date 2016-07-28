using UnityEngine;
using System.Collections;

public class c01 : MonoBehaviour {

    private Camera camera;

	// Use this for initialization
	void Start () {

        camera = gameObject.GetComponent<Camera>();
	
	}
	
	// Update is called once per frame
	void OnGUI () {

        if (GUILayout.Button("first one",GUILayout.Height(50)))
        {
            camera.orthographic = true;
        }

        if (GUILayout.Button("second one", GUILayout.Height(50)))
        {
            camera.orthographic = false;
        }

    }
}
