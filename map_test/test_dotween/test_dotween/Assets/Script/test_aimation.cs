using UnityEngine;
using System.Collections;

public class test_aimation : MonoBehaviour {

    private GameObject obj;

    public float hSliderValue = 0;

    public float animLegth = 0;

	// Use this for initialization
	void Start () {

        obj = GameObject.Find("Cube");


        animLegth = GetComponent<Animation>().GetClip("cubeTest").length;

        Debug.Log(animLegth);
	
	}
	
	// Update is called once per frame
	void OnGUI () {

        string show = "the length is " + hSliderValue.ToString() + " (s)/" + animLegth.ToString() + "(s)";
        GUILayout.Label(show);


        hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0, 5, GUILayout.Width(200));

        //PlaySliderAnimation(obj, hSliderValue);

        if (GUILayout.Button("play",GUILayout.Height(50)))
        {
            AnimationClip clip = new AnimationClip();
            GetComponent<Animation>().AddClip(clip, "cutClip");
            //clip = GetComponent<Animation>().GetClip("cubeTest").Sam


        }

        GetComponent<Animation>().GetClip("cubeTest").SampleAnimation(this.gameObject, hSliderValue);



    }

    public void PlaySliderAnimation(GameObject target, float times)
    {
        target.GetComponent<Animation>().Play();
        target.GetComponent<Animation>().GetClip("cubeTest").SampleAnimation(target, times);
    }
}
