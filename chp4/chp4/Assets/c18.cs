using UnityEngine;
using System.Collections;

public class c18 : MonoBehaviour {

    GameObject plane;

    GameObject cube;

    float mapWidth;
    float mapHeight;

    float widthCheck;
    float heightCheck;

    float mapcube_x = 0;
    float mapcube_y = 0;

    bool keyUp;
    bool keyDown;
    bool keyLeft;
    bool keyRight;

    public Texture map;
    public Texture map_cube;

	// Use this for initialization
	void Start () {

        plane = GameObject.Find("Plane");
        cube = GameObject.Find("Cube");

        float size_x = plane.GetComponent<MeshFilter>().mesh.bounds.size.x;
        float scal_x = plane.transform.localScale.x;
        float size_z = plane.GetComponent<MeshFilter>().mesh.bounds.size.z;
        float scal_z = plane.transform.localScale.z;

        mapWidth = size_x * scal_x;
        mapHeight = size_z * scal_z;

        widthCheck = mapWidth / 2;
        heightCheck = mapHeight / 2;

        check();

    }

    void OnGUI()
    {
        keyUp = GUILayout.RepeatButton("go forward");
        keyDown = GUILayout.RepeatButton("go back");
        keyLeft = GUILayout.RepeatButton("go left");
        keyRight = GUILayout.RepeatButton("go right");

        GUI.DrawTexture(new Rect(Screen.width - map.width, 0, map.width, map.height), map);
        GUI.DrawTexture(new Rect(mapcube_x, mapcube_y, map_cube.width, map_cube.height), map_cube);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (keyUp)
        {
            cube.transform.Translate(Vector3.forward * Time.deltaTime * 5);
            check();
        }

        if (keyDown)
        {
            cube.transform.Translate(-Vector3.forward * Time.deltaTime * 5);
            check();
        }

        if (keyLeft)
        {
            cube.transform.Translate(Vector3.left * Time.deltaTime * 5);
            check();
        }

        if (keyRight)
        {
            cube.transform.Translate(Vector3.right * Time.deltaTime * 5);
            check();
        }

    }

    void check()
    {
        float x = cube.transform.position.x;
        float z = cube.transform.position.z;

        if (x >= widthCheck)
        {
            x = widthCheck;
        }

        if (x <= -widthCheck)
        {
            x = -widthCheck;
        }

        if (z >= heightCheck)
        {
            z = heightCheck;
        }

        if (z <= -heightCheck)
        {
            z = -heightCheck;
        }

        cube.transform.position = new Vector3(x, cube.transform.position.y, z);

        mapcube_x = (map.width / mapWidth * x) + ((map.width / 2) - (map_cube.width / 2)) +
            (Screen.width - map.width);
        mapcube_y = map.height - ((map.height / mapHeight * z) + (map.height / 2));
    }
}
