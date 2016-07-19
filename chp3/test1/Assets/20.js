#pragma strict

private var anim : Object[];

private var nowFram : int;

private var mFrameCount : int;

private var fps : float = 5;

private var time : float = 0;

function Start () {

	anim = Resources.LoadAll("animation");

	mFrameCount = anim.Length;

}

function OnGUI () {

	DrawAnimation(anim,Rect(100, 100, 32, 48));

}

function DrawAnimation(tex:Object[], rect : Rect)
{
	GUILayout.Label("anima playing now: the " + nowFram + " fps");
	GUI.DrawTexture(rect, tex[nowFram], ScaleMode.StretchToFill, true, 0);
	time += Time.deltaTime;
	if(time >= 1.0 / fps)
	{
		nowFram++;
		time = 0;
		if(nowFram >= mFrameCount)
		{
			nowFram = 0;
		}
	}
}