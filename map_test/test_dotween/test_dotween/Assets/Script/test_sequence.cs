using UnityEngine;
using System.Collections;
using DG.Tweening;

public class test_sequence : MonoBehaviour {

    GameObject obj;

    Tweener tweener;

    Sequence sequence;

    Vector3[] positions;

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

        sequence = DOTween.Sequence();

        DOTween.defaultAutoPlay = AutoPlay.None;

        for (int i = 0; i < positions.Length; i++)
        {
            sequence.Append(obj.transform.DOMove(positions[i], 1, false));
        }

        sequence.SetAutoKill(false).SetEase(Ease.Linear);
        
	
	}
	
	// Update is called once per frame
	void OnGUI () {

        GUILayout.Label(obj.transform.position.ToString());


        if (GUILayout.Button("play", GUILayout.Height(50)))
        {
            sequence.PlayForward();
        }

        if (GUILayout.Button("pause", GUILayout.Height(50)))
        {
            sequence.Pause();
        }


        if (sequence.IsPlaying())
        {
            GUILayout.HorizontalSlider(sequence.fullPosition, 0, 10, GUILayout.Width(200));
            hSliderValue = sequence.fullPosition;
        }
        else
        {
            hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0, 10, GUILayout.Width(200));

            sequence.Goto(hSliderValue, false);
        }

    }
}
