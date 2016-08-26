/*

M:这里处理保存数据,更新数据发送信息



 */





using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItem_M : MonoBehaviour
{
	public int id = 255;  //物品Id
	[SerializeField]
	public int num = 1;  //物品个数

	private Text numtext;
	void Awake(){
		numtext = GetComponentInChildren<Text>();
	}
	void OnEnable(){
		Inventory.On_AllGrids += UpdateMyInfoToGrid;

	}
	void OnDisable(){
		Inventory.On_AllGrids -= UpdateMyInfoToGrid;
	}




	public void SetId(int id, int num = 1){
		this.id = id;
		this.num = num;
	}

	public void UpdateMyInfoToGrid(){
		InvetoryItemGrid g =GetComponentInParent<InvetoryItemGrid>();
		g.id = this.id;
		g.num = this.num;
		numtext.text = this.num.ToString();
	}

	public void ClearOldGridInfo(InvetoryItemGrid  grid){

		grid.id = 0;
		grid.num = 0;
	}

	//更新物品数量
	public void SetNum(int num =1){
		this.num += num;
		print(1231);
	}
}
