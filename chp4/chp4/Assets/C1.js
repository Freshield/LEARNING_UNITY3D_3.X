#pragma strict

function Start () {
	
	//gameObject.BroadcastMessage ("ReceiveBroadcastMessage", "C1-----BroadcastMessage()");
	//gameObject.SendMessage ("ReceiveSendMessage", "C1-----SendMessage()");
	gameObject.SendMessageUpwards ("ReceiveSendMessag", "C1-----SendMessageUpwards()");
	
}

function ReceiveBroadcastMessag(str : String){
	Debug.Log("C1----Receive" +str);
}
function ReceiveSendMessag(str : String){
	Debug.Log("C1----Receive" +str);
}
function ReceiveSendMessageUpward (str : String){
	Debug.Log("C1----Receive" +str);
}