using UnityEngine;
using System.Collections;

public class CS_test : MonoBehaviour {


	void OnGUI()
	{

		if(GUI.Button(new Rect(100,170,200,100),"C#调用JavaScript"))
		{
			//获取JavaScript脚本对象
			JS_test jsScript = (JS_test)GetComponent("JS_test");
			//调用JavaScript脚本中方法
			jsScript.CallMe("我来自C#");
		}

	}

	public void CallMe(string test)
	{
		Debug.Log(test);
	}
}
