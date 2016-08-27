using UnityEngine;
using System.Collections;
using DG.Tweening;

public class test_sequence : MonoBehaviour {

    GameObject obj;

    Tweener tweener;
    
    Tweener Vsequence;


    Vector3 myVector;

    public float hSliderValue = 0;

    bool button = true;

    bool isPlaying = false;

    // Use this for initialization
    void Start () {


        DOTween.defaultAutoPlay = AutoPlay.None;



        Vector3[] positions = new Vector3[10];

        positions[0] = new Vector3(0, 0, 0);

        for (int i = 1; i < 5; i++)
        {
            positions[i] = new Vector3(0, i, 0);
        }
        positions[5] = new Vector3(0, 4, 1);
        positions[6] = new Vector3(0, 4, 5);
        positions[7] = new Vector3(0, 5, 3);
        positions[8] = new Vector3(0, 2, 4);
        positions[9] = new Vector3(0, 1, 6);

        obj = GameObject.Find("Cube");

        myVector = obj.transform.position;
        

        Vector3[] endValues = positions;
        float[] durations = new[] { 0.01f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f};
        Vsequence = DOTween.ToArray(() => myVector, x => myVector = x, endValues, durations);
        
        Vsequence.SetAutoKill(false).SetEase(Ease.Linear);
        
        Vsequence.Pause();
        


    }

    // Update is called once per frame
    void OnGUI () {
        
        GUILayout.Label(obj.transform.position.ToString());
        GUILayout.Label(myVector.ToString());


        if (GUILayout.Button("play", GUILayout.Height(50)))
        {
            Vsequence.PlayForward();
            isPlaying = true;

        }

        if (GUILayout.Button("pause", GUILayout.Height(50)))
        {
            Vsequence.Pause();
            isPlaying = false;
        }


        if (isPlaying)
        {
            GUILayout.HorizontalSlider(Vsequence.fullPosition, 0, 10, GUILayout.Width(200));
            hSliderValue = Vsequence.fullPosition;

            drawLine();

            if (Vsequence.fullPosition == 10)
            {
                isPlaying = false;
            }
        }
        else
        {
            hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0, 10, GUILayout.Width(200));


            if (hSliderValue != Vsequence.fullPosition)
            {
                Vsequence.Goto(hSliderValue, false);
                
                drawLine();
            }

        }

    }

    void Update()
    {
        if (button)
        {
            obj.transform.position = myVector;
        }
        


    }

    void drawLine()
    {
        if (isPlaying)
        {
            Vsequence.Pause();
        }
        

        button = false;

        float timeNow = Vsequence.fullPosition;
        

        ArrayList positions = new ArrayList();
        
        float count = timeNow;
        
        while (count > 0)
        {
            Vsequence.Goto(count);
            positions.Add(myVector);
            count -= 0.03f;
        }

        Vsequence.Goto(timeNow);


        obj.GetComponent<LineRenderer>().SetVertexCount(positions.Count);
        obj.GetComponent<LineRenderer>().SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));

        button = true;

        if (isPlaying)
        {
            Vsequence.Play();
        }
        
    }
}
