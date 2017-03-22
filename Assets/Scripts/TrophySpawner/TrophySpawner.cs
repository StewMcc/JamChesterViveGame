﻿using UnityEngine;

public class TrophySpawner : MonoBehaviour {

	[SerializeField]
	Wave[] waves = new Wave[0];

	LittleLot.SimpleTimer spawnTimer_ = new LittleLot.SimpleTimer();

	LittleLot.SimpleTimer delayAfterToolsTimer_ = new LittleLot.SimpleTimer();

	LittleLot.SimpleTimer delayWaveEndTimer_ = new LittleLot.SimpleTimer();

	bool hasSpawnedInitialItems = false;

	int currentWave_ = 0;

	int currentTrophy_ = 0;

	bool hasFinished = false;

	private void Start() {
		spawnTimer_.SetTimer(waves[currentWave_].respawnTime);
		spawnTimer_.StartTimer();
	}

	private void Update() {
		if (!hasFinished) {
			delayWaveEndTimer_.Update();
			if (delayWaveEndTimer_.IsFinished()) {
				delayAfterToolsTimer_.Update();

				if (!hasSpawnedInitialItems) {
					SpawnInitialItems();
				}

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
		hasSpawnedInitialItems = true;
		if (waves[currentWave_].hasToolsAndInstructions) {
			delayAfterToolsTimer_.SetTimer(waves[currentWave_].delayAfterTools);
			delayAfterToolsTimer_.StartTimer();

			// Instantiate.
			Debug.Log("SpawnItems");
			GameObject newGameObject = Instantiate(waves[currentWave_].instructionLetter, transform);
			newGameObject.SetActive(true);

			newGameObject = Instantiate(waves[currentWave_].newTool, transform);
			newGameObject.SetActive(true);
		}
	}

	private void SpawnTrophy() {
		// Instantiate.
		Debug.Log("SpawnTrophy");
		GameObject newGameObject = Instantiate(waves[currentWave_].trophies[currentTrophy_], transform);
		newGameObject.SetActive(true);
		SoundManager.PlaySFX(SoundManager.SFX.kSpawn);
		currentTrophy_++;
		// if none left start the next wave
		if (currentTrophy_ >= waves[currentWave_].trophies.Length) {
			Debug.Log("DelayedWave");
			delayWaveEndTimer_.SetTimer(waves[currentWave_].delayAfterWaveEnds);
			delayWaveEndTimer_.StartTimer();

			currentTrophy_ = 0;
			hasSpawnedInitialItems = false;
			currentWave_++;

			if (currentWave_ >= waves.Length) {
				hasFinished = true;
				Debug.Log("GameFinished");
				return;
			}
		}
		spawnTimer_.SetTimer(waves[currentWave_].respawnTime);
		spawnTimer_.StartTimer();
	}

}
