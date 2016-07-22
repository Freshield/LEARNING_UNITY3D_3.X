#pragma strict

var obj : GameObject;

var times : int = 1;

function Start () {

	obj = GameObject.Find("Sphere");

}

function OnGUI () {

	if(GUILayout.Button("clone an instance", GUILayout.Height(50)))
	{
		var clone : GameObject = Instantiate(obj, obj.transform.position * 2 * times, obj.transform.rotation);

		times ++;

		Destroy(clone, 5);
	}

}