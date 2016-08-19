using UnityEngine;
using System.Collections;

public class MouseControllor : MonoBehaviour {
    int MouseFrame = 0;
    Vector3 position;
    float speed = 0.05f;
    Vector3 cameraPosition;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetMouseButtonDown(0))
        {
            position = Input.mousePosition;
            cameraPosition = Camera.main.transform.position;
            //Debug.Log("press left button position " + Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 change = Input.mousePosition - position;
            if (Camera.main.transform.position.z > -4)
            {
                speed = 0.01f;
            }
            else
            {
                speed = 0.05f;
            }
            Camera.main.transform.position = -change * speed + cameraPosition;
            //Debug.Log("hold left button position " + Input.mousePosition + " speed " + speed);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("up left button position " + Input.mousePosition);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //Debug.Log(Camera.main.transform.position.z);
            if (Camera.main.transform.position.z < -0.7)
            {
                cameraPosition = Camera.main.transform.position;
                float distance = 0;
                if (Camera.main.transform.position.z < -4)
                {
                    distance = Input.GetAxis("Mouse ScrollWheel") * 10;
                }
                else
                {
                    distance = Input.GetAxis("Mouse ScrollWheel") * 2;
                }

                Camera.main.transform.position = new Vector3(0, 0, distance) + cameraPosition;
                //Debug.Log("scrollwheel position " + Input.mousePosition);
            }

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //Debug.Log(Camera.main.transform.position.z);
            if (Camera.main.transform.position.z > -20)
            {
                cameraPosition = Camera.main.transform.position;
                float distance = 0;
                if (Camera.main.transform.position.z < -4)
                {
                    distance = Input.GetAxis("Mouse ScrollWheel") * 10;
                }
                else
                {
                    distance = Input.GetAxis("Mouse ScrollWheel") * 2;
                }
                Camera.main.transform.position = new Vector3(0, 0, distance) + cameraPosition;
                //Debug.Log("scrollwheel position " + Input.mousePosition);
            }

        }


    }
}
