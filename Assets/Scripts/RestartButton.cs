using UnityEngine;
using UnityEngine.SceneManagement;

using VRTK;
using VRTK.UnityEventHelper;

public class RestartButton : MonoBehaviour {	

	private VRTK_Button_UnityEvents buttonEvents_;

	private bool isRestarting_ = false;

	private void Start() {
		buttonEvents_ = GetComponent<VRTK_Button_UnityEvents>();
		if (buttonEvents_ == null) {
			buttonEvents_ = gameObject.AddComponent<VRTK_Button_UnityEvents>();
		}
		buttonEvents_.OnPushed.AddListener(HandleButtonPushed);
	}

	private void HandleButtonPushed(object sender, Control3DEventArgs e) {
		if (!isRestarting_) {
			isRestarting_ = true;
			SceneManager.LoadSceneAsync("Intro");
		}
	}
}
