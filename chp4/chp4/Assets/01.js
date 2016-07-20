#pragma strict

function Start () {


}

function OnGUI () {

	if(GUILayout.Button("create cube",GUILayout.Height(50)))
	{
		var objCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

		objCube.AddComponent(Rigidbody);

		objCube.name = "Cube";

		objCube.GetComponent.<Renderer>().material.color = Color.blue;

		objCube.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
	}

	if(GUILayout.Button("create sphere",GUILayout.Height(50)))
	{
		var objSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

		objSphere.AddComponent(Rigidbody);

		objSphere.name = "Sphere";

		objSphere.GetComponent.<Renderer>().material.color = Color.red;

		objSphere.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
	}


}