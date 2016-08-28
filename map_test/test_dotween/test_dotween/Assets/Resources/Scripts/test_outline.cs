using UnityEngine;
using System.Collections;

public class test_outline : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //GameObject obj = GameObject.Find("Sphere");

        //obj.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.01f);

        Debug.Log("here");
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log("here1");

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log(hit.collider.gameObject.name);
            //hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetFloat("Outline width", 0.05f);
        //}
        //else
        //{
        //    Debug.Log("not hit");
        //}

        Debug.Log("here2");

    }
}
