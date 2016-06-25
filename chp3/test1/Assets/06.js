#pragma strict

var scrollPosition : Vector2;

function Start () {

	scrollPosition[0] = 50;
	scrollPosition[1] = 50;

}



function OnGUI () {

	scrollPosition = GUI.BeginScrollView(Rect(0, 0, 300, 200), scrollPosition, Rect(0, 0, Screen.width, 300), true, true);

	GUI.Label(Rect(100, 40, Screen.width, 30), "test scroll view, test scroll view, test scroll view, test scroll view");

	GUI.EndScrollView();

}