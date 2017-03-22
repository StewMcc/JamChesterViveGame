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

	[SerializeField]
	bool requiresContact = true;

	[SerializeField]
	GameObject useEffect = null;

	private Trophy currentTrophy_ = null;

	private bool isUsing_ = false;

	protected override void Awake() {
		base.Awake();
		if (useEffect) {
			useEffect.SetActive(false);
		}
	}

	protected void OnCollisionEnter(Collision collision) {
		if (requiresContact) {
			if (collision.gameObject.tag == "Trophy") {
				currentTrophy_ = collision.gameObject.GetComponent<Trophy>();
			}
		}
	}

	protected void OnCollisionExit(Collision collision) {
		if (requiresContact) {
			if (collision.gameObject.tag == "Trophy") {
				// remove the refrence if it is the current trophy leaving.
				if (collision.gameObject.GetComponent<Trophy>() == currentTrophy_) {
					currentTrophy_ = null;
				}
			}
		}
	}

	protected void OnTriggerEnter(Collider collider) {
		if (!requiresContact) {
			if (collider.gameObject.tag == "Trophy") {
				currentTrophy_ = collider.gameObject.GetComponent<Trophy>();
			}
		}
	}

	protected void OnTriggerExit(Collider collider) {
		if (!requiresContact) {
			if (collider.gameObject.tag == "Trophy") {
				// remove the refrence if it is the current trophy leaving.
				if (collider.gameObject.GetComponent<Trophy>() == currentTrophy_) {
					currentTrophy_ = null;
				}
			}
		}
	}

	protected override void Update() {
		base.Update();
		if (currentTrophy_ && IsGrabbed()) {
			if (!requiresUseToClean) {
				currentTrophy_.CleanTrophy(cleaningType, cleaningRate * Time.deltaTime);
			}
			else if (isUsing_) {

				currentTrophy_.CleanTrophy(cleaningType, cleaningRate * Time.deltaTime);
			}
		}
	}
	
	public override void StartUsing(GameObject usingObject) {
		base.StartUsing(usingObject);
		if (useEffect) {
			useEffect.SetActive(true);
		}
		isUsing_ = true;
	}

	public override void StopUsing(GameObject previousUsingObject) {
		base.StopUsing(previousUsingObject);
		if (useEffect) {
			useEffect.SetActive(false);
		}
		isUsing_ = false;
	}
}
