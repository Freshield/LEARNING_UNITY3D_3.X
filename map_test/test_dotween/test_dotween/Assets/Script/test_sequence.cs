using UnityEngine;
using System.Collections;
using DG.Tweening;

public class test_sequence : MonoBehaviour {

    GameObject obj;

    Tweener tweener;

    Tweener sequence;

    Tweener Vsequence;

    Vector3[] positions;

    Vector3 myVector;
    Vector3 mVector;

    public float hSliderValue = 0;

    // Use this for initialization
    void Start () {


        DOTween.defaultAutoPlay = AutoPlay.None;


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

        myVector = obj.transform.position;
        mVector = obj.transform.position;


        Vector3[] endValues = positions;
        float[] durations = new[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f};
        Vsequence = DOTween.ToArray(() => myVector, x => myVector = x, endValues, durations);
        sequence = DOTween.ToArray(() => mVector, x => mVector = x, endValues, durations);

        Vsequence.SetAutoKill(false).SetEase(Ease.Linear);
        sequence.SetAutoKill(false).SetEase(Ease.Linear);

        Vsequence.Pause();
        sequence.Pause();



    }

    // Update is called once per frame
    void OnGUI () {
        
        GUILayout.Label(obj.transform.position.ToString());
        GUILayout.Label(myVector.ToString());


        if (GUILayout.Button("play", GUILayout.Height(50)))
        {
            Vsequence.PlayForward();
            sequence.PlayForward();

        }

        if (GUILayout.Button("pause", GUILayout.Height(50)))
        {
            Vsequence.Pause();
            sequence.Pause();
        }


        if (Vsequence.IsPlaying())
        {
            GUILayout.HorizontalSlider(Vsequence.fullPosition, 0, 10, GUILayout.Width(200));
            hSliderValue = Vsequence.fullPosition;
        }
        else
        {
            hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0, 10, GUILayout.Width(200));

            Vsequence.Goto(hSliderValue, false);
            sequence.Goto(hSliderValue, false);
        }

    }

    void Update()
    {

        obj.transform.position = myVector;

        drawLine();

    }

    void drawLine()
    {

        float timeNow = Vsequence.fullPosition;
        

        ArrayList positions = new ArrayList();
        //positions.Add(nowPosition);

        
        float count = timeNow;
        
        while (count > 0)
        {
            sequence.Goto(count);
            positions.Add(mVector);
            count -= 0.02f;
        }

        sequence.Goto(timeNow);


        obj.GetComponent<LineRenderer>().SetVertexCount(positions.Count);
        obj.GetComponent<LineRenderer>().SetPositions((Vector3[])positions.ToArray(typeof(Vector3)));

    }
}
