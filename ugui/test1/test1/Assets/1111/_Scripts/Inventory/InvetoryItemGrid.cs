using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InvetoryItemGrid : MonoBehaviour {
	public int id,num;



	public void SetId(int id,int num){
		this.id = id;
	}

	public void SetNum(int num =1){
		print(this.gameObject.name);
		GetComponentInChildren<InventoryItem_M>().SetNum(num);
	}




}//Class End
