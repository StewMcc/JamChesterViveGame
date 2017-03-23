using UnityEngine;
using VRTK;

public class Trophy : VRTK_InteractableObject {

	const float kInitialRemainingPercentage = 0.0f;
	const float kMaxRemainingPercentage = 1.0f;
	const float kDirtyErrorMargin = 6;

	public enum CleaningRule {
		kNone, kBrushed, kWiped, kSponged, kSprayedBlue, kSprayedGreen, kSprayedPurple
	}

	[Header("Trophy Settings")]
	[SerializeField]
	CleaningRule[] cleaningRules = new CleaningRule[1];

	[SerializeField]
	Renderer trophyBaseRenderer = null;
	[SerializeField]
	Renderer trophyTopRenderer = null;

	[SerializeField]
	float dirtAmplifier = 2.0f;

	private int numberOfRules_ = 0;

	private float remainingBrushedPercentage_ = kInitialRemainingPercentage;
	private float remainingWipedPercentage_ = kInitialRemainingPercentage;
	private float remainingSpongedPercentage_ = kInitialRemainingPercentage;
	private float remainingSprayedBluePercentage_ = kInitialRemainingPercentage;
	private float remainingSprayedGreenPercentage_ = kInitialRemainingPercentage;
	private float remainingSprayedPurplePercentage_ = kInitialRemainingPercentage;

	private bool isLocked_ = false;

	private float percentageDirty_ = 0.0f;

	private bool hasPlayedCleanedSFX_ = false;

	protected override void Awake() {
		trophyTopRenderer.material.EnableKeyword("_EMISSION");
		trophyBaseRenderer.material.EnableKeyword("_EMISSION");

		foreach (var rule in cleaningRules) {
			switch (rule) {
				case CleaningRule.kBrushed:
					numberOfRules_++;
					remainingBrushedPercentage_ = kMaxRemainingPercentage;
					break;
				case CleaningRule.kWiped:
					numberOfRules_++;
					remainingWipedPercentage_ = kMaxRemainingPercentage;
					break;
				case CleaningRule.kSponged:
					numberOfRules_++;
					remainingSpongedPercentage_ = kMaxRemainingPercentage;
					break;
				case CleaningRule.kSprayedBlue:
					numberOfRules_++;
					remainingSprayedBluePercentage_ = kMaxRemainingPercentage;
					break;
				case CleaningRule.kSprayedGreen:
					numberOfRules_++;
					remainingSprayedGreenPercentage_ = kMaxRemainingPercentage;
					break;
				case CleaningRule.kSprayedPurple:
					numberOfRules_++;
					remainingSprayedPurplePercentage_ = kMaxRemainingPercentage;
					break;
			}
		}
		UpdateEmissiveValue();
	}

	public bool IsClean() {
		//Debug.Log("IsClean: Percentage Dirty:" + percentageDirty_);	
		return percentageDirty_ <= kDirtyErrorMargin;
	}

	public bool IsLocked() {
		return isLocked_;
	}

	public void LockTrophy() {
		GetComponent<Rigidbody>().Sleep();
		GetComponent<Rigidbody>().detectCollisions = false;
		GetComponent<Rigidbody>().isKinematic = true;
		forcedDropped = true;
		isGrabbable = false;
		isUsable = false;
		isLocked_ = true;
	}

	public void CleanTrophy(CleaningRule cleaningType, float amount) {
		if (!isLocked_) {
			switch (cleaningType) {
				case CleaningRule.kBrushed:
					SoundManager.PlaySFX(SoundManager.SFX.kBrushed, 0.2f);
					remainingBrushedPercentage_ -= amount;
					break;
				case CleaningRule.kWiped:
					SoundManager.PlaySFX(SoundManager.SFX.kWiped, 0.2f);
					remainingWipedPercentage_ -= amount;
					break;
				case CleaningRule.kSponged:
					SoundManager.PlaySFX(SoundManager.SFX.kSponged, 0.2f);
					remainingSpongedPercentage_ -= amount;
					break;
				case CleaningRule.kSprayedBlue:

					remainingSprayedBluePercentage_ -= amount;
					break;
				case CleaningRule.kSprayedGreen:

					remainingSprayedGreenPercentage_ -= amount;
					break;
				case CleaningRule.kSprayedPurple:

					remainingSprayedPurplePercentage_ -= amount;
					break;
			}
			UpdateEmissiveValue();

			if (IsClean() && !hasPlayedCleanedSFX_) {
				hasPlayedCleanedSFX_ = true;
				SoundManager.PlaySFX(SoundManager.SFX.kClean);
			}
		}
	}

	private void ClampPercentages() {
		if (remainingBrushedPercentage_ < 0) {
			remainingBrushedPercentage_ = 0;
		}
		if (remainingWipedPercentage_ < 0) {
			remainingWipedPercentage_ = 0;
		}
		if (remainingSpongedPercentage_ < 0) {
			remainingSpongedPercentage_ = 0;
		}
		if (remainingSprayedBluePercentage_ < 0) {
			remainingSprayedBluePercentage_ = 0;
		}
	}

	private void UpdateEmissiveValue() {
		ClampPercentages();
		float colorMod = (remainingBrushedPercentage_ +
			remainingWipedPercentage_ + 
			remainingSpongedPercentage_ + 
			remainingSprayedBluePercentage_ +
			remainingSprayedGreenPercentage_ +
			remainingSprayedPurplePercentage_) 
			/ (kMaxRemainingPercentage * numberOfRules_);
		percentageDirty_ = Mathf.CeilToInt(colorMod * 100.0f);
		// Debug.Log("Percentage Dirty:" + percentageDirty_);	

		Color newEmissive = Color.white * colorMod * dirtAmplifier;
		trophyTopRenderer.material.SetColor("_EmissionColor", newEmissive);
		trophyBaseRenderer.material.SetColor("_EmissionColor", newEmissive);
	}

}
