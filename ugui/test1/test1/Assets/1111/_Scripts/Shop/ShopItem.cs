using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShopItem : MonoBehaviour {
	private Text des;
	private Image mySprite;
	[SerializeField]
	private int id;

	void Awake(){
		des = GetComponentInChildren<Text>();
		mySprite =transform.FindChild("ItemSprite").GetComponent<Image>();
	}

	void Start(){
		ShopItemInit(this.id);

	}
	public void ShopItemInit( int id){
		ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
		des.text = UpdateItemInfo(info);
		//修改sprite
		foreach(Sprite sp in Inventory._instance.itemSpriteList){
			if(sp.name == info.coin_name){
				mySprite.sprite = sp;
				break;
			}

		}
	}


	string UpdateItemInfo(ObjectInfo info){
		string str = " ";
		switch (info.type) {
		case ObjectType.Drug:
			str = info.name + "   ";
			if(info.hp != 0){
				str += "加血量:" + info.hp + "\n";
			}
			else
			{
				str += "加蓝量:" + info.mp + "\n";
			}
			str += "购买价格:" + info.price_buy + "\n";
			str += "出售价格:" + info.price_sell;
			break;
			
		default:
			break;
		}
		
		return str;
	}

	//处理购买
	public void Buy(){print(this.id);
		Shop._instance.buyId = this.id;
	}
}
