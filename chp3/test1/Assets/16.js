#pragma strict

var winArrayList = new ArrayList();

var icon : Texture;

function Start () {
	winArrayList.Add(Rect(winArrayList.Count * 100, 50, 150, 100));
}

function OnGUI () {

	var count = winArrayList.Count;
	for(var i = 0; i < count; i++)
	{
		winArrayList[i] = GUILayout.Window(i,winArrayList[i],AddWindow,"whindow id: " + i);
	}

}

function AddWindow(windowID:int)
{
	GUILayout.BeginHorizontal();
	GUILayout.Label(icon,GUILayout.Width(50),GUILayout.Height(50));
	GUILayout.Label("this is a new window");
	GUILayout.EndHorizontal();

	GUILayout.BeginHorizontal();
	if(GUILayout.Button("add new window"))
	{
		winArrayList.Add(Rect(winArrayList.Count * 100, 50, 150, 100));
	}

	if(GUILayout.Button("remove this window"))
	{
		winArrayList.RemoveAt(windowID);
	}

	GUILayout.EndHorizontal();
	GUI.DragWindow(Rect(0, 0, Screen.width, Screen.height));
}