using UnityEngine;
using System.Collections;

public class ObjectTypeDelegate : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		ObjectsInfo.On_AllType += DrugType;
		ObjectsInfo.On_AllType += EquipType;
		ObjectsInfo.On_AllType += MatType;	
	}
	void OnDisable(){
		ObjectsInfo.On_AllType -= DrugType;
		ObjectsInfo.On_AllType -= EquipType;
		ObjectsInfo.On_AllType -= MatType;
	}
	
	void DrugType(string type){
		if(type == "Drug"){
			ObjectsInfo._instance.delagateInfo.type = ObjectType.Drug;
		
		}
			
		}

	void  EquipType(string type){
	
		if(type == "Equip"){
			ObjectsInfo._instance.delagateInfo.type= ObjectType.Equip;
		}

	}

	void MatType(string type){

		if(type == "Mat"){
			ObjectsInfo._instance.delagateInfo.type= ObjectType.Mat;
		}

	}

}
