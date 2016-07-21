#pragma strict

private var obj : GameObject;

function Start () {

	obj = GameObject.FindWithTag("MyTag");

}

function Update () {

	obj.transform.Rotate(0.0f, Time.deltaTime * 200, 0.0f);

}