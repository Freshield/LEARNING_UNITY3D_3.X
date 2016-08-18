using UnityEngine;
using System.Collections;
using DG.Tweening;

public class test_dotween : MonoBehaviour {

    Vector3[] positions;

    GameObject obj;

    GameObject train;

    Tweener tweener;

    Vector3 startPosition;

    public float hSliderValue = 0;

    // Use this for initialization
    void Start () {

        positions = new Vector3[10];

        for (int i = 0; i < 5; i++)
        {
            positions[i] = new Vector3(0, i, 0);
        }
        positions[5] = new Vector3(0, 4, 1);
        positions[6] = new Vector3(0, 4, 5);
        positions[7] = new Vector3(0, 5, 3);
        positions[8] = new Vector3(0, 2, 4);
        positions[9] = new Vector3(0, 1, 6);



        obj = GameObject.Find("Cube");

        startPosition = obj.transform.position;

        //train = GameObject.Find("CubeTrain");

        tweener = obj.transform.DOPath(positions, 4, PathType.CatmullRom, PathMode.Full3D, 5, null);

        tweener.SetAutoKill(false).SetEase(Ease.Linear);



        
	
	}
	
	// Update is called once per frame
	void Update () {

        
	
	}

    void OnGUI()
    {
        // Vector3 temp = tweener.PathGetPoint(0.15f);

        // Debug.Log(temp);


        if (GUILayout.Button("play",GUILayout.Height(50)))
        {
            tweener.PlayForward();
        }

        if (GUILayout.Button("pause",GUILayout.Height(50)))
        {
            tweener.Pause();
        }

        if (tweener.IsPlaying())
        {
            GUILayout.HorizontalSlider(tweener.fullPosition, 0, 4, GUILayout.Width(200));
            hSliderValue = tweener.fullPosition;
            drawLine();
        }
        else
        {
            hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0, 4, GUILayout.Width(200));

            tweener.Goto(hSliderValue, false);

            drawLine();
        }

        //Debug.Log(tweener.fullPosition);
        Debug.Log(tweener.PathLength());



    }

    void drawLine()
    {
        float timeNow = tweener.fullPosition;
        float percent = timeNow / 4;
        Vector3 nowPosition = tweener.PathGetPoint(percent);

        ArrayList positions = new ArrayList();
        //positions.Add(nowPosition);

        float count = timeNow;

        while (count > 0)
        {
            float per = count / 4;
            positions.Add(tweener.PathGetPoint(per));
            count -= 0.02f;
        }

        obj.GetComponent<LineRenderer>().SetVertexCount(positions.Count);
        obj.GetComponent<LineRenderer>().SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));

    }
}
