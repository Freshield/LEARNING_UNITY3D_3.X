using UnityEngine;
using System.Collections;

public class c15 : MonoBehaviour {

    public Material material;

    void OnPostRender()
    {
        if (!material)
        {
            Debug.Log("null material");
            return;
        }

        material.SetPass(0);

        GL.LoadOrtho();

        GL.Begin(GL.LINES);

        DrawLine(0, 0, 200, 100);

        DrawLine(0, 50, 200, 150);

        DrawLine(0, 100, 200, 200);

        GL.End();
    }

    void DrawLine(float x1, float y1, float x2, float y2)
    {
        GL.Vertex(new Vector3(x1 / Screen.width, y1 / Screen.height, 0));
        GL.Vertex(new Vector3(x2 / Screen.width, y2 / Screen.height, 0));
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
