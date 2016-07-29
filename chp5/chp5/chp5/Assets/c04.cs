using UnityEngine;
using System.Collections;

public class c04 : MonoBehaviour {

    GameObject Camer1;
    GameObject Camer2;
    GameObject Camer3;

    // Use this for initialization
    void Start () {

        Camer1 = GameObject.Find("Camera1");
        Camer2 = GameObject.Find("Camera2");
        Camer3 = GameObject.Find("Camera3");


    }
	
	// Update is called once per frame
	void OnGUI () {

        if (GUILayout.Button("camer1",GUILayout.Height(50)))
        {
            Camer1.SetActive(true);
            Camer2.SetActive(false);
            Camer3.SetActive(false);
        }

        if (GUILayout.Button("camer2", GUILayout.Height(50)))
        {
            Camer2.SetActive(true);
            Camer1.SetActive(false);
            Camer3.SetActive(false);
        }

        if (GUILayout.Button("camer3", GUILayout.Height(50)))
        {
            Camer3.SetActive(true);
            Camer1.SetActive(false);
            Camer2.SetActive(false);
        }

    }
}
