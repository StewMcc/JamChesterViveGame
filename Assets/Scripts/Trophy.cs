using UnityEngine;
using VRTK;

public class Trophy : VRTK_InteractableObject {

	const float kInitialRemainingPercentage = 0.0f;
	const float kMaxRemainingPercentage = 1.0f;

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

	private bool isLocked = false;

	private float percentageDirty = 0.0f;

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
			}
		}
		UpdateEmissiveValue();
	}

	public bool IsClean() {
		//Debug.Log("IsClean: Percentage Dirty:" + percentageDirty);	
		return percentageDirty <= 0;
	}

	public bool IsLocked() {
		return isLocked;
	}
	public void LockTrophy() {
		GetComponent<Rigidbody>().isKinematic = true;
		forcedDropped = true;
		isGrabbable = false;
		isUsable = false;
		isLocked = true;
	}

	public void CleanTrophy(CleaningRule cleaningType, float amount) {
		if (!isLocked) {
			switch (cleaningType) {
				case CleaningRule.kBrushed:
					SoundManager.PlaySFX(SoundManager.SFX.kBrushed);
					remainingBrushedPercentage_ -= amount;
					break;
				case CleaningRule.kWiped:
					SoundManager.PlaySFX(SoundManager.SFX.kWiped);
					remainingWipedPercentage_ -= amount;
					break;
				case CleaningRule.kSponged:
					SoundManager.PlaySFX(SoundManager.SFX.kSponged);
					remainingSpongedPercentage_ -= amount;
					break;
				case CleaningRule.kSprayedBlue:
					SoundManager.PlaySFX(SoundManager.SFX.kSprayed);
					remainingSprayedBluePercentage_ -= amount;
					break;
			}
			UpdateEmissiveValue();
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
		float colorMod = (remainingBrushedPercentage_ + remainingWipedPercentage_ + remainingSpongedPercentage_ + remainingSprayedBluePercentage_) / (kMaxRemainingPercentage * numberOfRules_);
		percentageDirty = Mathf.CeilToInt(colorMod * 100.0f);
		// Debug.Log("Percentage Dirty:" + percentageDirty);	

		Color newEmissive = Color.white * colorMod * dirtAmplifier;
		trophyTopRenderer.material.SetColor("_EmissionColor", newEmissive);
		trophyBaseRenderer.material.SetColor("_EmissionColor", newEmissive);
	}

}
