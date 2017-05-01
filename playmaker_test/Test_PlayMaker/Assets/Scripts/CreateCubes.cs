using UnityEngine;
using System.Collections;

public class CreateCubes : MonoBehaviour {

    public GameObject cubePrefab;
    public GameObject cubeParent;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < 10; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.name = i.ToString();
            cube.transform.position = new Vector3(i + (0.2f * i), 0, 0);
            cube.transform.parent = cubeParent.transform;

        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
