/*

C:处理代码逻辑


 */





using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItem_C : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	private GameObject m_DraggingIcon;
	private RectTransform m_DraggingPlane;
	private GameObject item = null;

	public void OnBeginDrag(PointerEventData eventData)
	{
		var canvas = FindInParents<Canvas>(gameObject);
		if (canvas == null)
			return;
		//得到本身的格子；清空Id


		item = transform.FindChild("image").gameObject;
		// We have clicked something that can be dragged.
		// What we want to do is create an icon for this.
		m_DraggingIcon = new GameObject("icon");

		m_DraggingIcon.transform.SetParent (canvas.transform, false);
		m_DraggingIcon.transform.SetAsLastSibling();
		
		var image = m_DraggingIcon.AddComponent<Image>();
		// The icon will be under the cursor.
		// We want it to be ignored by the event system.
		CanvasGroup group = m_DraggingIcon.AddComponent<CanvasGroup>();
		group.blocksRaycasts = false;

		image.sprite = item.GetComponent<Image>().sprite; print(item.name);
		image.SetNativeSize();
	    m_DraggingPlane = canvas.transform as RectTransform;
		
		SetDraggedPosition(eventData);
		//隐藏item
		item.SetActive(false);
	}

	public void OnDrag(PointerEventData data)
	{
		if (m_DraggingIcon != null)
			SetDraggedPosition(data);
	}

	private void SetDraggedPosition(PointerEventData data)
	{
		var rt = m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;

		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GameObject go = eventData.pointerEnter;
		if (go != null){
			if(go.tag != Tags.InventoryItemGrid){
				item.SetActive(true);
			}
			else
			{
				//作为go格子的子物体
				transform.SetParent(go.transform,false);
				item.SetActive(true);
			}
			//背包数据更新
//			go.GetComponent<InvetoryItemGrid>().SetId(this.id,this.num);
		}
		item.SetActive(true);	
		Destroy(m_DraggingIcon);
	    
		    
	}

	static public T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null) return null;
		var comp = go.GetComponent<T>();

		if (comp != null)
			return comp;
		
		Transform t = go.transform.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}
}
