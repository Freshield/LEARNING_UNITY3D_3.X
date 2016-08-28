using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MouseControllor : MonoBehaviour {
    int MouseFrame = 0;
    Vector3 position;
    float speed = 0.05f;
    Vector3 cameraPosition;
    GameObject hited;
    GameObject label;

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
    void Start()
    {
        label = GameObject.Find("hitObjectText");

        theCamera = GameObject.Find("Main Camera");

    }

    // Update is called once per frame
    void Update()
    {

        switch (button)
        {
            case 0:

                if (Input.GetMouseButtonDown(1))
                {
                    position = Input.mousePosition;
                    cameraPosition = Camera.main.transform.position;
                    //Debug.Log("press left button position " + Input.mousePosition);
                }

                if (Input.GetMouseButton(1))
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

                if (Input.GetMouseButtonUp(1))
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
                    if (Camera.main.transform.position.z > -50)
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
                    }

                }

                //the ray hit
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.name.Contains("lane"))
                    {
                        if (hited != null)
                        {
                            hited.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.00f);
                        }
                        label.GetComponent<Text>().text = "";

                    }
                    else if (hited != hit.collider.gameObject)
                    {

                        if (hited != null)
                        {
                            hited.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.00f);

                        }
                        hited = hit.collider.gameObject;
                        label.GetComponent<Text>().text = "";
                    }
                    else
                    {
                        hited = hit.collider.gameObject;
                        hited.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.01f);
                        label.GetComponent<Text>().text = "The target object is " + hited.name;
                        if (Input.GetMouseButton(0))
                        {
                            Debug.Log("here");
                            button = 1;
                            hited = hit.collider.gameObject;
                        }
                    }
                }
                break;

            case 1:

                switch (cameraButton)
                {
                    //look at the target first
                    case 0:
                        Debug.Log("here1");
                        changeTarget = theCamera.transform.DOLookAt(hited.transform.position, 1);
                        changeTarget.SetAutoKill(false).SetEase(Ease.Linear);
                        changeTarget.Play();
                        cameraButton = 1;
                        break;
                    //set path and rotate
                    case 1:
                        Debug.Log("here2");
                        if (changeTarget.IsComplete())
                        {
                            Vector3[] path = new Vector3[3];
                            path[0] = hited.transform.position + new Vector3(0, 12, -12);
                            path[1] = hited.transform.position + new Vector3(0, 8, -14.5f);
                            path[2] = hited.transform.position + new Vector3(0, 2.5f, -5);


                            changeTarget = theCamera.transform.DOPath(path, 3, PathType.CatmullRom, PathMode.Full3D, 5, null).SetLookAt(hited.transform.position);

                            changeTarget.SetAutoKill(false).SetEase(Ease.Linear);
                            changeTarget.Play();

                            theCamera.transform.DORotate(new Vector3(27, 2, 0), 3).Play();

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
