using UnityEngine;
using System.Collections;
using DG.Tweening;

public class test_dotween : MonoBehaviour {

    Vector3[] positions;

    GameObject obj;

    Tweener tweener;

    public float hSliderValue = 0;

    // Use this for initialization
    void Start () {

        positions = new Vector3[5];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector3(i, i, 0);
        }

        obj = GameObject.Find("Cube");

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
        }
        else
        {
            hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0, 4, GUILayout.Width(200));

            tweener.Goto(hSliderValue, false);
        }

        Debug.Log(tweener.fullPosition);
        
        

    }
}
