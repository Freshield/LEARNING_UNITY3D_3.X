#pragma strict

var mySkin : GUISkin;

function Start () {

}

function OnGUI () {

	GUI.skin = mySkin;

	GUILayout.Box("华文宋体");

	GUILayout.Button("华文楷体");

	GUILayout.Label("默认字体");
}