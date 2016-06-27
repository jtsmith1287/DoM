using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class DraggableResourceIcon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

	private GameObject _dragged;

	public void OnBeginDrag(PointerEventData eventData) {

		Text amount = eventData.pointerDrag.transform.GetChild(0).GetComponentInChildren<Text>();
		int x = Int32.Parse(amount.text);
		if (x <= 0) {
			return;
		}
		amount.text = (x - 1).ToString();

		Destroy(_dragged);
		_dragged = Instantiate(gameObject.transform.GetChild(0).gameObject);
		_dragged.transform.SetParent(transform);
		_dragged.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		_dragged.transform.position = eventData.position;
		_dragged.GetComponentInChildren<Text>().text = "1";
		_dragged.name = eventData.pointerDrag.name;
		

		CanvasGroup cg = _dragged.AddComponent<CanvasGroup>();
		cg.blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData) {

		if (_dragged != null) {
			_dragged.transform.position = eventData.position; 
		}
	}

	public void OnEndDrag(PointerEventData eventData) {

		GameObject g = eventData.pointerCurrentRaycast.gameObject;
		if (g && g.CompareTag("DiscoveryDropZone")) {
			Transform container = g.transform.GetChild(1);
			Transform icon = container.FindChild(_dragged.name);
			if (icon != null) {
				Text amount = icon.GetChild(0).GetComponentInChildren<Text>();
				int x = Int32.Parse(amount.text);
				amount.text = (x + 1).ToString();
				Destroy(_dragged);
			}else {
				_dragged.transform.SetParent(container);
			}
		} else {
			Text amount = eventData.pointerDrag.transform.GetChild(0).GetComponentInChildren<Text>();
			int x = Int32.Parse(amount.text);
			amount.text = (x + 1).ToString();
			Destroy(_dragged);
		}
		_dragged = null;
	}
}
