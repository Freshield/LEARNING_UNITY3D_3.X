#pragma strict

var obj : GameObject;

var scaleX : float = 0.5;
var scaleY : float = 0.5;
var scaleZ : float = 0.5;

function Start () {

	obj = GameObject.Find("Cube");

}

function OnGUI () {

	GUILayout.Label("x scale");
	scaleX = GUILayout.HorizontalSlider(scaleX, 0.5, 2.0, GUILayout.Width(100));

	GUILayout.Label("y scale");
	scaleY = GUILayout.HorizontalSlider(scaleY, 0.5, 2.0, GUILayout.Width(100));

	GUILayout.Label("z scale");
	scaleZ = GUILayout.HorizontalSlider(scaleZ, 0.5, 2.0, GUILayout.Width(100));

	obj.transform.localScale = Vector3(scaleX, scaleY, scaleZ);

}