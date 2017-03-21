using UnityEngine;
using VRTK;

public class InteractableTool : VRTK_InteractableObject {

	[Header("Tool Settings")]
	[SerializeField]
	Trophy.CleaningRule cleaningType = Trophy.CleaningRule.kNone;

	[SerializeField]
	float cleaningRate = 1.0f;

	[SerializeField]
	bool requiresUseToClean = false;

	private Trophy currentTrophy = null;

	protected override void OnEnable() {
		base.OnEnable();
		if (requiresUseToClean) {
			GetComponent<VRTK_ControllerEvents>().AliasUseOn += UseItem;
		}
	}
	protected override void OnDisable() {
		base.OnDisable();
		if (requiresUseToClean) {
			GetComponent<VRTK_ControllerEvents>().AliasUseOn -= UseItem;
		}
	}

	protected void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Trophy") {
			currentTrophy = collider.gameObject.GetComponent<Trophy>();
		}
	}
	
	protected void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Trophy") {
			currentTrophy = null;
		}
	}

	// clean the trophy if we have one, and the tool doesnt require use to clean the item.
	protected override void Update() {
		base.Update();
		if(!requiresUseToClean && currentTrophy) {
			currentTrophy.CleanTrophy(cleaningType, cleaningRate*Time.deltaTime);
		}

		// CleaningRule specific updates
	}

	// clean the trophy if we have one, when the tool is used.
	protected virtual void UseItem(object sender, ControllerInteractionEventArgs e) {
		Debug.Log("tool used");
		if (currentTrophy) {
			currentTrophy.CleanTrophy(cleaningType, cleaningRate);
		}
		// Clean the trophy specific on use.
	}

}
