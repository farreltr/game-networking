using UnityEngine;
using System.Collections;
using System;

public class ScoreTracker : MonoBehaviour
{
	
		public GameObject pip;
		private PlayerController player;
		private float pipHeight = 48.0f;
		private float pipWidth = 36.0f;

		// Update is called once per frame
		void Start ()
		{

				string colour = this.tag;
				PlayerManager player = GameObject.FindObjectOfType<PlayerManager> ();
				if (player != null) {
						int wins = player.GetWins (colour);
						Vector3 knightPosition = gameObject.transform.position;
						Vector3 initialPosition = new Vector3 (knightPosition.x - 80.0f, knightPosition.y - 105.0f, 1);
						Vector3 position = initialPosition;
						for (int i=0; i<wins; i++) {
								GameObject pipObject = Instantiate (pip, position, pip.transform.rotation) as GameObject;
								pipObject.transform.parent = gameObject.transform;
								if (i == 4) {
										position = new Vector3 (initialPosition.x - pipWidth, initialPosition.y);
								} else {
										position = new Vector3 (position.x, position.y + pipHeight);
								}
				
						}

				}


	
		}
}
