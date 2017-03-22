using UnityEngine;

public class SoundManager : LittleLot.Singleton<SoundManager> {

	public enum SFX {
		kBrushed, kSponged, kWiped, kSprayed, kSpawn, kCompletedGood,
		kCompletedBad, kDrinking, kGameOver, kTrophyOnFloor, kStamp
	};
	[SerializeField]
	AudioClip[] soundlist = new AudioClip[0];

	[SerializeField]
	AudioSource source = null;
	
	public static void PlaySFX(SFX sfx) {
		//instance.source.PlayOneShot(instance.soundlist[(int)sfx]);
	}

}
