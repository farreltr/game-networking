using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour
{

		public static Stairs stairs;
	
		void Awake ()
		{
				if (stairs == null) {
						DontDestroyOnLoad (stairs);
						stairs = this;
				} else if (stairs != this) {
						Destroy (gameObject);
				}
		}

		// Use this for initialization
		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				GameObject[] stairs = Resources.LoadAll<GameObject> ("Stairs/Prefabs/");
				foreach (GameObject staircase in stairs) {
						GameObject s = (GameObject)Instantiate (staircase);
						s.transform.parent = this.transform;

						SpriteRenderer renderer = s.GetComponent<SpriteRenderer> ();
						renderer.enabled = false;
						renderer.sortingLayerID = 6;
						renderer.sortingOrder = 0;

						BoxCollider2D boxCollider = s.AddComponent<BoxCollider2D> ();
						boxCollider.size = new Vector2 (1.0f, 0.5f);
						boxCollider.center = new Vector2 (0.0f, -0.25f);
						boxCollider.isTrigger = true;

						Rigidbody2D rigidbody = s.AddComponent<Rigidbody2D> ();
						rigidbody.gravityScale = 0.0f;
                        
						if (s.GetComponent<StairsCollider> () == null) {
								s.AddComponent<StairsCollider> ();
						}
				}
		
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
}
