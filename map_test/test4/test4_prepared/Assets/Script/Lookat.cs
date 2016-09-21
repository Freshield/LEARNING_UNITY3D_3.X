using UnityEngine;
using System.Collections;

public class Lookat : MonoBehaviour {
    
	// Update is called once per frame
	void FixedUpdate () {
        transform.LookAt(Camera.main.gameObject.transform);
        transform.Rotate(new Vector3(90, 0, 0));
    }
}
