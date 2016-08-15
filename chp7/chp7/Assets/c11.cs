using UnityEngine;
using System.Collections;

public class c11 : MonoBehaviour {

    public Material material;

    void OnPostRender()
    {
        if (!material)
        {
            Debug.Log("need material");
            return;
        }

        material.SetPass(0);

        GL.LoadOrtho();

        GL.Begin(GL.LINES);

        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
