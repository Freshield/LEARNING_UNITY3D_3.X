#pragma strict

function Start () {

	var objs = GameObject.FindGameObjectsWithTag("MyTag");

	objs[5].tag = "TestTag";

	for( var obj in objs)
	{
		Debug.Log("use " + obj.tag + " tag as name " + obj.name);

		if(obj.tag == "TestTag")
		{
			Debug.Log("this tag is TestTag");
		}

		if(obj.CompareTag("TestTag"))
		{
			Debug.Log("obj has extend tag as TestTag");
		}
	}

}

function Update () {

}