using UnityEngine;
using System.Collections;

public class test_linerender : MonoBehaviour {

    GameObject lineRenderObj;

    LineRenderer lineRender;

    int lineCount = 4;

    Vector3 v0 = new Vector3(1, 0, 0);
    Vector3 v1 = new Vector3(0, 1, 0);
    Vector3 v2 = new Vector3(0, 0, 1);
    Vector3 v3 = new Vector3(1, 0, 0);

    ArrayList positions;

    // Use this for initialization
    void Start () {

        lineRenderObj = GameObject.Find("line");

        lineRender = lineRenderObj.GetComponent<LineRenderer>();

        lineRender.SetVertexCount(lineCount);

        lineRender.SetWidth(0.2f, 0.2f);

        positions = new ArrayList{ v0, v1, v2, v3 };
        

        lineRender.SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));

    }
	
	// Update is called once per frame
	void Update () {

	
	}

    void OnGUI()
    {
        if (GUILayout.Button("add",GUILayout.Height(50)))
        {
            lineCount++;
            lineRender.SetVertexCount(lineCount);
            Vector3 temp = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
            positions.Add(temp);
            lineRender.SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));
        }
    }
}
