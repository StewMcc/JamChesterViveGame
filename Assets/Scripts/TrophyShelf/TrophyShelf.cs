using UnityEngine;

public class TrophyShelf : MonoBehaviour {

	[SerializeField]
	TrophySlot[] slots = new TrophySlot[0];
	
	bool isFull = false;

	int numFilledSlots = 0;

	private float startTime_ = 0;

	private Vector3 startPosition_ = Vector3.zero;

	private Vector3 endPosition_ = Vector3.zero;

	private bool isMoving_ = false;

	private float transitionTime = 3;

	private void Update() {

		if (isMoving_) {
			// transition between the 2 positions.
			transform.position = LittleLot.MathUtil.SmoothLerp(
				startPosition_,
				endPosition_,
				startTime_,
				transitionTime,
				out isMoving_);
		}
	}

	public void SlotFilled() {
		numFilledSlots++;

		if (numFilledSlots >= slots.Length) {
			isFull = true;
		}
	}

	public bool IsFull() {
		return isFull;
	}

	public void MoveShelf(Vector3 newPosition) {
		isMoving_ = true;
		startTime_ = Time.time;
		startPosition_ = transform.position;
		endPosition_ = startPosition_ + newPosition;
	}

}
