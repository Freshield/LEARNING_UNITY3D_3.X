#pragma strict

var verticalValue : int = 0;

var horizontalValue : float = 0.0f;

function Start () {

}

function Update () {

}

function OnGUI () {
	verticalValue = GUI.VerticalSlider(Rect(25, 25, 30, 100), verticalValue, 100, 0);
	horizontalValue = GUI.HorizontalSlider(Rect(50, 25, 100, 30), horizontalValue, 0.0f, 100.0f);

	GUI.Label(Rect(10, 150, Screen.width, 30), "vertical: " + verticalValue + "%");
	GUI.Label(Rect(10, 180, Screen.width, 30), "horizontal: " + horizontalValue + "%");
}