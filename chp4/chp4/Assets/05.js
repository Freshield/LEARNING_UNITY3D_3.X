#pragma strict

private var obj : GameObject;

private var render : Renderer;

public var texture : Texture;

function Start () {

	obj = GameObject.Find("Cube");

	UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(obj, "Assets/05.js(13,9)", "Test");

	render = obj.GetComponent("Renderer");

}

function OnGUI () {

	if(GUILayout.Button("add color", GUILayout.Width(100),GUILayout.Height(50)))
	{
		render.material.color = Color.green;

		render.material.mainTexture = null;
	}

	if(GUILayout.Button("add texture",GUILayout.Width(100),GUILayout.Height(50)))
	{
		render.material = null;
		render.material.mainTexture = texture;
	
	}
}