#pragma strict

private var select : int;

private var barResource : String[];

private var selectToggle0 : boolean;

private var selectToggle1 : boolean;

function Start () {

	select = 0;
	barResource = ["first","second","third","forth"];

	selectToggle0 = false;
	selectToggle1 = false;

}

function OnGUI () {

	var oldSelect = select;

	select = GUI.Toolbar(Rect(10, 10, barResource.length * 100, 30), select, barResource);

	if(oldSelect != select)
	{
		selectToggle0 = false;
		selectToggle1 = false;
	}

	switch(select)
	{
		case 0:
			selectToggle0 = GUI.Toggle(Rect(10, 50, 150, 30), selectToggle0, "firstChoice----1");
			selectToggle1 = GUI.Toggle(Rect(10, 80, 150, 30), selectToggle1, "firstChoice----2");
			break;

		case 1:
			selectToggle0 = GUI.Toggle(Rect(10, 50, 150, 30), selectToggle0, "secondChoice----1");
			selectToggle1 = GUI.Toggle(Rect(10, 80, 150, 30), selectToggle1, "secondChoice----2");
			break;

		case 2:
			selectToggle0 = GUI.Toggle(Rect(10, 50, 150, 30), selectToggle0, "thirdChoice----1");
			selectToggle1 = GUI.Toggle(Rect(10, 80, 150, 30), selectToggle1, "thirdChoice----2");
			break;

		case 3:
			selectToggle0 = GUI.Toggle(Rect(10, 50, 150, 30), selectToggle0, "forthChoice----1");
			selectToggle1 = GUI.Toggle(Rect(10, 80, 150, 30), selectToggle1, "forthChoice----2");
			break;
	}

}