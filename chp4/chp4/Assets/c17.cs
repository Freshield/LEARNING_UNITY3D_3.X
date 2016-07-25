using UnityEngine;
using System.Collections;

public class c17 : MonoBehaviour {

    bool isRotation = false;
    float total = 0;
	// Use this for initialization
	void Start () {



	
	}

    void OnGUI()
    {
        if (GUILayout.Button("rotate fix angle",GUILayout.Height(50)))
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 50.0f, 0.0f);
        }

        if (GUILayout.Button("rotate add angle", GUILayout.Height(50)))
        {
            isRotation = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        if (isRotation)
        {
            total += Time.deltaTime * 0.1f;

            if (total >= 1.0f)
            total = 1.0f;
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(0.0f, 50.0f, 0.0f), total);
            
        }
	
	}
}
