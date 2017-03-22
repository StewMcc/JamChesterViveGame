using UnityEngine;

public class TrophySlot : MonoBehaviour {

	[SerializeField]
	Transform clampLocked = null;

	TrophyShelf trophyShelf = null;

	Renderer outlineRenderer = null;

	bool isUsed = false;

	void Start() {
		outlineRenderer = GetComponent<Renderer>();
		trophyShelf = GetComponentInParent<TrophyShelf>();
	}

	public bool IsUsed() {
		return isUsed;
	}

	void OnTriggerEnter(Collider collider) {
		// check if it can collide with the slot
		if (!isUsed && collider.tag == "Trophy") {

			// get the trophy
			Trophy trophy = collider.gameObject.GetComponent<Trophy>();

			// make it used
			isUsed = true;

			trophy.LockTrophy();

			// snap it to the clamp point.
			trophy.transform.rotation = clampLocked.rotation;
			trophy.transform.position = clampLocked.position;

			// fill a slot in the shelf
			trophyShelf.SlotFilled();

			Color previousColor = outlineRenderer.material.GetColor("_Color");

			if (trophy.IsClean()) {
				// Debug.Log("Clean Trophy Slotted");
				SoundManager.PlaySFX(SoundManager.SFX.kCompletedGood);

				Color newColor = Color.green;
				newColor.a = previousColor.a;
				outlineRenderer.material.SetColor("_Color", newColor);
			}
			else {
				// Debug.Log("Dirty Trophy Slotted");
				SoundManager.PlaySFX(SoundManager.SFX.kCompletedBad);

				Color newColor = Color.red;
				newColor.a = previousColor.a;
				outlineRenderer.material.SetColor("_Color", newColor);

			}

		}
	}
}
