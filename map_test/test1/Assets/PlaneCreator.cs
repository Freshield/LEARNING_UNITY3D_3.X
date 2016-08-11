using UnityEngine;
using System.Collections;

public class PlaneCreator : MonoBehaviour {

    public GameObject[] planes;
    public GameObject plane_parent;

    public PlaneCreator(Vector3 center , int x, int y, float width,GameObject prefab)
    {
        planes = new GameObject[x * y];

        plane_parent = new GameObject("plane_parent");

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                GameObject plane = Instantiate(prefab);
                plane.name = "plane" + (x * i + j);
                plane.transform.parent = plane_parent.transform;
                float hor = (((-x+1) * width) / 2) + (width * j);
                float ver = (((y-1) * width) / 2) - (width * i);
                Debug.Log(plane.name + " " + hor + "," + ver);
                plane.transform.position = new Vector3(hor + center.x, ver + center.y, center.z);
                planes[x * i + j] = plane;
            }
        }
        
    }

    
	
}
