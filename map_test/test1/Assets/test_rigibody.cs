using UnityEngine;
using System.Collections;

public class test_rigibody : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUILayout.Button("fall off",GUILayout.Height(50)))
        {
            for (int i = 0; i < 100; i++)
            {
                GameObject track = GameObject.Find("o" + i);
                foreach (Transform child in track.transform)
                {
                    child.gameObject.AddComponent<Rigidbody>();
                }
            }
        }

    }
}
