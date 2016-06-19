#pragma strict

function Start () {

}

function OnGUI () {

	GUILayout.BeginArea(Rect(0, 0, Screen.width, Screen.height));

	GUILayout.BeginHorizontal();
	GUILayout.BeginVertical();

	GUILayout.Box("test1");
	GUILayout.FlexibleSpace();
	GUILayout.Box("test2");
	GUILayout.EndVertical();


	GUILayout.FlexibleSpace();

	GUILayout.BeginVertical();
	GUILayout.Box("test3");
	GUILayout.FlexibleSpace();
	GUILayout.Box("test4");
	GUILayout.EndVertical();


	GUILayout.EndHorizontal();
	GUILayout.EndArea();
}