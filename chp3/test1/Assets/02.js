#pragma strict

var buttonTexture : Texture2D;

private var str : String;

private var frameTime : int;


function Start () {

	str = "please press the button";

}

function OnGUI () {

	GUI.Label(Rect(10, 10, Screen.width, 30), str);

	if(GUI.Button(Rect(10, 50, buttonTexture.width, buttonTexture.height), buttonTexture))
	{
		str = "you press the image button";
	}
	GUI.color = Color.green;

	GUI.backgroundColor = Color.red;

	if(GUI.Button(Rect(10, 200, 70, 30), "word button"))
	{
		str = "you press the word button";
	}
	GUI.color = Color.yellow;

	GUI.backgroundColor = Color.black;

	if(GUI.RepeatButton(Rect(10, 250, 100, 30), "in pressing"))
	{
		str = "pressing time " + frameTime;
		frameTime ++; 
	} 

}