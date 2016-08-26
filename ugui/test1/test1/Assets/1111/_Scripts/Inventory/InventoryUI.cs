using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour {
	public Text coinLabel;


	private bool isPanelShow = false;
	private GameObject Inventory_BG;

	void Awake(){
		Inventory_BG = transform.Find("Inventory_bg").gameObject;
	
		coinLabel.text = "524";
	}


	//面板的开启与关闭
	public void InventoryPanelTransformState(){
		if( ! isPanelShow){
			Inventory_BG.gameObject.SetActive(true);
			isPanelShow = true;
		}
		else
		{
			Inventory_BG.gameObject.SetActive(false);
			isPanelShow = false;
		}
	}

}//Class End
