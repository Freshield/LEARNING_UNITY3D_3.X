using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class test_line2 : MonoBehaviour {
    // Use this for initialization  

    private List<Vector3> list;
    private bool IsDraw = false;
    private LineRenderer lineRenderer;



    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame  
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (list == null)
                list = new List<Vector3>();

            list.Clear();
            IsDraw = true;
            lineRenderer.SetVertexCount(0);

        }
        if (Input.GetMouseButton(1))
        {
            list.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
        }

        if (Input.GetMouseButtonUp(1))
        {
            IsDraw = false;
        }



        drawBezierCurve();
        //drawInputPointCurve();  

    }

    private void drawBezierCurve()
    {
        if (IsDraw && list.Count > 0)
        {
            List<Vector3> bcList;
            //          BezierCurve bc= new BezierCurve();  
            
            BezierPath bc = new BezierPath();

            //bcList = bc.CreateCurve(list);//  通过贝塞尔曲线 平滑  
            bcList = bc.CreateCurve(list);//  通过贝塞尔曲线 平滑  

            lineRenderer.SetVertexCount(bcList.Count);
            for (int i = 0; i < bcList.Count; i++)
            {
                Vector3 v = bcList[i];
                v += new Vector3(0, 0.5f, 0);
                lineRenderer.SetPosition(i, v);
            }

        }

    }
}
