using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsInfo : MonoBehaviour {
	public static ObjectsInfo _instance; //单例模式
	public TextAsset objectsInfoListText; //获取物品属性文本
	private Dictionary<int,ObjectInfo> objectDic= new Dictionary<int, ObjectInfo>(); //使用字典存放物品属性
	//使用委托来对物品的类型进行判断,这样方便扩展
	//声明委托
	public delegate void AllTypeHandler( string type);
	//声明事件
	public static event AllTypeHandler On_AllType;
	//方便外部委托方法赋值 详见ObjectTypeDelegate 脚本
	public  ObjectInfo delagateInfo = new ObjectInfo();

	// Use this for initialization
	void Awake () {
		_instance = this;
		ReadInfo();
//		ObjectInfo inf = GetObjectInfoById(2003);
//		print(inf.name);
	}

	//让外部通过Id获取对应的物品属性
	public ObjectInfo GetObjectInfoById(int id){
		ObjectInfo info = null;
		objectDic.TryGetValue(id,out info);

		return info;
	}
	//读取物品属性  放到字典里面  可以通过ID获取到相应属性
	void ReadInfo(){
		string text = objectsInfoListText.text;
		string[] strArray = text.Split('\n');

		foreach(string temp in strArray){
			ObjectInfo info = new ObjectInfo();
			string[] strPro = temp.Split(',');
			info.id = int.Parse(strPro[0]);
			info.name = strPro[1];
			info.coin_name = strPro[2];
			//调用委托
		    On_AllType(strPro[3]);
			info.type = delagateInfo.type;
			if(info.type == ObjectType.Drug){
				info.hp = int.Parse( strPro[4]);
				info.mp =  int.Parse(strPro[5]);
				info.price_buy =  int.Parse(strPro[6]);
			    info.price_sell =  int.Parse(strPro[7]);
			}
			else if(info.type == ObjectType.Equip){
				info.speed =  int.Parse(strPro[4]);
				info.attack =   int.Parse(strPro[5]);
				info.def =   int.Parse(strPro[6]);
				info.price_buy =   int.Parse(strPro[7]);
				info.price_sell =   int.Parse(strPro[8]);
			}
			else if(info.type == ObjectType.Mat){
				info.price_buy =  int.Parse(strPro[4]);
				info.price_sell =  int.Parse(strPro[5]);
			}
			objectDic.Add(info.id,info);

		}
	}// ReadInfo End


}//Class End


/* 0	1	         2	                 3	      4	    5	         6	              7	         8
	ID	name	coin_name  Drug   加血量	加蓝量	price_buy	 price_sell	

		***	**	**	      Equip	加速度	加攻击	加防御	   price_buy   price_sell

		**	**	**	      Mat  price_buy  price_sell

*/
public enum ObjectType{
	Null,
	Drug,
	Equip,
	Mat
}
public class ObjectInfo{
	public int id;
	public string name;
	public string coin_name;
	public ObjectType type;
	public int hp;
	public int mp;
	public int attack;
	public int speed;
	public int def;
	public int price_buy;
	public int price_sell;
}