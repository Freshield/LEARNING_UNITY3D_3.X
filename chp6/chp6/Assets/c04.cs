using UnityEngine;
using System.Collections;

public class c04 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Ray ray = new Ray(Vector3.zero, transform.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);
        Debug.DrawLine(ray.origin, hit.point);
	
	}
}
