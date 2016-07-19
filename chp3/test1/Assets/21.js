#pragma strict

private var animUp : Object[];
private var animDown : Object[];
private var animLeft : Object[];
private var animRight : Object[];

private var map : Texture2D;

private var tex : Object[];

private var x : int = 80;
private var y : int = 120;

private var imgSizex : int;
private var imgSizey : int;
private var imgChange : int;

private var buttonSizex : int;
private var buttonSizey : int;

private var nowFram : int;

private var mFramCount : int;

private var fps : float = 5;

private var time : float = 0;

//var mySkin : GUISkin;


function Start () {


	animUp = Resources.LoadAll("up");
	animDown = Resources.LoadAll("down");
	animLeft = Resources.LoadAll("left");
	animRight = Resources.LoadAll("right");

	map = Resources.Load("map/map");

	tex = animUp;

	imgSizex = Screen.width / 8;
	imgSizey = Screen.height / 8;
	imgChange = Screen.width / 100;
	buttonSizex = Screen.width / 5;
	buttonSizey = Screen.height / 15;

	x = Screen.width / 2;
	y = Screen.height / 2;

}

function OnGUI () {


	//mySkin.button.fontSize = 25;

	GUI.skin.button.fontSize = Screen.width / 17;

	GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), map, ScaleMode.StretchToFill, true, 0);

	DrawAnimation(tex, Rect(x, y, imgSizex, imgSizey));

	if(GUILayout.RepeatButton("up",GUILayout.Width(buttonSizex),GUILayout.Height(buttonSizey)))
	{
		y -= imgChange;
		tex = animUp;
	}

	if(GUILayout.RepeatButton("down",GUILayout.Width(buttonSizex),GUILayout.Height(buttonSizey)))
	{
		y += imgChange;
		tex = animDown;
	}

	if(GUILayout.RepeatButton("left",GUILayout.Width(buttonSizex),GUILayout.Height(buttonSizey)))
	{
		x -= imgChange;
		tex = animLeft;
	}

	if(GUILayout.RepeatButton("right",GUILayout.Width(buttonSizex),GUILayout.Height(buttonSizey)))
	{
		x += imgChange;
		tex = animRight;
	}

	if(x < 0)
	{
		x = 0;
	}

	if(x > Screen.width - imgSizex)
	{
		x = Screen.width - imgSizex;
	}

	if(y < 0)
	{
		y = 0;
	}

	if(y > Screen.height - imgSizey)
	{
		y = Screen.height - imgSizey;
	}



}

function DrawAnimation(tex : Object[], rect : Rect)
{
	GUI.DrawTexture(rect, tex[nowFram], ScaleMode.StretchToFill, true, 0);

	time += Time.deltaTime;

	if(time >= 1.0 / fps)
	{
		nowFram++;

		time = 0;

		if(nowFram >= tex.Length)
		{
			nowFram = 0;
		}
	}
}