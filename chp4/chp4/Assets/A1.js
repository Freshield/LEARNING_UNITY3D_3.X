#pragma strict

function Start () {
	
	gameObject.BroadcastMessage ("ReceiveBroadcastMessag", "A1-----BroadcastMessage()");
	//gameObject.SendMessage ("ReceiveSendMessage", "C1-----SendMessage()");
	gameObject.SendMessageUpwards ("ReceiveSendMessag", "A1-----SendMessageUpwards()");
	
}

function ReceiveBroadcastMessag(str : String){
	Debug.Log("A1----Receive" +str);
}
function ReceiveSendMessag(str : String){
	Debug.Log("A1----Receive" +str);
}
function ReceiveSendMessageUpward (str : String){
	Debug.Log("A1----Receive" +str);
}