using UnityEngine;

namespace LittleLot {
	class SimpleTimer {
		private float timeRemaining_ = 0.0f;
		private float duration_ = 0.0f;

		private bool isFinished_ = false;

		public bool IsFinished() {
			return isFinished_;
		}

		public void SetTimer(float duration) {
			duration_ = duration;
		}

		public void StartTimer() {
			isFinished_ = false;
			timeRemaining_ = duration_;
		}
		public float TimeRemaining() {
			return timeRemaining_;
		}
		public float Duration() {
			return duration_;
		}

		public void Update() {
			timeRemaining_ -= Time.deltaTime;
			if (timeRemaining_ <= 0) {
				isFinished_ = true;
			}
		}

	}
}