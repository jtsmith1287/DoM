using UnityEngine;
using System.Collections;

public class ParentUI : MonoBehaviour {

	public static ParentUI Instance;

	public GameObject OpenWindow;
	public InventoryUI InventoryUI;
	public CraftingUI CraftingUI;

	private void Awake() {

		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}

		InventoryUI = GetComponent<InventoryUI>();
		CraftingUI = GetComponent<CraftingUI>();
	}

	private void Update() {

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (Instance.OpenWindow != null) {
				Instance.OpenWindow.SetActive(false);
			}
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			InventoryUI.OpenInventory();
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			CraftingUI.OpenCraftingWindow();
		}
	}
}
