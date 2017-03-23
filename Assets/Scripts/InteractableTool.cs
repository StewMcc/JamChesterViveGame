using UnityEngine;
using VRTK;

public class InteractableTool : VRTK_InteractableObject {

	[Header("Tool Settings")]
	[SerializeField]
	Trophy.CleaningRule cleaningType = Trophy.CleaningRule.kNone;

	[SerializeField]
	float cleaningRate = 10.0f;

	[SerializeField]
	bool requiresUseToClean = false;

	[SerializeField]
	bool requiresContact = true;

	[SerializeField]
	ParticleSystem useEffect = null;

	private Trophy currentTrophy_ = null;

	private LittleLot.SimpleTimer delayClean_ = new LittleLot.SimpleTimer();

	protected override void Awake() {
		base.Awake();		
		delayClean_.SetTimer(0.2f);
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
		delayClean_.Update();
		if (currentTrophy_ && IsGrabbed()) {
			if (delayClean_.IsFinished()) {
				if (!requiresUseToClean) {
					delayClean_.StartTimer();
					currentTrophy_.CleanTrophy(cleaningType, cleaningRate * Time.deltaTime);
					PlayToolSfx();
				}
				else if (IsUsing()) {
					delayClean_.StartTimer();
					currentTrophy_.CleanTrophy(cleaningType, cleaningRate * Time.deltaTime);					
				}
			}
		}		
	}
	
	public override void StartUsing(GameObject usingObject) {
		base.StartUsing(usingObject);
		if (useEffect) {
			useEffect.Stop(true);
			useEffect.Play(true);
		}
		if (requiresUseToClean) {
			PlayToolSfx();
		}
	}	


	private void PlayToolSfx() {
		switch (cleaningType) {
			case Trophy.CleaningRule.kBrushed:
				SoundManager.PlaySFX(SoundManager.SFX.kBrushed, 0.2f);				
				break;
			case Trophy.CleaningRule.kWiped:
				SoundManager.PlaySFX(SoundManager.SFX.kWiped, 0.2f);				
				break;
			case Trophy.CleaningRule.kSponged:
				SoundManager.PlaySFX(SoundManager.SFX.kSponged, 0.2f);				
				break;
			case Trophy.CleaningRule.kSprayedBlue:
				SoundManager.PlaySFX(SoundManager.SFX.kSprayed, 0.2f);
				break;
			case Trophy.CleaningRule.kSprayedGreen:
				SoundManager.PlaySFX(SoundManager.SFX.kSprayed, 0.2f);
				break;
			case Trophy.CleaningRule.kSprayedPurple:
				SoundManager.PlaySFX(SoundManager.SFX.kSprayed, 0.2f);
				break;
		}
	}
}
