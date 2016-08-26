using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragBar :
MonoBehaviour,IDragHandler,IBeginDragHandler {
    private RectTransform panel;
	Vector3 offSet;

	// Use this for initialization
	void Awake() {
		panel = transform.parent.GetComponent<RectTransform>();
	}
	/// <summary>
	///面板拖拽效果 
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnBeginDrag (PointerEventData eventData){

		offSet = Input.mousePosition - panel.position;
	}
		

	public void OnDrag (PointerEventData eventData){
		panel.transform.position = Input.mousePosition - offSet;
	  
	}

  

}