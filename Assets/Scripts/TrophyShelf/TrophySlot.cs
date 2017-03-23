using UnityEngine;

public class TrophySlot : MonoBehaviour {

	[SerializeField]
	Transform clampLocked = null;

	TrophyShelf trophyShelf_ = null;

	Renderer outlineRenderer_ = null;

	Trophy connectedTrophy_ = null;

	bool isUsed = false;

	void Start() {
		outlineRenderer_ = GetComponent<Renderer>();
		trophyShelf_ = GetComponentInParent<TrophyShelf>();
	}

	public bool IsUsed() {
		return isUsed;
	}

	public void ClampTrophy() {
		if (connectedTrophy_) {
			// snap it to the clamp point.
			connectedTrophy_.transform.rotation = clampLocked.rotation;
			connectedTrophy_.transform.position = clampLocked.position;
			connectedTrophy_.transform.parent = clampLocked;
		}
	}

	void OnTriggerEnter(Collider collider) {
		// check if it can collide with the slot
		if (!isUsed && collider.tag == "Trophy") {
			// get the trophy
			Trophy trophy = collider.gameObject.GetComponent<Trophy>();

			// make it used
			isUsed = true;

			trophy.LockTrophy();

			connectedTrophy_ = trophy;

			ClampTrophy();

			// fill a slot in the shelf
			trophyShelf_.SlotFilled();

			Color previousColor = outlineRenderer_.material.GetColor("_Color");

			if (trophy.IsClean()) {				
				SoundManager.PlaySFX(SoundManager.SFX.kCompletedGood);

				Color newColor = Color.green;
				newColor.a = previousColor.a;
				outlineRenderer_.material.SetColor("_Color", newColor);
			}
			else {
				SoundManager.PlaySFX(SoundManager.SFX.kCompletedBad);

				Color newColor = Color.red;
				newColor.a = previousColor.a;
				outlineRenderer_.material.SetColor("_Color", newColor);

			}

		}
	}
}
