using UnityEngine;

public class TrophySpawner : MonoBehaviour {

	[SerializeField]
	Wave[] waves = new Wave[0];

	private LittleLot.SimpleTimer spawnTimer_ = new LittleLot.SimpleTimer();

	private LittleLot.SimpleTimer delayAfterToolsTimer_ = new LittleLot.SimpleTimer();

	private LittleLot.SimpleTimer delayWaveEndTimer_ = new LittleLot.SimpleTimer();

	private bool hasSpawnedInitialItems_ = false;

	private int currentWaveArrayPosition_ = 0;

	private int currentTrophy_ = 0;
	private int numSpawned_ = -1;

	private bool hasFinished_ = false;

	private Wave currentWave_ = null;

	private void Start() {
		currentWave_ = waves[currentWaveArrayPosition_];
		spawnTimer_.SetTimer(currentWave_.respawnTime);
		spawnTimer_.StartTimer();
	}

	private void Update() {
		if (!hasFinished_) {
			delayWaveEndTimer_.Update();
			if (delayWaveEndTimer_.IsFinished()) {
				if (!hasSpawnedInitialItems_) {
					SpawnInitialItems();
				}
				delayAfterToolsTimer_.Update();

				if (delayAfterToolsTimer_.IsFinished()) {
					spawnTimer_.Update();
					if (spawnTimer_.IsFinished()) {
						SpawnTrophy();
					}
				}
			}
		}
	}

	private void SpawnInitialItems() {
		delayAfterToolsTimer_.SetTimer(currentWave_.delayAfterTools);
		delayAfterToolsTimer_.StartTimer();
		hasSpawnedInitialItems_ = true;

		if (currentWave_.hasToolsAndInstructions) {
			SoundManager.PlaySFX(SoundManager.SFX.kSpawn);			
			GameObject newGameObject = Instantiate(currentWave_.instructionLetter, transform);
			newGameObject.SetActive(true);

			newGameObject = Instantiate(currentWave_.newTool, transform);
			newGameObject.SetActive(true);
		}
	}

	private void SpawnTrophy() {
		if (currentWave_.isRandomPrefab) {
			currentTrophy_ = Random.Range(0, currentWave_.trophies.Length);
			GameObject newGameObject = Instantiate(currentWave_.trophies[currentTrophy_], transform);
			newGameObject.SetActive(true);			
			numSpawned_++;
		}
		else {			
			GameObject newGameObject = Instantiate(currentWave_.trophies[currentTrophy_], transform);
			newGameObject.SetActive(true);
			currentTrophy_++;
		}
		SoundManager.PlaySFX(SoundManager.SFX.kSpawn);



		// if none left start the next wave
		if (currentTrophy_ >= currentWave_.trophies.Length || numSpawned_ >= currentWave_.maxNumberOfSpawns) {
			delayWaveEndTimer_.SetTimer(currentWave_.delayAfterWaveEnds);
			delayWaveEndTimer_.StartTimer();

			currentTrophy_ = 0;
			numSpawned_ = -1;
			hasSpawnedInitialItems_ = false;
			currentWaveArrayPosition_++;

			if (currentWaveArrayPosition_ >= waves.Length) {
				hasFinished_ = true;
				Debug.Log("GameFinished");
				return;
			}
			currentWave_ = waves[currentWaveArrayPosition_];
		}
		if (currentWave_.hasChangingRespawnTime) {
			float newRespawnTime = currentWave_.respawnTime - currentWave_.respawnTimeModifier;
			if(newRespawnTime > currentWave_.lowestRespawnTime) {
				currentWave_.respawnTime = newRespawnTime;
			}
		}
		spawnTimer_.SetTimer(currentWave_.respawnTime);
		spawnTimer_.StartTimer();
	}

}
