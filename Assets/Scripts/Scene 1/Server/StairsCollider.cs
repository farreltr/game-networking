using UnityEngine;
using System.Collections;

public class StairsCollider : MonoBehaviour
{

		public GameController controller;

		// Use this for initialization
		void Start ()
		{
				controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTriggerEnter2D (Collider2D collider)
		{
				GameObject colliderObject = collider.gameObject;
				string myName = GetFormattedName (gameObject);
				PlayerController playerController = colliderObject.transform.GetComponent<PlayerController> ();
				if (playerController != null) {
						if (myName == playerController.GetName ()) {
								playerController.isWinner = true;
								controller.GameOver ();
						} else if (myName == playerController.GetRespawnName () && playerController.isRespawn) {
								//do nothing
						} else {
								playerController.ChangeDirection ();
						}
				}
					
		}

		private static string GetFormattedName (GameObject o)
		{
				return o.name.Replace ("(Clone)", "");
		}

}
