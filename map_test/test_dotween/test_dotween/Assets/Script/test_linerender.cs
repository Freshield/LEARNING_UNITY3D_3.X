using UnityEngine;
using System.Collections;

public class test_linerender : MonoBehaviour {
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    private int LengthOflineRenderer = 0;
    private int index = 0;
    LineRenderer lineRend;
    Vector3 position;
    // Use this for initialization
    void Start()
    {
        lineRend = gameObject.AddComponent<LineRenderer>();
        lineRend.SetColors(c1, c2);
        lineRend.SetWidth(0.1f, 0.1f);
        lineRend.SetVertexCount(LengthOflineRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        lineRend = GetComponent<LineRenderer>();
        if (Input.GetMouseButtonDown(0))
        {
            print("1");
            position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
            LengthOflineRenderer++;
            lineRend.SetVertexCount(LengthOflineRenderer);
        }
        //while (index
         print("2");
        lineRend.SetPosition(index, position);
        index++;
    }

void OnGUI()
{
    GUILayout.Label("鼠标的x轴" + Input.mousePosition.x);
    GUILayout.Label("鼠标的y轴" + Input.mousePosition.y);
}
}
