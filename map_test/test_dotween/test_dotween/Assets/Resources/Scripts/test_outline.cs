using UnityEngine;
using System.Collections;
using DG.Tweening;

public class test_outline : MonoBehaviour {

    GameObject hited;

    int button = 0;

    int cameraButton = 0;

    Tweener changeTarget;

    GameObject theCamera;

    //for rotate mouse
    public float distance;

    float x;
    float y;

    const float yMinLimit = 0.0f;
    const float yMaxLimit = 80.0f;

    const float xSpeed = 250.0f;
    const float ySpeed = 120.0f;

    // Use this for initialization
    void Start () {

        theCamera = GameObject.Find("Main Camera");

        //obj.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.01f);

    }
	
	// Update is called once per frame
	void Update () {

        switch (button)
        {
            case 0:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    hited = hit.collider.gameObject;
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.01f);
                    if (Input.GetMouseButton(0))
                    {
                        button = 1;
                        hited = hit.collider.gameObject;
                    }
                }
                else
                {
                    Debug.Log("not hit");
                    if (hited != null)
                    {
                        hited.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.00f);
                    }

                }
                break;

            case 1:

                switch (cameraButton)
                {
                    //look at the target first
                    case 0:
                        changeTarget = theCamera.transform.DOLookAt(hited.transform.position, 1);
                        changeTarget.SetAutoKill(false).SetEase(Ease.Linear);
                        cameraButton = 1;
                        break;
                    //set path and rotate
                    case 1:
                        if (changeTarget.IsComplete())
                        {
                            Vector3[] path = new Vector3[3];
                            path[0] = hited.transform.position + new Vector3(0, 12, -12);
                            path[1] = hited.transform.position + new Vector3(0, 8, -14.5f);
                            path[2] = hited.transform.position + new Vector3(0, 2.5f, -5);


                            changeTarget = theCamera.transform.DOPath(path, 3, PathType.CatmullRom, PathMode.Full3D, 5, null).SetLookAt(hited.transform.position);

                            changeTarget.SetAutoKill(false).SetEase(Ease.Linear);

                            theCamera.transform.DORotate(new Vector3(27, 2, 0), 3);

                            cameraButton = 2;

                        }
                        break;
                    //when finished change to next situation
                    case 2:
                        if (changeTarget.IsComplete())
                        {
                            //prepare
                            Vector2 angles = theCamera.transform.eulerAngles;
                            x = angles.y;
                            y = angles.x;
                            Debug.Log("fist x y " + x + "," + y);

                            if (GetComponent<Rigidbody>())
                            {
                                GetComponent<Rigidbody>().freezeRotation = true;
                            }

                            //distance = Vector3.Distance(target.position,transform.position);
                            distance = (theCamera.transform.position - hited.transform.position).magnitude;
                            button = 2;
                        }
                        break;
                    default:
                        break;
                }
                
                break;
            //focuse rotate
            case 2:
                if (Input.GetMouseButton(0))
                {
                    if (hited)
                    {
                        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                        y = ClampAngle(y, yMinLimit, yMaxLimit);

                        
                        
                    }
                }

                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    //Debug.Log(Camera.main.transform.position.z);
                    if (distance > 0.7)
                    {
                        if (distance > 4)
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 10;
                        }
                        else
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
                        }

                    }

                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    //Debug.Log(Camera.main.transform.position.z);
                    if (distance < 50)
                    {
                        if (distance > 4)
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 10;
                        }
                        else
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
                        }
                    }

                }

                moveCamear();

                break;

            default:
                break;
        }



    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

    void moveCamear()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + hited.transform.position;
        theCamera.transform.rotation = rotation;
        theCamera.transform.position = position;
    }
}
