#pragma strict

private var bg : Texture2D;

private var title : Texture2D;

private var tex : Object[];

private var x : int;

private var y : int;

private var nowFram : int;

private var mFrameCount : int;

private var fps : float = 5;

private var time : float = 0;

function Start () {

	bg = Resources.Load("bg");

	title = Resources.Load("title");

	tex = Resources.LoadAll("anim");

	x = Screen.width;
	y = 200;

}

function OnGUI () {

	GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), bg, ScaleMode.StretchToFill, true, 0);

	GUI.DrawTexture(Rect((Screen.width - title.width)>>1, 30, title.width, title.height), title, ScaleMode.StretchToFill, true, 0);

	DrawAnimation(tex, Rect(x, y, 40, 60));

	x --;

	if(x < -42)
	{
		x = 480;
	}

	GUI.Button(Rect(230, 200, 100, 30), "start game");
	GUI.Button(Rect(230, 240, 100, 30), "load game");
	GUI.Button(Rect(230, 280, 100, 30), "about game");
	GUI.Button(Rect(230, 320, 100, 30), "exit game");

}

function DrawAnimation(tex : Object[], rect : Rect)
{
	GUI.DrawTexture(rect, tex[nowFram], ScaleMode.StretchToFill, true, 0);

	time += Time.deltaTime;

	if(time >= 1.0 / fps)
	{
		nowFram ++;

		time = 0;

		if(nowFram >= tex.Length)
		{
			nowFram = 0;
		}
	}
}