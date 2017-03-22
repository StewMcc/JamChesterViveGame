using UnityEngine;

public class TrophySlot : MonoBehaviour {

	[SerializeField]
	Transform clampLocked = null;

	TrophyShelf trophyShelf = null;

	Renderer outlineRenderer = null;

	Trophy connectedTrophy = null;

	bool isUsed = false;

	void Start() {
		outlineRenderer = GetComponent<Renderer>();
		trophyShelf = GetComponentInParent<TrophyShelf>();
	}

	public bool IsUsed() {
		return isUsed;
	}

	public void ClampTrophy() {
		// snap it to the clamp point.
		connectedTrophy.transform.rotation = clampLocked.rotation;
		connectedTrophy.transform.position = clampLocked.position;
		connectedTrophy.transform.parent = clampLocked;
	}

	void OnTriggerEnter(Collider collider) {
		// check if it can collide with the slot
		if (!isUsed && collider.tag == "Trophy") {
			// get the trophy
			Trophy trophy = collider.gameObject.GetComponent<Trophy>();

			// make it used
			isUsed = true;

			trophy.LockTrophy();

			connectedTrophy = trophy;

			ClampTrophy();



			// fill a slot in the shelf
			trophyShelf.SlotFilled();

			Color previousColor = outlineRenderer.material.GetColor("_Color");

			if (trophy.IsClean()) {				
				SoundManager.PlaySFX(SoundManager.SFX.kCompletedGood);

				Color newColor = Color.green;
				newColor.a = previousColor.a;
				outlineRenderer.material.SetColor("_Color", newColor);
			}
			else {
				SoundManager.PlaySFX(SoundManager.SFX.kCompletedBad);

				Color newColor = Color.red;
				newColor.a = previousColor.a;
				outlineRenderer.material.SetColor("_Color", newColor);

			}

		}
	}
}
