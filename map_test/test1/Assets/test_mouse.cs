using UnityEngine;
using System.Collections;

public class test_mouse : MonoBehaviour {

    int MouseFrame = 0;
    Vector3 position;
    float speed = 0.2f;
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

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("press right button position " + Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("press middle button position " + Input.mousePosition);
        }
        //////////////////////////////////////////////////////////
        if (Input.GetMouseButton(0))
        {
            Vector3 change = Input.mousePosition - position;
            Camera.main.transform.position = change * speed  + cameraPosition ;
            Debug.Log("hold left button position " + Input.mousePosition);
        }

        if (Input.GetMouseButton(01))
        {
           
            Debug.Log("hold right button position " + Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            
            Debug.Log("hold middle button position " + Input.mousePosition);
        }
        ///////////////////////////////////////////////////////
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("up left button position " + Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("up right button position " + Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(2))
        {
            Debug.Log("up middle button position " + Input.mousePosition);
        }
        /////////////////////////////////////////////
        /*
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= 100)
                Camera.main.fieldOfView += 2;
            if (Camera.main.orthographicSize <= 20)
                Camera.main.orthographicSize += 0.5F;
        }
        //Zoom in  
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 2)
                Camera.main.fieldOfView -= 2;
            if (Camera.main.orthographicSize >= 1)
                Camera.main.orthographicSize -= 0.5F;
        }
        */

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log(Camera.main.transform.position.z);
            if (Camera.main.transform.position.z < -1)
            {
                cameraPosition = Camera.main.transform.position;
                float distance = Input.GetAxis("Mouse ScrollWheel") * 5;
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
