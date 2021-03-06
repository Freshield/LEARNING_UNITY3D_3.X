﻿using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class MouseControllor : MonoBehaviour {
    Vector3 position;
    float speed = 0.05f;
    Vector3 cameraPosition;
    GameObject hited;
    GameObject label;

    public int mouseFlow = 0;

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
    public Main main;


    // Use this for initialization
    void Start()
    {
        label = GameObject.Find("hitObjectText");

        theCamera = GameObject.Find("Main Camera");

        main = GameObject.Find("Main Camera").GetComponent<Main>();

        distance = (theCamera.transform.position - new Vector3(0, 0, 0)).magnitude;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (mouseFlow)
        {
            //for the orthographic
            case 0:
                //right press to log the position now
                if (Input.GetMouseButtonDown(1))
                {
                    position = Input.mousePosition;
                    cameraPosition = Camera.main.transform.position;
                }
                //right hold to drag
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
                }
                //zoom in
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (distance > 2.5f)
                    {
                        if (distance > 4)
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
                        }
                        else
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
                        }

                        if (distance < 2.5f)
                        {
                            distance = 2.5f;
                        }
                    }
                    else
                    {
                        distance = 2.5f;
                    }
                    theCamera.transform.position = new Vector3(theCamera.transform.position.x, distance, theCamera.transform.position.z);

                }
                //zoom out
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (distance < 50)
                    {
                        if (distance > 4)
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
                        }
                        else
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
                        }

                        if (distance < 2.5f)
                        {
                            distance = 2.5f;
                        }
                    }
                    else
                    {
                        distance = 50;
                    }
                    theCamera.transform.position = new Vector3(theCamera.transform.position.x, distance, theCamera.transform.position.z);
                }
                //the ray hit
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    
                    //for plane
                    if (hit.collider.gameObject.name.Contains("lane") || hit.collider.gameObject.name.Contains("board"))
                    {
                        if (hited != null)
                        {
                            var em = hited.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;
                        }
                        label.GetComponent<Text>().text = "";

                    }//for new hited object, empty the last hited
                    else if (hited != hit.collider.gameObject)
                    {

                        if (hited != null)
                        {
                            var em = hited.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;
                        }
                        hited = hit.collider.gameObject;
                        label.GetComponent<Text>().text = "";
                    }//for hited object now
                    else
                    {
                        hited = hit.collider.gameObject;
                        if (hited != focusObj)
                        {
                            var em = hited.GetComponent<ParticleSystem>().emission;
                            em.enabled = true;
                        }
                        label.GetComponent<Text>().text = "The target object is " + hited.name;
                        //left press to focus object
                        if (Input.GetMouseButtonUp(0))
                        {
                            mouseFlow = 1;
                            focusObj = hit.collider.gameObject;
                            focusObjName = "The target object is " + focusObj.name;
                            var em = focusObj.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;
                        }
                    }
                }
                break;
            //changing to focus object
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
                            //release last one
                            changeTarget.Kill();

                            Vector3[] path = new Vector3[2];
                            path[0] = focusObj.transform.position + new Vector3(0, 2.5f, 5);
                            path[1] = focusObj.transform.position + new Vector3(0, 1.5f, 3);
                            
                            changeTarget = theCamera.transform.DOPath(path, 3, PathType.CatmullRom, PathMode.Full3D, 5, null).SetLookAt(focusObj.transform.position);

                            changeTarget.SetAutoKill(false).SetEase(Ease.Linear);
                            changeTarget.Play();

                            theCamera.transform.DORotate(new Vector3(27, 180, 0), 3).Play();

                            cameraButton = 2;
                            //release
                            Array.Clear(path, 0, path.Length);
                        }
                        break;
                    //when finished change to next situation
                    case 2:
                        if (changeTarget.IsComplete())
                        {
                            //release last one
                            changeTarget.Kill();

                            //prepare
                            Vector2 angles = theCamera.transform.eulerAngles;
                            x = angles.y;
                            y = angles.x;

                            Drawer temp = null;
                            foreach (Drawer drawer in main.drawers)
                            {
                                if (drawer.obj.name == focusObj.name)
                                {
                                    temp = drawer;
                                }
                            }
                            temp.isFocus = true;
                            HighlightableObject ho = focusObj.GetComponent<HighlightableObject>();
                            if (ho != null)
                            {
                                ho.ConstantOn(Color.red);
                            }
                            else
                            {
                                Debug.Log("ho is null");
                            }
                            var em = hited.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;
                            em = focusObj.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;

                            if (GetComponent<Rigidbody>())
                            {
                                GetComponent<Rigidbody>().freezeRotation = true;
                            }
                            distance = (theCamera.transform.position - focusObj.transform.position).magnitude;
                            mouseFlow = 2;
                            cameraButton = 0;
                        }
                        break;
                    default:
                        break;
                }
                break;
            //focuse rotate
            case 2:
                //right press to drag
                if (Input.GetMouseButton(1))
                {
                    if (focusObj)
                    {
                        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                        y = ClampAngle(y, yMinLimit, yMaxLimit);
                    }
                }
                //zoom in
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (distance > 2.5f)
                    {
                        if (distance > 4)
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
                        }
                        else
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
                        }

                        if (distance < 2.5f)
                        {
                            distance = 2.5f;
                        }
                    }
                    else
                    {
                        distance = 2.5f;
                    }
                }
                //zoom out
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (distance < 50)
                    {
                        if (distance > 4)
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 5;
                        }
                        else
                        {
                            distance -= Input.GetAxis("Mouse ScrollWheel") * 2;
                        }

                        if (distance < 2.5f)
                        {
                            distance = 2.5f;
                        }
                    }
                    else
                    {
                        distance = 50;
                    }
                }
                //let camera focus on object
                moveCamera();
                //the ray hit
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    //for plane
                    if (hit.collider.gameObject.name.Contains("lane"))
                    {
                        if (hited != null && hited != focusObj)
                        {
                            var em = hited.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;
                        }
                        else
                        {
                            var em = focusObj.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;
                        }
                        label.GetComponent<Text>().text = focusObjName;

                    }//for new hited object, empty the last hited
                    else if (hited != hit.collider.gameObject)
                    {
                        if (hited != null && hited != focusObj)
                        {
                            var em = hited.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;
                        }
                        hited = hit.collider.gameObject;
                        label.GetComponent<Text>().text = focusObjName;
                    }//for new hited object
                    else
                    {
                        hited = hit.collider.gameObject;
                        if (hited != focusObj)
                        {
                            var em = hited.GetComponent<ParticleSystem>().emission;
                            em.enabled = true;
                        }
                        label.GetComponent<Text>().text = "The target object is " + hited.name;
                        if (Input.GetMouseButtonUp(0))
                        {
                            mouseFlow = 1;
                            Drawer temp = null;
                            foreach (Drawer drawer in main.drawers)
                            {
                                if (drawer.obj.name == focusObj.name)
                                {
                                    temp = drawer;
                                }
                            }
                            temp.isFocus = false;
                            HighlightableObject ho = focusObj.GetComponent<HighlightableObject>();
                            if (ho != null)
                            {
                                ho.Off();
                            }
                            else
                            {
                                Debug.Log("ho is null");
                            }
                            //focusObj.GetComponent<MeshRenderer>().material.SetFloat("_GlowStrength", 0);
                            //focusObj.GetComponent<LineRenderer>().material.SetColor("_Color", new Color(0, 97.0f/255.0f, 1, 1));
                            focusObj = hit.collider.gameObject;
                            focusObjName = "The target object is " + focusObj.name;
                            var em = focusObj.GetComponent<ParticleSystem>().emission;
                            em.enabled = false;


                        }
                    }
                }
                break;
            
            default:
                break;
        }
    }
    //limit angle
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
    //let camera focus on object
    void moveCamera()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + focusObj.transform.position;
        theCamera.transform.rotation = rotation;
        theCamera.transform.position = position;
    }
}
