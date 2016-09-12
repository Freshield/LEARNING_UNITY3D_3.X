using UnityEngine;
using System.Collections.Generic;
using System;

public class c16 : MonoBehaviour {

    public Material material;

    private List<Vector3> lineInfo;

    Vector3 lastPosition;
    Vector3 newPosition;

	// Use this for initialization
	void Start () {

        lineInfo = new List<Vector3>();
        lastPosition = Input.mousePosition;
	
	}
	
	// Update is called once per frame
	void Update () {
        //lineInfo.Add(Input.mousePosition);
	}

    void OnGUI()
    {
        GUILayout.Label("mouse x is: " + Input.mousePosition.x);
        GUILayout.Label("mouse y is: " + Input.mousePosition.y);
    }

    void OnPostRender()
    {
        if (!material)
        {
            Debug.Log("null material");
            return;
        }

        newPosition = Input.mousePosition;

        material.SetPass(0);

        GL.LoadOrtho();

        GL.Begin(GL.LINES);

        /*
        int size = lineInfo.Count;

        for (int i = 0; i < size - 1; i++)
        {
            Vector3 start = lineInfo[i];
            Vector3 end = lineInfo[i + 1];
            DrawLine(start.x, start.y, end.x, end.y);
        }
        */
        DrawLine(lastPosition.x, lastPosition.y, newPosition.x, newPosition.y);


        GL.End();

        lastPosition = newPosition;
    }

    void DrawLine(float x1, float y1, float x2, float y2)
    {
        GL.Vertex(new Vector3(x1 / Screen.width, y1 / Screen.height, 0));
        GL.Vertex(new Vector3(x2 / Screen.width, y2 / Screen.height, 0));
    }
}
