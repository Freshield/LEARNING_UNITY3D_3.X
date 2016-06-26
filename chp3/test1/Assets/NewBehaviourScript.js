#pragma strict

var str : String;

var imageTexture : Texture;

private var imageWidth : int;

private var imageHeight : int;

private var screenWidth : int;

private var screenHeight : int;


function Start () {

	screenWidth = Screen.width;
	screenHeight = Screen.height;

	imageWidth = imageTexture.width;
	imageHeight = imageTexture.height;

}

function OnGUI () {

	GUI.Label(Rect(100, 10, 100, 30), str);
	GUI.Label(Rect(100, 40, 100, 30), "the screenWidht: " + screenWidth);
	GUI.Label(Rect(100, 80, 100, 30), "the screenHeight: " + screenHeight);

	GUI.Label(Rect(100, 120, imageWidth, imageHeight), imageTexture);

}