#pragma strict

var addStr : String = "add test string";

function Start () {

}

function OnGUI () {

	if(GUI.Button(Rect(50, 50, 100, 30), addStr))
	{
		addStr  += addStr;
	}

	if(GUILayout.Button(addStr))
	{
		addStr += addStr;
	}

}