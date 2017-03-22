using UnityEngine;

public class DisableAllChildren : MonoBehaviour {
		
	void Start () {
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);
		}
	}	
}
