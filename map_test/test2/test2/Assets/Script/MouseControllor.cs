using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class MouseControllor : MonoBehaviour {
    int MouseFrame = 0;
    Vector3 position;
    float speed = 0.05f;
    Vector3 cameraPosition;
    GameObject hited;
    GameObject label;
    Main main;

    public int button = 0;

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

    //for ray hit test
    Ray ray;
    RaycastHit hit;
    string focusObjName;
    GameObject focusObj;

    //for companion
    public int companionNumber;
    public int companionPlayButton = 0;
    public List<List<Drawer>> targetCompanion;
    Tweener companionTarget;
    public GameObject cylinderPrefab;
    public List<GameObject> cylinders;
    string labelText;
    string companionText = "";
    string targetText = "";
    int focusNumber = 0;


    // Use this for initialization
    void Start()
    {
        label = GameObject.Find("hitObjectText");

        theCamera = GameObject.Find("Main Camera");

        distance = (theCamera.transform.position - new Vector3(0, 0, 0)).magnitude;
        
        main = GameObject.Find("Main Camera").GetComponent<Main>();

        cylinders = new List<GameObject>();


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
                }

                if (Input.GetMouseButton(1))
                {
                    Vector3 change = new Vector3(Input.mousePosition.x - position.x,0, Input.mousePosition.y - position.y);
                    if (Camera.main.transform.position.y < 4)
                    {
                        speed = 0.01f;
                    }
                    else
                    {
                        speed = 0.05f;
                    }
                    Camera.main.transform.position = change * speed + cameraPosition;
                    //Debug.Log("hold left button position " + Input.mousePosition + " speed " + speed);
                }

                if (Input.GetMouseButtonUp(1))
                {
                    //Debug.Log("up left button position " + Input.mousePosition);
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
                        theCamera.transform.position = new Vector3(theCamera.transform.position.x, distance, theCamera.transform.position.z);
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
                        theCamera.transform.position = new Vector3(theCamera.transform.position.x, distance, theCamera.transform.position.z);
                    }

                }

                //the ray hit
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                
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
                        hited.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.005f);
                        label.GetComponent<Text>().text = "The target object is " + hited.name;
                        if (Input.GetMouseButton(0))
                        {
                            button = 1;
                            focusObj = hit.collider.gameObject;
                            focusObjName = "The target object is " + focusObj.name;
                        }
                    }
                }
                break;

            case 1:

                switch (cameraButton)
                {
                    //look at the target first
                    case 0:
                        changeTarget = theCamera.transform.DOLookAt(focusObj.transform.position, 0.8f);
                        changeTarget.SetAutoKill(false).SetEase(Ease.Linear);
                        changeTarget.Play();
                        cameraButton = 1;
                        break;
                    //set path and rotate
                    case 1:
                        if (changeTarget.IsComplete())
                        {
                            Vector3[] path = new Vector3[3];
                            path[0] = focusObj.transform.position + new Vector3(0, 22, 12);
                            path[1] = focusObj.transform.position + new Vector3(0, 8, 14.5f);
                            path[2] = focusObj.transform.position + new Vector3(0, 2.5f, 5);


                            changeTarget = theCamera.transform.DOPath(path, 3, PathType.CatmullRom, PathMode.Full3D, 5, null).SetLookAt(focusObj.transform.position);

                            changeTarget.SetAutoKill(false).SetEase(Ease.Linear);
                            changeTarget.Play();

                            theCamera.transform.DORotate(new Vector3(27, 180, 0), 3).Play();

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
                            distance = (theCamera.transform.position - focusObj.transform.position).magnitude;
                            button = 2;
                            cameraButton = 0;
                        }
                        break;
                    default:
                        break;
                }

                break;
            //focuse rotate
            case 2:
                if (Input.GetMouseButton(1))
                {
                    if (focusObj)
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

                //the ray hit
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.name.Contains("lane"))
                    {
                        if (hited != null && hited != focusObj)
                        {
                            hited.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.00f);
                        }
                        label.GetComponent<Text>().text = focusObjName;

                    }
                    else if (hited != hit.collider.gameObject)
                    {

                        if (hited != null && hited != focusObj)
                        {
                            hited.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.00f);

                        }
                        hited = hit.collider.gameObject;
                        label.GetComponent<Text>().text = focusObjName;
                    }
                    else
                    {
                        hited = hit.collider.gameObject;
                        hited.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.005f);
                        label.GetComponent<Text>().text = "The target object is " + hited.name;
                        if (Input.GetMouseButton(0))
                        {
                            button = 1;
                            focusObj.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.00f);
                            focusObj = hit.collider.gameObject;
                            focusObjName = "The target object is " + focusObj.name;
                        }
                    }
                }

                break;
            //for companion
            case 3:
                switch (companionPlayButton)
                {
                    //change carmera
                    case 0:
                        if (focusObj != null)
                        {
                            focusObj.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.00f);
                        }

                        targetCompanion = main.companions;
                        focusObj = targetCompanion[companionNumber][0].obj;

                        companionTarget = theCamera.transform.DOLookAt(focusObj.transform.position, 0.8f);
                        companionTarget.SetAutoKill(false).SetEase(Ease.Linear);
                        companionTarget.Play();
                        companionPlayButton = 1;
                        break;
                    //set path and rotate
                    case 1:
                        if (companionTarget.IsComplete())
                        {
                            Vector3[] path = new Vector3[2];
                            path[0] = focusObj.transform.position + new Vector3(-8, 5, -10);
                            path[1] = focusObj.transform.position + new Vector3(-14, 14, -17);

                            companionTarget = theCamera.transform.DOPath(path, 3, PathType.CatmullRom, PathMode.Full3D, 5, null).SetLookAt(focusObj.transform.position);
                            companionTarget.SetAutoKill(false).SetEase(Ease.Linear);
                            companionTarget.Play();

                            theCamera.transform.DORotate(new Vector3(31, 38, 0), 3).SetEase(Ease.Linear).Play();

                            //companion label
                            
                            for (int i = 0; i < targetCompanion[companionNumber].Count; i++)
                            {
                                companionText += targetCompanion[companionNumber][i].obj.name + " ";
                                //create new ball
                                GameObject ball = Instantiate(main.objPrefab);
                                ball.name = "companion" + i;

                                ball.transform.position = targetCompanion[companionNumber][i].obj.transform.position - Drawer.companionHeight;

                                ball.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", new Color(3f / 255f, 225f / 255f, 115f / 255f, 1));
                                ball.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.005f);
                            }
                            targetText = focusObj.name;
                            labelText = "Companion are "+companionText + "\nThe target object is "+focusObj.name;

                            label.GetComponent<Text>().DOText(labelText, 3).SetEase(Ease.Linear).Play();

                            


                            companionPlayButton = 2;

                        }
                        break;
                    case 2:
                        if (companionTarget.IsComplete())
                        {
                            for (int i = 0; i < targetCompanion[companionNumber].Count; i++)
                            {
                                GameObject ball = GameObject.Find("companion" + i);
                                GameObject cylinder = Instantiate(cylinderPrefab);
                                cylinder.transform.position += ball.transform.position;

                                ball.transform.DOMoveY(10.25f, 3).SetEase(Ease.Linear).Play();
                                cylinder.transform.DOMoveY(5, 3, true).SetEase(Ease.Linear).Play();

                            }
                            float temp;
                            companionTarget = DOTween.To(x => temp = x, 0, 12, 3);
                            companionTarget.SetAutoKill(false).SetEase(Ease.Linear);
                            companionTarget.Play();
                            companionPlayButton = 3;
                        }
                        break;
                    case 3:
                        if (companionTarget.IsComplete())
                        {

                            companionTarget = DOTween.To(x => main.hSliderValue = x,0, targetCompanion[companionNumber][0].WfirstPosition.time.totalTime,3);
                            companionTarget.SetAutoKill(false).SetEase(Ease.Linear);
                            companionTarget.Play();
                            companionPlayButton = 4;
                        }
                        break;
                    case 4:
                        if (companionTarget.IsComplete())
                        {
                            for (int i = 0; i < targetCompanion[companionNumber].Count; i++)
                            {
                                GameObject cylinder = Instantiate(cylinderPrefab);
                                targetCompanion[companionNumber][i].obj.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
                                targetCompanion[companionNumber][i].obj.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", new Color(3f / 255f, 225f / 255f, 115f / 255f, 1));
                                targetCompanion[companionNumber][i].obj.GetComponent<MeshRenderer>().material.SetFloat("_Outline", 0.005f);
                                cylinder.transform.position += (targetCompanion[companionNumber][i].obj.transform.position);
                                cylinders.Add(cylinder);
                            }
                            //prepare
                            Vector2 angles = theCamera.transform.eulerAngles;
                            x = angles.y;
                            y = angles.x;

                            if (GetComponent<Rigidbody>())
                            {
                                GetComponent<Rigidbody>().freezeRotation = true;
                            }

                            //distance = Vector3.Distance(target.position,transform.position);
                            distance = (theCamera.transform.position - focusObj.transform.position).magnitude;
                            companionPlayButton = 0;
                            button = 4;
                            main.companionPrepared = true;
                        }
                        break;
                    default:
                        break;
                }
                break;
            //companion control
            case 4:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    focusNumber++;
                    if (focusNumber >= targetCompanion[companionNumber].Count)
                    {
                        focusNumber = 0;
                    }
                    focusObj = targetCompanion[companionNumber][focusNumber].obj;
                    
                }

                if (Input.GetMouseButton(1))
                {
                    if (focusObj)
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

                //the ray hit
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.name.Contains("lane") || hit.collider.gameObject.name.Contains("cylinder"))
                    {
                        labelText = "Companion are " + companionText + "\nThe target object is " + focusObj.name;
                        label.GetComponent<Text>().text = labelText;

                    }
                    else if (hited != hit.collider.gameObject)
                    {

                        labelText = "Companion are " + companionText + "\nThe target object is " + focusObj.name;
                        hited = hit.collider.gameObject;
                        label.GetComponent<Text>().text = labelText;
                    }
                    else
                    {
                        hited = hit.collider.gameObject;
                        label.GetComponent<Text>().text = "The target object is " + hited.name;
                    }
                }
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
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + focusObj.transform.position;
        theCamera.transform.rotation = rotation;
        theCamera.transform.position = position;
    }
}
