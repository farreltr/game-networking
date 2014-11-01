using UnityEngine;
using System.Collections;

public class InGameRestart : MonoBehaviour
{

		float x = 7.5f * Screen.width / 10;
		float y = 8 * Screen.height / 10;

		// Use this for initialization
		void OnGUI ()
		{
				bool restartButton = GUI.Button (new Rect (x, y, 100, 20), "RESTART");
				if (restartButton) {
						PlayerManager manager = GameObject.FindObjectOfType<PlayerManager> ();
						manager.showCharSelect = true;
						manager.Reset ();
						CleanUpObjects ();
						Application.LoadLevel ("Character Select");
				}

	
		}

		void CleanUpObjects ()
		{
				GameController controller = GameObject.FindObjectOfType<GameController> ();
				foreach (Transform child in controller.transform) {
						GameObject.Destroy (child.gameObject);
				}
				Destroy (controller.gameObject);
		}
}
