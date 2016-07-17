#pragma strict

var myskin : GUISkin;

private var choose : boolean = false;

var windowRect : Rect = Rect(20, 20, 220, 250);

var edit : String = "please input words";

function Start () {

}

function OnGUI () {

	GUI.skin = myskin;

	GUI.Button(Rect(100,100,100,100), "button");

	choose = GUI.Toggle(Rect(10, 50, 100, 30), choose, "toggle");

	edit = GUI.TextField(Rect(200, 10, 200, 20), edit, 25);

	windowRect = GUI.Window(0, windowRect, setWindow, "this is a window");

}

function setWindow(windowID : int)
{
	GUI.DragWindow();
	GUI.Button(Rect(10,20,100,30),"button");
}