using UnityEngine;

public class TrophySpawner : MonoBehaviour {

	[SerializeField]
	Wave[] waves = new Wave[0];

	LittleLot.SimpleTimer spawnTimer_ = new LittleLot.SimpleTimer();

	LittleLot.SimpleTimer delayAfterToolsTimer_ = new LittleLot.SimpleTimer();

	LittleLot.SimpleTimer delayWaveEndTimer_ = new LittleLot.SimpleTimer();

	bool hasSpawnedInitialItems = false;

	int currentWaveArrayPosition_ = 0;

	int currentTrophy_ = 0;

	bool hasFinished = false;

	Wave currentWave = null;

	private void Start() {
		currentWave = waves[currentWaveArrayPosition_];
		spawnTimer_.SetTimer(currentWave.respawnTime);
		spawnTimer_.StartTimer();
	}

	private void Update() {
		if (!hasFinished) {
			delayWaveEndTimer_.Update();
			if (delayWaveEndTimer_.IsFinished()) {
				if (!hasSpawnedInitialItems) {
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
		delayAfterToolsTimer_.SetTimer(currentWave.delayAfterTools);
		delayAfterToolsTimer_.StartTimer();
		hasSpawnedInitialItems = true;

		if (currentWave.hasToolsAndInstructions) {
			SoundManager.PlaySFX(SoundManager.SFX.kSpawn);			
			GameObject newGameObject = Instantiate(currentWave.instructionLetter, transform);
			newGameObject.SetActive(true);

			newGameObject = Instantiate(currentWave.newTool, transform);
			newGameObject.SetActive(true);
		}
	}

	private void SpawnTrophy() {
		GameObject newGameObject = Instantiate(currentWave.trophies[currentTrophy_], transform);
		newGameObject.SetActive(true);
		SoundManager.PlaySFX(SoundManager.SFX.kSpawn);
		currentTrophy_++;

		// if none left start the next wave
		if (currentTrophy_ >= currentWave.trophies.Length) {
			delayWaveEndTimer_.SetTimer(currentWave.delayAfterWaveEnds);
			delayWaveEndTimer_.StartTimer();

			currentTrophy_ = 0;
			hasSpawnedInitialItems = false;
			currentWaveArrayPosition_++;

			if (currentWaveArrayPosition_ >= waves.Length) {
				hasFinished = true;
				Debug.Log("GameFinished");
				return;
			}
			currentWave = waves[currentWaveArrayPosition_];
		}

		if (currentWave.hasChangingRespawnTime) {
			float newRespawnTime = currentWave.respawnTime - currentWave.respawnTimeModifier;
			if(newRespawnTime > currentWave.lowestRespawnTime) {
				currentWave.respawnTime = newRespawnTime;
			}
		}
		spawnTimer_.SetTimer(currentWave.respawnTime);
		spawnTimer_.StartTimer();
	}

}
