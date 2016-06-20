#pragma strict

function Start () {

}

function OnGUI () {

	GUILayout.BeginArea(Rect(0, 0, 200, 60));
	GUILayout.BeginHorizontal();
	GUILayout.BeginVertical();
	GUILayout.Box("test1");

	GUILayout.Space(10);
	GUILayout.Box("text2");
	GUILayout.EndVertical();

	GUILayout.Space(20);

	GUILayout.BeginVertical();
	GUILayout.Box("test3");

	GUILayout.Space(10);
	GUILayout.Box("test4");

	GUILayout.EndVertical();

	GUILayout.EndHorizontal();

	GUILayout.EndArea();
}