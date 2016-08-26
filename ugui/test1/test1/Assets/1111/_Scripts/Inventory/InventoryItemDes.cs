using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventoryItemDes : MonoBehaviour {
	public static InventoryItemDes _instance;
	private Text des;
 
	void Awake(){
		des = GetComponentInChildren<Text>();
		_instance  = this;
		transform.GetChild(0).gameObject.SetActive(false);
	}

	public  Transform ShowItemDes(int id){
		ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);

		des.text = UpdateDes(info);
		return this.transform;
	}

	string UpdateDes(ObjectInfo info){
		string str = " ";
		switch (info.type) {
		case ObjectType.Drug:
			str = info.name + "\n";
			str += "加血量：" + info.hp + "\n";
			str += "加蓝量：" + info.mp + "\n";
			str += "购买价格：" + info.price_buy + "\n";
			str += "出售价格：" + info.price_sell;
			break;
		
				default:
						break;
		}

		return str;
	}


	public void ShowDes(){
		transform.GetChild(0).gameObject.SetActive(true);
	}

	public void HideDes(){
		transform.GetChild(0).gameObject.SetActive(false);
	}
}
