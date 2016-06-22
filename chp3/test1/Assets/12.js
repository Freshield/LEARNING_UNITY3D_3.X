#pragma strict

function Start () {

}

function OnGUI () {

	GUILayout.Button("width 300, height 30", GUILayout.Width(300),GUILayout.Height(30));
	GUILayout.Button("minwidth 100, minheight 20", GUILayout.MinWidth(100),GUILayout.MinHeight(20));
	GUILayout.Button("maxwidth 400, maxheight 40", GUILayout.MaxWidth(400),GUILayout.MaxHeight(40));
	GUILayout.Button("expandwidth false", GUILayout.ExpandWidth(false));
	GUILayout.Button("expandwidth true", GUILayout.ExpandWidth(true));

}