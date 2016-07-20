#pragma strict

private var hero : GameObject;

private var keyUp : boolean;
private var keyDown : boolean;
private var keyLeft : boolean;
private var keyRight : boolean;

private var time : float;

private var fps : float = 10;

private var nowFram : int;

private var animUp : Object[];
private var animDown : Object[];
private var animLeft : Object[];
private var animRight : Object[];
private var nowAnim : Object[];
private var backAnim : Object[];

function Start () {

	hero = GameObject.Find("hero");

	animUp = Resources.LoadAll("up");
	animDown = Resources.LoadAll("down");
	animLeft = Resources.LoadAll("left");
	animRight = Resources.LoadAll("right");
	nowAnim = animDown;
	backAnim = animDown;

}

function OnGUI () {

	keyUp = GUILayout.RepeatButton("up");
	keyDown = GUILayout.RepeatButton("down");
	keyLeft = GUILayout.RepeatButton("left");
	keyRight = GUILayout.RepeatButton("right");

}

function FixedUpdate()
{
	if(keyUp)
	{
		SetAnimation(animUp);
		hero.transform.Translate(-Vector3.forward * 0.05f);
	}

	if(keyDown)
	{
		SetAnimation(animDown);
		hero.transform.Translate(Vector3.forward * 0.05f);
	}

	if(keyLeft)
	{
		SetAnimation(animLeft);
		hero.transform.Translate(Vector3.right * 0.05f);
	}

	if(keyRight)
	{
		SetAnimation(animRight);
		hero.transform.Translate(-Vector3.right * 0.05f);
	}

	DrawAnimation(nowAnim);
}


function DrawAnimation(tex : Object[])
{
	
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

	//var theRenderer = hero.GetComponent(Renderer);
	//theRenderer.material.mainTexture = tex[nowFram];
	hero.GetComponent.<Renderer>().material.mainTexture = tex[nowFram];
}

function SetAnimation(tex : Object[])
{
	nowAnim = tex;
	if(!backAnim.Equals(nowAnim))
	{
		nowFram = 0;
		backAnim = nowAnim;
	}

}