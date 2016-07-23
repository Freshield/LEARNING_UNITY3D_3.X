#pragma strict

var obj : GameObject;

function Start () {

	obj = GameObject.Find("Cube");

}

function OnGUI () {

	if(GUILayout.Button("move forward", GUILayout.Height(50)))
	{
		obj.transform.Translate(Vector3.forward * Time.deltaTime);
	}

	if(GUILayout.Button("move backward", GUILayout.Height(50)))
	{
		obj.transform.Translate(-Vector3.forward * Time.deltaTime);
	}

	if(GUILayout.Button("move left", GUILayout.Height(50)))
	{
		obj.transform.Translate(Vector3.left * Time.deltaTime);
	}

	if(GUILayout.Button("move right", GUILayout.Height(50)))
	{
		obj.transform.Translate(Vector3.right * Time.deltaTime);
	}

	if(GUILayout.Button("move up", GUILayout.Height(50)))
	{
		obj.transform.Translate(Vector3.up * Time.deltaTime);
	}

	if(GUILayout.Button("move down", GUILayout.Height(50)))
	{
		obj.transform.Translate(-Vector3.up * Time.deltaTime);
	}

	GUILayout.Label("the cube's position is: " + obj.transform.position);

}