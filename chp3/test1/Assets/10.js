#pragma strict

var mySkin : GUISkin;

function Start () {

}

function OnGUI () {

	GUI.skin = mySkin;

	GUI.Button(Rect(330, 100, 130, 110),"", "Custom1");
	GUI.Button(Rect(0, 100, 130, 110),"","Custom0");

}