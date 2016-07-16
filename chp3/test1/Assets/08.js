#pragma strict

private var window0 : Rect = Rect (20, 20, 200, 200);
private var window1 : Rect = Rect (250, 20, 200, 200);

function Start () {

}

function OnGUI () {

	GUI.Window( 0, window0, oneWindow, "first window");
	GUI.Window( 1, window1, twoWindow, "second window");

}

function oneWindow(windowID : int)
{
	GUI.Box(Rect(10, 50, 150, 50), " this window's ID is: " + windowID);
	if(GUI.Button(Rect(10, 120, 150, 50), "normal button"))
	{
		Debug.Log("window ID " + windowID);
	}
}

function twoWindow(windowID : int)
{
	GUI.Box(Rect(10, 50, 150, 50), " this window's ID is: " + windowID);
	if(GUI.Button(Rect(10, 120, 150, 50), "normal button"))
	{
		Debug.Log("window ID " + windowID);
	}
}