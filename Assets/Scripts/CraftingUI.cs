using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Extensions;
using System.Collections.Generic;
using System;

public class CraftingUI : MonoBehaviour {

	public GameObject Discovery_View;
	public GameObject Blueprint_View;

	[SerializeField]
	private GameObject _craftingPanel;
	private Transform _blueprintList;
	private Transform _resourceRadial;
	private Transform _discoveryDropZone;
	private Transform _discoveryDisplayPanel;
	private GameObject _radialIconPrefab;
	[SerializeField]
	private Material _lineMaterial;
	[SerializeField]
	private GameObject _craftItemButton;
	private Dictionary<string, GameObject> _blueprintIcons = new Dictionary<string, GameObject>();
	private Dictionary<Resource, GameObject> _radialIcons = new Dictionary<Resource, GameObject>();
	private GameObject _openView;

	public void OpenCraftingWindow() {

		if (_craftingPanel.activeInHierarchy) {
			_craftingPanel.SetActive(false);
			ParentUI.Instance.OpenWindow = null;
			return;
		}

		if (ParentUI.Instance.OpenWindow != null) {
			ParentUI.Instance.OpenWindow.SetActive(false);
		}

		PopulateBlueprintButtons();
		AlignDiscoveryRadial();
		OpenDiscoveryView();

		_craftingPanel.SetActive(true);
		ParentUI.Instance.OpenWindow = _craftingPanel;
	}

	public void OpenDiscoveryView() {

		if (!Discovery_View.activeSelf) {
			Discovery_View.SetActive(true);
			AlignDiscoveryRadial();
			if (_openView != null) {
				_openView.SetActive(false); 
			}
		}
		_openView = Discovery_View;
	}

	public void OpenBlueprintView() {

		if (!Blueprint_View.activeSelf) {
			Blueprint_View.SetActive(true);
			PopulateBlueprintButtons();
			if (_openView != null) {
				_openView.SetActive(false);

			}
		}
		_openView = Blueprint_View;
	}

	public void AttemptDiscovery() {

		if (_discoveryDropZone.childCount == 0) {
			return;
		}

		Dictionary<Resource, int> mats = new Dictionary<Resource, int>();

		for (int i = 0; i < _discoveryDropZone.childCount; i++) {
			Transform child = _discoveryDropZone.GetChild(i);
			Resource r = (Resource)Enum.Parse(typeof(Resource), child.name);
			int q = Int32.Parse(child.GetComponentInChildren<Text>().text);
			mats.Add(r, q);
		}

		List<string> discovered = CraftingManager.Instance.DiscoverBlueprints(mats);
		if (discovered.Count > 0) {
			DisplayDiscoveries(discovered); 
		}

		UpdateInventory(mats);

		List<Transform> children = new List<Transform>();
		foreach (Transform t in _discoveryDropZone) {
			children.Add(t);
		}
		children.ForEach(child => Destroy(child.gameObject));
	}


	public void CancelDiscovery() {

		for (int i = 0; i < _discoveryDropZone.childCount; i++) {
			Transform droppedIcon = _discoveryDropZone.GetChild(i);
			GameObject resourceIcon = _radialIcons[(Resource)Enum.Parse(typeof(Resource), droppedIcon.name)];
			string droppedAmountText = droppedIcon.GetComponentInChildren<Text>().text;
			Text resourceIconText = resourceIcon.GetComponentInChildren<Text>();
			int amount = Int32.Parse(droppedAmountText);
			resourceIconText.text = (Int32.Parse(resourceIconText.text) + amount).ToString();
			Destroy(droppedIcon.gameObject);
		}
	}

	public void CloseDiscoveryPanel() {

		List<Transform> children = new List<Transform>();
		foreach (Transform t in _discoveryDisplayPanel.FindChildRecursive("Content")) {
			if (t.GetSiblingIndex() == 0) {
				t.gameObject.SetActive(false);
			} else {
				children.Add(t);
			}
		}
		children.ForEach(child => Destroy(child.gameObject));
		_discoveryDisplayPanel.gameObject.SetActive(false);
	}

	private void UpdateInventory(Dictionary<Resource, int> mats) {
		
		foreach (var kvPair in mats) {
			InventoryManager.Instance.Resources[kvPair.Key] -= kvPair.Value;
		}
	}

	private void DisplayDiscoveries(List<string> names) {

		Transform content = _discoveryDisplayPanel.FindChildRecursive("Content");
		foreach (string name in names) {
			Sprite image = Resources.Load<Sprite>("Images/BlueprintIcons/" + name);
			GameObject discoveredObject = Instantiate(content.GetChild(0).gameObject);
			discoveredObject.SetActive(true);
			discoveredObject.GetComponentInChildren<Image>().sprite = image;
			discoveredObject.GetComponentInChildren<Text>().text = name;
			discoveredObject.transform.SetParent(content);
		}

		_discoveryDisplayPanel.gameObject.SetActive(true);
	}

	private void AlignDiscoveryRadial() {

		int resourceCount = InventoryManager.Instance.Resources.Count;
		int spacing = (resourceCount > 0) ? 360 / resourceCount : 0;

		foreach (var kvPair in InventoryManager.Instance.Resources) {
			if (!_radialIcons.ContainsKey(kvPair.Key)) {
				AddNewRadialIcon(kvPair);
			}
			_radialIcons[kvPair.Key].transform.FindChildRecursive("Text")
				.GetComponent<Text>().text = kvPair.Value.ToString();
		}
		int rotation = 0;
		foreach (var kvPair in _radialIcons) {
			kvPair.Value.transform.localRotation = Quaternion.Euler((new Vector3(0, 0, -rotation)));
			kvPair.Value.transform.GetChild(0).localRotation = Quaternion.Euler((new Vector3(0, 0, rotation)));
			rotation += spacing;
		}
	}

	private void AddNewRadialIcon(KeyValuePair<Resource, int> kvPair) {

		// Get resources.
		string name = Enum.GetName(typeof(Resource), kvPair.Key);
		Sprite iconSprite = Resources.Load<Sprite>("Images/ResourceIcons/" + name);
		
		// Create icon.
		GameObject icon = Instantiate(_radialIconPrefab);

		// Set values.
		icon.name = name;
		icon.transform.GetChild(0).GetComponent<Image>().sprite = iconSprite;
		icon.transform.FindChildRecursive("Text").GetComponent<Text>().text = kvPair.Value.ToString();
		icon.transform.SetParent(_resourceRadial);
		icon.transform.localPosition = new Vector3(0, 0, 0);

		// Add to icon dictionary.
		_radialIcons.Add(kvPair.Key, icon);
	}

	private void PopulateBlueprintButtons() {

		foreach (string itemName in CraftingManager.Instance.KnownBlueprints) {
			string name = itemName;
			if (!_blueprintIcons.ContainsKey(name)) {
				GameObject button = Instantiate(_craftItemButton);
				_blueprintIcons.Add(name, button);
				button.transform.FindChild("ItemName").GetComponent<Text>().text = name;
				Sprite icon = Resources.Load<Sprite>("Images/BlueprintIcons/" + name);
				button.GetComponent<Image>().sprite = icon;
				button.GetComponent<Button>().onClick.AddListener(delegate {
					CraftingManager.Instance.CraftItem(name);
				});
				button.transform.SetParent(_blueprintList); 
			}
		}
	}

	private void Awake() {

		_craftItemButton = Resources.Load<GameObject>("Prefabs/CraftBlueprint_Button");
		_blueprintList = transform.FindChildRecursive("BlueprintList_Container");
		_resourceRadial = transform.FindChildRecursive("ResourceIconRadial");
		_discoveryDropZone = transform.FindChildRecursive("DropZone_Container");
		_discoveryDisplayPanel = transform.FindChildRecursive("DiscoverDisplay_Panel");
		_radialIconPrefab = Resources.Load<GameObject>("Prefabs/UI/RadialIcon");
	}
}
