using UnityEngine;
using System.Collections;
using UnityEngine.Extensions;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {

	[HideInInspector]
	public InventoryManager Inventory;
	public GameObject Resources_View;
	public GameObject WeaponsAndArmor_View;

	[SerializeField]
	private GameObject _inventoryPanel;
	private GameObject _resourceContainer;	
	private GameObject _openInventoryView;
	private Dictionary<Resource, GameObject> _resourceContainers = new Dictionary<Resource, GameObject>();

	private void Awake() {

		Inventory = InventoryManager.Instance;
		_resourceContainer = Resources.Load<GameObject>("Prefabs/UI/Resource_Container");
	}

	public void OpenInventory() {

		if (_inventoryPanel.activeInHierarchy) {
			_inventoryPanel.SetActive(false);
			ParentUI.Instance.OpenWindow = null;
			return;
		}

		if (ParentUI.Instance.OpenWindow != null) {
			ParentUI.Instance.OpenWindow.SetActive(false);
		}

		_inventoryPanel.SetActive(true);
		ParentUI.Instance.OpenWindow = _inventoryPanel;
		if (_openInventoryView == null) {
			_openInventoryView = Resources_View;
			Resources_View.SetActive(true);
		}
		UpdateResourcesUI();
	}

	private void UpdateResourcesUI() {

		string resourceName;
		GameObject container;
		Transform view0 = Resources_View.transform.GetChild(0);
		Transform view1 = Resources_View.transform.GetChild(1);

		foreach (var kvPair in InventoryManager.Instance.Resources) {
			if (_resourceContainers.TryGetValue(kvPair.Key, out container)) {
				container.GetComponentInChildren<Text>().text =
					string.Format("{0} x {1}", Enum.GetName(typeof(Resource), kvPair.Key), kvPair.Value);
			} else {
				resourceName = Enum.GetName(typeof(Resource), kvPair.Key);
				container = Instantiate(_resourceContainer);
				container.GetComponentsInChildren<Image>()[1].sprite =
					Resources.Load<Sprite>("Images/ResourceIcons/" + resourceName);
				container.GetComponentInChildren<Text>().text =
					string.Format("{0} x {1}", Enum.GetName(typeof(Resource), kvPair.Key), kvPair.Value);
				// If there's an even number of elements, we want to put the element on the left column
				if ((view0.childCount + view1.childCount) % 2 == 0) {
					container.transform.SetParent(view0);
				// Else, we want to put the element on the right column
				} else {
					container.transform.SetParent(view1);
				}
				_resourceContainers.Add(kvPair.Key, container);
			}
		}
	}

	public void OpenResources() {

		if (!Resources_View.activeSelf) {
			Resources_View.SetActive(true);
			_openInventoryView.SetActive(false);
			_openInventoryView = Resources_View; 
		}
	}

	public void OpenWeaponsAndArmor() {

		if (!WeaponsAndArmor_View.activeSelf) {
			WeaponsAndArmor_View.SetActive(true);
			_openInventoryView.SetActive(false);
			_openInventoryView = WeaponsAndArmor_View; 
		}
	}
}
