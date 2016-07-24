
function OnGUI()
{
	if(GUI.Button(Rect(100,50,200,100),"JavaScript调用C#"))
	{
			//获取C#脚本对象
	 	    var cs = this.GetComponent("CS_test"); 
	    	//脚本C#脚本中方法
	    	cs.CallMe("我来自JavaScript");
	}

}

function CallMe(test : String)
{
	Debug.Log(test);
}