#pragma strict

var obj : GameObject;

function Start () {

	obj = GameObject.Find("Cube");

}

function OnGUI () {

	if(GUILayout.Button("add script",GUILayout.Height(50)))
	{
		if(obj)
		{
			obj.AddComponent.<cube_script>();
		}
	}

	if(GUILayout.Button("delete script",GUILayout.Height(50)))
	{
		if(obj)
		{
			Destroy(obj.GetComponent("cube_script"));
		}
	}

	if(GUILayout.Button("delete cube",GUILayout.Height(50)))
	{
		if(obj)
		{
			Destroy(obj);
		}
	}

	if(GUILayout.Button("delete cube in 5 seconds",GUILayout.Height(50)))
	{
		if(obj)
		{
			Destroy(obj, 5);
		}
	}


}