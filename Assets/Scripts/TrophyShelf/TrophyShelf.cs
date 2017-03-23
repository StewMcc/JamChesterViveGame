using UnityEngine;

public class TrophyShelf : MonoBehaviour {

	[SerializeField]
	TrophySlot[] slots = new TrophySlot[0];

	private bool isFull_ = false;

	private int numFilledSlots_ = 0;

	private float startTime_ = 0;

	private Vector3 startPosition_ = Vector3.zero;

	private Vector3 endPosition_ = Vector3.zero;

	private bool isMoving_ = false;

	private float transitionTime_ = 3;

	private void Update() {

		if (isMoving_) {
			// transition between the 2 positions.			
			transform.position = LittleLot.MathUtil.SmoothLerp(
				startPosition_,
				endPosition_,
				startTime_,
				transitionTime_,
				out isMoving_);
			foreach (TrophySlot slot in slots) {
				slot.ClampTrophy();
			}
		}
	}

	public void SlotFilled() {
		numFilledSlots_++;

		if (numFilledSlots_ >= slots.Length) {
			isFull_ = true;
		}
	}

	public bool IsFull() {
		return isFull_;
	}

	public void MoveShelf(Vector3 newPosition) {
		isMoving_ = true;
		startTime_ = Time.time;
		startPosition_ = transform.position;
		endPosition_ = startPosition_ + newPosition;
	}

}
