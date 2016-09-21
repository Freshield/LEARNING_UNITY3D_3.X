using UnityEngine;
using System.Collections;

public class MouseTest : MonoBehaviour {

    Vector3 position;
    float speed = 0.05f;
    Vector3 cameraPosition;
    public float distance;
    GameObject theCamera;
    //for ray hit test
    Ray ray;
    RaycastHit hit;
    GameObject hited;

    // Use this for initialization
    void Start () {

        theCamera = GameObject.Find("Main Camera");
        distance = (theCamera.transform.position - new Vector3(0, 0, 0)).magnitude;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            position = Input.mousePosition;
            cameraPosition = Camera.main.transform.position;
        }
        //right hold to drag
        if (Input.GetMouseButton(1))
        {
            Vector3 change = new Vector3(Input.mousePosition.x - position.x, 0, Input.mousePosition.y - position.y);
            if (Camera.main.transform.position.y < 4)
            {
                speed = 0.01f;
            }
            else
            {
                speed = 0.05f;
            }
            Camera.main.transform.position = change * speed + cameraPosition;
        }
        //zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (distance > 3.5f)
            {
                if (distance > 4)
                {
                    distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
                }
                else
                {
                    distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
                }

                if (distance < 3.5f)
                {
                    distance = 3.5f;
                }
            }
            else
            {
                distance = 3.5f;
            }
            theCamera.transform.position = new Vector3(theCamera.transform.position.x, distance, theCamera.transform.position.z);

        }
        //zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (distance < 20)
            {
                if (distance > 4)
                {
                    distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
                }
                else
                {
                    distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
                }

                if (distance < 3.5f)
                {
                    distance = 3.5f;
                }
            }
            else
            {
                distance = 20;
            }
            theCamera.transform.position = new Vector3(theCamera.transform.position.x, distance, theCamera.transform.position.z);
        }
        //ray test
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {

            //for plane
            if (hit.collider.gameObject.name.Contains("mon"))
            {
                if (hited != null && hited != hit.collider.gameObject)
                {
                    HighlightableObject ho1 = hited.GetComponent<HighlightableObject>();
                    if (ho1 != null)
                    {
                        ho1.Off();
                    }
                    else
                    {
                        Debug.Log("ho1 is null");
                    }
                }
                hited = hit.collider.gameObject;
                HighlightableObject ho = hit.collider.gameObject.GetComponent<HighlightableObject>();
                if (ho != null)
                {
                    ho.ConstantOn(Color.red);
                }
                else
                {
                    Debug.Log("ho is null");
                }

            }
        }

    }
}
