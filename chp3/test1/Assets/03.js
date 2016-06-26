#pragma strict

private var editUsername : String;

private var editPassword : String;

private var editShow : String;

function Start () {

	editShow = "please insert username and password";
	editUsername = "please insert username";
	editPassword = "please insert password";

}

function OnGUI () {

	GUI.Label(Rect(10, 10, Screen.width, 30), editShow);

	if(GUI.Button(Rect(10, 120, 100, 50), "Log in"))
	{
		editShow = "you insert username is: " + editUsername + ". you insert password is: " + editPassword;
	}
	GUI.Label(Rect(10, 40, 50, 30), "username");
	GUI.Label(Rect(10, 80, 50, 30), "password");

	editUsername = GUI.TextField(Rect(60, 40, 200, 30), editUsername, 15);
	editPassword = GUI.PasswordField(Rect(60, 80, 200, 30), editPassword, "*"[0], 15);

}