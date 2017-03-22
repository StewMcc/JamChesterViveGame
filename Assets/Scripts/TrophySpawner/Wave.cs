using UnityEngine;

public class Wave : MonoBehaviour {

	public GameObject[] trophies = null;

	public float respawnTime = 0.0f;

	public float delayAfterWaveEnds = 0.0f;

	[Header("Instruction & Tool Settings")]

	public bool hasToolsAndInstructions = true;

	public GameObject instructionLetter = null;

	public GameObject newTool = null;

	public float delayAfterTools = 0.0f;

	
}
