#pragma strict

private var objCube : GameObject;

private var objSphere : GameObject;

private var isCubeRoate = false;

private var isSphereRoate = false;

private var CubeInfo : String = "rotate cube";

private var SphereInfo : String = "rotate sphere";

function Start () {

	objCube = GameObject.Find("Cube");
	objSphere = GameObject.Find("Object/Sphere");

}

function Update () {

	if(isCubeRoate)
	{
		if(objCube)
		{
			objCube.transform.Rotate(0.0f, Time.deltaTime * 200, 0.0f);
		}
	}

	if(isSphereRoate)
	{
		if(objSphere)
		{
			objSphere.transform.Rotate(0.0f, Time.deltaTime * 200, 0.0f);
		}
	}


}

function OnGUI()
{
	if(GUILayout.Button(CubeInfo, GUILayout.Height(50)))
	{
		if(!isCubeRoate)
		{
			isCubeRoate = true;
			CubeInfo = "stop rotate cube";
		}
		else
		{
			isCubeRoate = false;
			CubeInfo = "rotate cube";
		}
	}

	if(GUILayout.Button(SphereInfo, GUILayout.Height(50)))
	{
		if(!isSphereRoate)
		{
			isSphereRoate = true;
			SphereInfo = "stop rotate Sphere";
		}
		else
		{
			isSphereRoate = false;
			SphereInfo = "rotate Sphere";
		}
	}

	if(GUILayout.Button("destroy model", GUILayout.Height(50)))
	{
		Destroy(objCube);
		Destroy(objSphere);
	}
}