using UnityEngine;

public class SoundManager : LittleLot.Singleton<SoundManager> {

	public enum SFX {
		kBrushed, kSponged, kWiped, kSprayed, kSpawn, kClean, kCompletedGood,
		kCompletedBad
	};

	[SerializeField]
	AudioClip[] soundlist = new AudioClip[0];

	[SerializeField]
	AudioSource source = null;

	public static void PlaySFX(SFX sfx,float volumeScale = 1.0f) {		
		instance.source.PlayOneShot(instance.soundlist[(int)sfx], volumeScale);
	}

}
