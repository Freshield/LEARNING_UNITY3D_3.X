#pragma strict

var texture : Texture2D;

function Start () {

}

function OnGUI () {

	GUILayout.BeginHorizontal();
	GUILayout.Box("begin horizontal");
	GUILayout.Button("button");
	GUILayout.Label("label");
	GUILayout.TextField("textfield");
	GUILayout.Box(texture);
	GUILayout.EndHorizontal();

	GUILayout.BeginVertical();
	GUILayout.Box("begin vertical");
	GUILayout.Button("button");
	GUILayout.Label("label");
	GUILayout.TextField("textfield");
	GUILayout.Box(texture);
	GUILayout.EndVertical();


}