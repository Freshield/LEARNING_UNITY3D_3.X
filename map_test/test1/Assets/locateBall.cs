using UnityEngine;
using System.Collections;

public class locateBall : MonoBehaviour {

    public GameObject prefab;

	// Use this for initialization
	void Start () {

        GameObject ball = new GameObject();
        

        GameObject o0 = Instantiate(prefab);
        GameObject o1 = Instantiate(prefab);
        GameObject o2 = Instantiate(prefab);
        GameObject o3 = Instantiate(prefab);
        
        o0.transform.position = new Vector3(-10, 10, -0.5f);
        o1.transform.position = new Vector3(0, 10, -0.5f);
        o2.transform.position = new Vector3(10, 10, -0.5f);
        o3.transform.position = new Vector3(-10, 0, -0.5f);

        o0.transform.parent = ball.transform;
        o1.transform.parent = ball.transform;
        o2.transform.parent = ball.transform;
        o3.transform.parent = ball.transform;


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
