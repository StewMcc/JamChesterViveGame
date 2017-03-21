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

	private Trophy currentTrophy_ = null;

	private bool isUsing_ = false;

	protected void OnCollisionEnter(Collision collider) {
		if (collider.gameObject.tag == "Trophy") {
			currentTrophy_ = collider.gameObject.GetComponent<Trophy>();
		}
	}
	
	protected void OnCollisionExit(Collision collider) {
		if (collider.gameObject.tag == "Trophy") {
			// remove the refrence if it is the current trophy leaving.
			if(collider.gameObject.GetComponent<Trophy>() == currentTrophy_) {
				currentTrophy_ = null;
			}
		}
	}

	protected void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Trophy") {
			currentTrophy_ = collider.gameObject.GetComponent<Trophy>();
		}
	}

	protected void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Trophy") {
			// remove the refrence if it is the current trophy leaving.
			if (collider.gameObject.GetComponent<Trophy>() == currentTrophy_) {
				currentTrophy_ = null;
			}
		}
	}

	protected override void Update() {
		base.Update();
		if(currentTrophy_) {
			if (!requiresUseToClean) {
				currentTrophy_.CleanTrophy(cleaningType, cleaningRate);
			}else if (isUsing_) {
				currentTrophy_.CleanTrophy(cleaningType, cleaningRate);
			}
		}		
	}
		
	public override void StartUsing(GameObject usingObject) {
		base.StartUsing(usingObject);
		isUsing_ = true;
	}

	public override void StopUsing(GameObject previousUsingObject) {
		base.StopUsing(previousUsingObject);
		isUsing_ = false;
	}
}
