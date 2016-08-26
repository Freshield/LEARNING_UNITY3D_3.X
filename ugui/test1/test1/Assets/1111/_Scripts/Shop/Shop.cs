using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
	public static Shop _instance;
	public GameObject numDialog;

	public int buyId = 0;
	void Awake(){
		_instance = this;
		print(buyId);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HideNumDialog(){
		numDialog.SetActive(false);
	}

	public void ShowNumDialog(){
		numDialog.SetActive(true);

	}

	//处理购买,购买相应的item,背包减少相应的金钱
//	public void BuyItem(){
//		switch (buyId) {
//		case 1001:
//
//				default:
//						break;
//		}
//	}
}
