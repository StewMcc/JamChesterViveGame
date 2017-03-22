using UnityEngine;

namespace LittleLot {
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

		private static T instance_;

		public static T instance {
			get {
				if (instance_ == null) {
					instance_ = (T)FindObjectOfType(typeof(T));
				}
				return instance_;
			}
		}
	}
}
