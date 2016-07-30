using UnityEngine;
using System.Collections;

public class c01 : MonoBehaviour {

    GameObject addFrceObj = null;

    GameObject addPosObj = null;

    GameObject cubeObj = null;

	// Use this for initialization
	void Start () {

        addFrceObj = GameObject.Find("Sphere0");

        addPosObj = GameObject.Find("Sphere1");

        cubeObj = GameObject.Find("Cube");
	
	}
	
	// Update is called once per frame
	void OnGUI () {

        if (GUILayout.Button("normal force",GUILayout.Height(50)))
        {
            addFrceObj.GetComponent<Rigidbody>().AddForce(1000, 0, 1000);
        }

        if (GUILayout.Button("poisition force",GUILayout.Height(50)))
        {
            Vector3 force = cubeObj.transform.position - addPosObj.transform.position;

            force.y += 10;
            force.x += 200;

            addPosObj.GetComponent<Rigidbody>().AddForceAtPosition(force, addPosObj.transform.position, ForceMode.Impulse);
        }
	
	}
}
