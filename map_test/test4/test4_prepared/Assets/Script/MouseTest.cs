﻿using UnityEngine;
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
    MainTest mainTest;

    int mouseFlow = 0;

    public bool mouseLock = false;

    // Use this for initialization
    void Start () {

        theCamera = GameObject.Find("Main Camera");
        distance = (theCamera.transform.position - new Vector3(0, 0, 0)).magnitude;
        mainTest = GameObject.Find("Main Camera").GetComponent<MainTest>();
    }
	
	// Update is called once per frame
	void Update () {
        switch (mouseFlow)
        {
            case 0:
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

                if (mouseLock)
                {
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

                            if (mainTest.map.zoom != 15)
                            {
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

                            if (Input.GetMouseButtonUp(0))
                            {
                                if (mainTest.map.zoom < 15)
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
                                    GameObject tempPlane = hit.collider.gameObject;
                                    Debug.Log(tempPlane.name);
                                    string[] tempString = tempPlane.name.Split('r');
                                    Debug.Log(tempString[1]);
                                    Debug.Log(Map.monitor88Backplane44(int.Parse(tempString[1])));
                                    tempPlane = GameObject.Find("Plane" + Map.monitor88Backplane44(int.Parse(tempString[1])));
                                    mainTest.center = Track.world2position(new VecTime(tempPlane.transform.position, new PTime(0)), mainTest.center, mainTest.map.fullLat, mainTest.map.fullLon);
                                    switch (mainTest.map.zoom)
                                    {
                                        case 9:
                                            mainTest.lastLevel_9 = Map.monitor88Backplane44(int.Parse(tempString[1]));
                                            break;
                                        case 11:
                                            mainTest.lastLevel_11[0] = mainTest.lastLevel_9;
                                            mainTest.lastLevel_11[1] = Map.monitor88Backplane44(int.Parse(tempString[1]));
                                            break;
                                        case 13:
                                            mainTest.lastLevel_13[0] = mainTest.lastLevel_9;
                                            mainTest.lastLevel_13[1] = mainTest.lastLevel_11[1];
                                            mainTest.lastLevel_13[2] = Map.monitor88Backplane44(int.Parse(tempString[1]));
                                            break;
                                        default:
                                            break;
                                    }
                                    theCamera.transform.position = new Vector3(0, 7, 0);
                                    distance = (theCamera.transform.position - new Vector3(0, 0, 0)).magnitude;
                                    mainTest.flow = 3;
                                }


                            }
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                if (mainTest.map.zoom == 15)
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
                                    mainTest.lastLevel_9 = -1;
                                    mainTest.lastLevel_11 = new int[2] { -1, -1 };
                                    mainTest.lastLevel_13 = new int[3] { -1, -1, -1 };
                                    theCamera.transform.position = new Vector3(0, 7, 0);
                                    distance = (theCamera.transform.position - new Vector3(0, 0, 0)).magnitude;
                                    mainTest.flow = 4;
                                }
                            }
                        }
                    }
                }
                
                break;
            default:
                break;
        }
        

    }
}
