using UnityEngine;
using System.Collections;

public class test_mouse : MonoBehaviour {

    int MouseFrame = 0;
    Vector3 position;
    float speed = 0.05f;
    Vector3 cameraPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



        if (Input.GetMouseButtonDown(0))
        {
            position = Input.mousePosition;
            cameraPosition = Camera.main.transform.position;
            Debug.Log("press left button position " + Input.mousePosition);
        }
        
        if (Input.GetMouseButton(0))
        {
            Vector3 change = Input.mousePosition - position;
            Camera.main.transform.position = -change * speed  + cameraPosition ;
            Debug.Log("hold left button position " + Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("up left button position " + Input.mousePosition);
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log(Camera.main.transform.position.z);
            if (Camera.main.transform.position.z < -0.7)
            {
                cameraPosition = Camera.main.transform.position;
                float distance = Input.GetAxis("Mouse ScrollWheel") * 2;
                Camera.main.transform.position = new Vector3(0, 0, distance) + cameraPosition;
                Debug.Log("scrollwheel position " + Input.mousePosition);
            }
            
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Debug.Log(Camera.main.transform.position.z);
            if (Camera.main.transform.position.z > -20)
            {
                cameraPosition = Camera.main.transform.position;
                float distance = Input.GetAxis("Mouse ScrollWheel") * 5;
                Camera.main.transform.position = new Vector3(0, 0, distance) + cameraPosition;
                Debug.Log("scrollwheel position " + Input.mousePosition);
            }

        }


    }
}
