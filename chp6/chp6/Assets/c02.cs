using UnityEngine;
using System.Collections;

public class c02 : MonoBehaviour {

    string show = null;

	// Use this for initialization
	void Start () {

        show = "no collsion";
	
	}

    void OnCollisionEnter(Collision collision)
    {
        show = "begin collisioning, name is " + collision.gameObject.name;
    }

    void OnCollisionStay(Collision collision)
    {
        show = "collingsionING, name is " + collision.gameObject.name;
    }

    void OnCollisionExit(Collision collision)
    {
        show = "after collision, name is " + collision.gameObject.name;
        collision.gameObject.GetComponent<Rigidbody>().Sleep();
    }
	
	// Update is called once per frame
	void OnGUI () {

        GUI.Label(new Rect(100, 0, 300, 40), show);
	
	}
}
