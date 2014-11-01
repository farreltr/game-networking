using UnityEngine;
using System.Collections;

public class Knights : MonoBehaviour
{

		private static float SPEED = 40.0f;
		private PlayerController[] knightArray;

		public static Knights knights;
	
		void Awake ()
		{
				if (knights == null) {
						DontDestroyOnLoad (knights);
						knights = this;
				} else if (knights != this) {
						Destroy (gameObject);
				}
		}

	
		// Use this for initialization
		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				GameObject[] knightPrefabs = Resources.LoadAll<GameObject> ("Knights/Prefabs/");
				int i = 0;
				knightArray = new PlayerController[knightPrefabs.Length];
				foreach (GameObject knight in knightPrefabs) {
						GameObject s = (GameObject)Instantiate (knight);
						s.transform.parent = this.transform;
						BoxCollider2D collider = s.AddComponent<BoxCollider2D> ();
						collider.name = s.name;
						Rigidbody2D rb = s.GetComponent<Rigidbody2D> ();
						if (rb == null) {
								rb = s.AddComponent<Rigidbody2D> ();
						}
						
						rb.gravityScale = 0.0f;
						rb.fixedAngle = true;
						rb.angularDrag = 0.0f;
						rb.isKinematic = false;
						s.GetComponent<SpriteRenderer> ().sortingLayerName = "Player";
						PlayerController playerController = s.GetComponent<PlayerController> () == null ? 
							s.AddComponent<PlayerController> () : 
								s.GetComponent<PlayerController> ();
						PlayerController.SPEED = SPEED;
						AudioSource audio = s.AddComponent<AudioSource> ();
						audio.clip = Resources.Load<AudioClip> ("Audio/pop_sound");
						audio.playOnAwake = false;
						knightArray [i] = playerController;
						i++;
				}
	
		}


		public void ShiftKnightsRight (int x, int y)
		{
				foreach (PlayerController knight in knightArray) {
						knight.ShiftRight (x, y);
				}
		}


	
		public void ShiftKnightsDown (int x, int y)
		{
				foreach (PlayerController knight in knightArray) {
						knight.ShiftDown (x, y);
				}
		}
	
		public void ShiftKnightsUp (int x, int y)
		{
				foreach (PlayerController knight in knightArray) {
						knight.ShiftUp (x, y);
				}
		}
	
		public void ShiftKnightsLeft (int x, int y)
		{
				foreach (PlayerController knight in knightArray) {
						knight.ShiftLeft (x, y);
				}
		}
}
