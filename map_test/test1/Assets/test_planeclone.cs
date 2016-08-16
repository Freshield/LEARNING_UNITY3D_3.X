using UnityEngine;
using System.Collections;

public class test_planeclone : MonoBehaviour {

    int count = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUILayout.Button("clone",GUILayout.Height(50)))
        {
            GameObject temp = GameObject.Find("plane_parent");
            GameObject newPlane = Instantiate(temp);
            newPlane.transform.position += new Vector3(0, 0, 25 * count);
            count++;
        }
    }
}
