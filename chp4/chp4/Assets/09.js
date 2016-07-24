#pragma strict

private var objCube : GameObject;

private var objCylinder : GameObject;

private var objSphere : GameObject;

private var speed : int = 100;

private var model : int = 0;

function Start () {

	objCube = GameObject.Find("Cube");
	objCylinder = GameObject.Find("Cylinder");
	objSphere = GameObject.Find("Sphere1");
	//objSphere.transform.position = objCylinder.transform.position;
	//objSphere.transform.rotation = objCylinder.transform.rotation;

}

function Update () 
{
	if(model == 1)
	{
		objCube.transform.Rotate(Vector3(1, 0.0f, 0.0f) * Time.deltaTime * speed);
	}

	if(model == 2)
	{
		objCube.transform.Rotate(Vector3.up * Time.deltaTime * speed);
	}

	if(model == 3)
	{
		objCube.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
	}

	if(model == 4)
	{
		objCube.transform.RotateAround(objCylinder.transform.position, objCylinder.transform.right, Time.deltaTime * speed);
		//objSphere.transform.Rotate(Vector3.up * Time.deltaTime * speed);

		objSphere.transform.RotateAround(objCylinder.transform.position, objCylinder.transform.up, Time.deltaTime * speed * 2);
	}
	
}

function OnGUI () {

	if(GUILayout.Button("cube rotate by x", GUILayout.Height(50)))
	{
		
		model = 1;
	}

	if(GUILayout.Button("cube rotate by y", GUILayout.Height(50)))
	{
		
		model = 2;
	}

	if(GUILayout.Button("cube rotate by z", GUILayout.Height(50)))
	{
		
		model = 3;
	}

	if(GUILayout.Button("cube rotate by cylinder", GUILayout.Height(50)))
	{
		
		model = 4;
	}

	GUILayout.Label("cube rotate value " + objCube.transform.rotation);

}