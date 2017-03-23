using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TrophyShelfSpawner : MonoBehaviour {

	[SerializeField]
	TrophyShelf shelfPrefab = null;

	[SerializeField]
	GameObject top = null;

	[SerializeField]
	float rowHeight = 0;

	List<TrophyShelf> shelfList_ = new List<TrophyShelf>();

	void Start() {

		shelfPrefab.gameObject.SetActive(false);

		// create a clone from prefab
		shelfList_.Add(Instantiate(shelfPrefab, shelfPrefab.transform.position, shelfPrefab.transform.rotation) as TrophyShelf);

		// attach the top board
		top.gameObject.transform.parent = shelfList_.Last().gameObject.transform;

		// parent it to the spawner
		shelfList_.Last().gameObject.transform.parent = transform;

		// make it active
		shelfList_.Last().gameObject.SetActive(true);

	}

	void Update() {
		if (shelfList_.Last().IsFull()) {
			SpawnNewRow();
		}
	}

	public void SpawnNewRow() {
		// create a clone from prefab
		shelfList_.Add(Instantiate(shelfPrefab, shelfPrefab.transform.position, shelfPrefab.transform.rotation) as TrophyShelf);
		// parent it to the spawner
		shelfList_.Last().gameObject.transform.parent = transform;
		// move it below the current shelf
		shelfList_.Last().gameObject.transform.position -= new Vector3(0, rowHeight, 0);

		// move all the shelves
		foreach (TrophyShelf shelf in shelfList_) {
			shelf.MoveShelf(new Vector3(0, rowHeight, 0));
		}

		// make it active
		shelfList_.Last().gameObject.SetActive(true);

		
	}
}
