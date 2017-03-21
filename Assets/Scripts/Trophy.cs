using UnityEngine;
using VRTK;

public class Trophy : VRTK_InteractableObject {
	const float kMaxPercentage_ = 1.0f;
	public enum CleaningRule {
		kNone, kBrushed, kWiped, kSponged, kSprayed
	}

	[Header("Trophy Settings")]
	[SerializeField]
	CleaningRule[] cleaningRules = new CleaningRule[4];

	[SerializeField]
	Renderer trophyBaseRenderer = null;
	[SerializeField]
	Renderer trophyTopRenderer = null;

	[SerializeField]
	float dirtyMultiplier = 1.0f;

	private float percentageBrushed_ = kMaxPercentage_;
	private float percentageWiped_ = kMaxPercentage_;
	private float percentageSponged_ = kMaxPercentage_;
	private float percentageSprayed_ = kMaxPercentage_;

	protected override void Awake() {		
		foreach (var rule in cleaningRules) {
			switch (rule) {
				case CleaningRule.kBrushed:
					percentageBrushed_ = 0.0f;
					break;
				case CleaningRule.kWiped:
					percentageWiped_ = 0.0f;
					break;
				case CleaningRule.kSponged:
					percentageSponged_ = 0.0f;
					break;
				case CleaningRule.kSprayed:
					percentageSprayed_ = 0.0f;
					break;
			}
		}
		SetEmissiveValue(1.0f-((percentageBrushed_ + percentageWiped_ + percentageSponged_ + percentageSprayed_) / (kMaxPercentage_ * 4.0f)));
	}
	
	public bool IsClean() {
		return (percentageBrushed_ >= 1) && (percentageWiped_ >= 1) && (percentageSponged_ >= 1) && (percentageSprayed_ >= 1);
	}

	public void CleanTrophy(CleaningRule cleaningType, float amount) {
		
		switch (cleaningType) {
			case CleaningRule.kBrushed:
				percentageBrushed_ += amount;
				break;
			case CleaningRule.kWiped:
				percentageWiped_ += amount;
				break;
			case CleaningRule.kSponged:
				percentageSponged_ += amount;
				break;
			case CleaningRule.kSprayed:
				percentageSprayed_ += amount;
				break;
		}
		ClampPercentages();
		SetEmissiveValue(1.0f - ((percentageBrushed_ + percentageWiped_ + percentageSponged_ + percentageSprayed_) / (kMaxPercentage_ * 4.0f)));
	}

	private void ClampPercentages() {
		if (percentageBrushed_ > kMaxPercentage_) {
			percentageBrushed_ = kMaxPercentage_;
		}
		if (percentageWiped_ > kMaxPercentage_) {
			percentageWiped_ = kMaxPercentage_;
		}
		if (percentageSponged_ > kMaxPercentage_) {
			percentageSponged_ = kMaxPercentage_;
		}
		if (percentageSprayed_ > kMaxPercentage_) {
			percentageSprayed_ = kMaxPercentage_;
		}
	}

	private void SetEmissiveValue(float value) {
		value *= dirtyMultiplier;
		Debug.Log("EmissiveValue: " + value);
		// change texture cleanliness :/		
		Color baseColor = Color.white; //Replace this with whatever you want for your base color at emission level '1'
		Color finalColor = baseColor * value;

		trophyTopRenderer.material.SetColor("_EmissionColor", finalColor);
		trophyBaseRenderer.material.SetColor("_EmissionColor", finalColor);
	}


}
