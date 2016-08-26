using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
public class Inventory : MonoBehaviour {
	public static Inventory _instance;
	public List<InvetoryItemGrid> itemGridsList = new List<InvetoryItemGrid>();
	public List<Sprite> itemSpriteList = new List<Sprite>();
	public Sprite ss;
	public GameObject InvetoryItemPrefab;
	public Transform canvas;

	//声明委托 更新所有格子信息
	public  delegate void AllGridsHandler();
	//声明事件
	public static event AllGridsHandler On_AllGrids;
	void Awake(){
		_instance = this;
	}
	void Update(){
		if(Input.GetMouseButtonDown(1)){
			//生成一个Item
			UpdateInventoryGrid(Random.Range(1001,1005));
		}
	}

	public static void UpdateAllGrids(){
		On_AllGrids(); 

	}

	GameObject CreatItemInInventory(int id){
		GameObject item = Instantiate(InvetoryItemPrefab) as GameObject;
		ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
		Image itemImage = item.GetComponentInChildren<Image>();
		Sprite sp = itemSpriteList.Where(m =>m.name ==info.coin_name).FirstOrDefault();
		itemImage.sprite = sp; 
		item.GetComponent<InventoryItem_M>().SetId(id);
		RectTransform rt = item.transform as RectTransform;
//		rt.SetParent(canvas,false);
		rt.SetAsLastSibling();
		return item;
	}

	//格子放进背包里面
	void UpdateInventoryGrid(int id){ print(id);
	 //第一步,先判断背包里面有没该item，有，就直接数量+1
		//第二步，没有,实例化该Item,然后在找一个空格子放进去
		InvetoryItemGrid g = null;
		foreach(InvetoryItemGrid temp in itemGridsList){
			if(temp.id == id){  
				g = temp;
				break;
			}
		}

		if(g != null){
			g.SetNum();
		}
		else
		{
			foreach(InvetoryItemGrid temp in itemGridsList){
				if(temp.id == 0){
				GameObject item = CreatItemInInventory(id);
					item.transform.SetParent(temp.transform,false);
					item.transform.localPosition = Vector3.zero;
					break;
				}
			}
		}
		//更新所有格子信息
		UpdateAllGrids();


	}






















}
