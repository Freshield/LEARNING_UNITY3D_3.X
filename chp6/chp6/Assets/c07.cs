using UnityEngine;
using System.Collections;

public class c07 : MonoBehaviour {

    GameObject connectedObj = null;

    Component jointComponent = null;

	// Use this for initialization
	void Start () {

        connectedObj = GameObject.Find("Cube1");
	
	}
	
	// Update is called once per frame
	void OnGUI () {

        if (GUILayout.Button("add chain joint"))
        {
            ResetJoint();
            jointComponent = gameObject.AddComponent<HingeJoint>();
            HingeJoint hjoint = (HingeJoint)jointComponent;
            connectedObj.GetComponent<Rigidbody>().useGravity = true;
            hjoint.connectedBody = connectedObj.GetComponent<Rigidbody>();
        }

        if (GUILayout.Button("add fix joint"))
        {
            ResetJoint();
            jointComponent = gameObject.AddComponent<FixedJoint>();
            FixedJoint hjoint = (FixedJoint)jointComponent;
            connectedObj.GetComponent<Rigidbody>().useGravity = true;
            hjoint.connectedBody = connectedObj.GetComponent<Rigidbody>();
        }

        if (GUILayout.Button("add spring joint"))
        {
            ResetJoint();
            jointComponent = gameObject.AddComponent<SpringJoint>();
            SpringJoint hjoint = (SpringJoint)jointComponent;
            connectedObj.GetComponent<Rigidbody>().useGravity = true;
            hjoint.connectedBody = connectedObj.GetComponent<Rigidbody>();
        }

        if (GUILayout.Button("add character joint"))
        {
            ResetJoint();
            jointComponent = gameObject.AddComponent<CharacterJoint>();
            CharacterJoint hjoint = (CharacterJoint)jointComponent;
            connectedObj.GetComponent<Rigidbody>().useGravity = true;
            hjoint.connectedBody = connectedObj.GetComponent<Rigidbody>();
        }

        if (GUILayout.Button("add configurable joint"))
        {
            ResetJoint();
            jointComponent = gameObject.AddComponent<ConfigurableJoint>();
            ConfigurableJoint hjoint = (ConfigurableJoint)jointComponent;
            connectedObj.GetComponent<Rigidbody>().useGravity = true;
            hjoint.connectedBody = connectedObj.GetComponent<Rigidbody>();
        }


    }

    void ResetJoint()
    {
        Destroy(jointComponent);
        this.transform.position = new Vector3(6.0f, 3.0f, 16.0f);
        connectedObj.gameObject.transform.position = new Vector3(3.0f, 4.0f, 15.0f);
        connectedObj.GetComponent<Rigidbody>().useGravity = false;
    }
}
