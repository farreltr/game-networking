using UnityEngine;
using System.Collections;

public class MoveOnStart : MonoBehaviour
{


		private bool flag;
		Vector3 startPosition;
		Vector3 endPosition;
		float duration = 0.4f;
		public string direction;
	
		void Start ()
		{
				startPosition = gameObject.transform.position;
				switch (direction) {
				case "right":
						endPosition = new Vector3 (startPosition.x + TileMap.tileSize, startPosition.y, startPosition.z);
						break;
				case "left":
						endPosition = new Vector3 (startPosition.x - TileMap.tileSize, startPosition.y, startPosition.z);
						break;
				case "up":
						endPosition = new Vector3 (startPosition.x, startPosition.y + TileMap.tileSize, startPosition.z);
						break;
				case "down":
						endPosition = new Vector3 (startPosition.x, startPosition.y - TileMap.tileSize, startPosition.z);
						break;
				default:
						endPosition = startPosition;
						break;
				}				
				flag = true;
	
		}
	
		void Update ()
		{
		
				if (flag && !Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude)) {
						//move the gameobject to the desired position
						gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, endPosition, 1 / (duration * (Vector3.Distance (gameObject.transform.position, endPosition))));
				} else if (flag && Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude)) {
						flag = false;
						this.enabled = false;
						foreach (BoxCollider2D collider in gameObject.GetComponents<BoxCollider2D>()) {
								collider.enabled = true;
						}
						
				}
		}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				// Always send transform (depending on reliability of the network view)
				if (stream.isWriting) {
						Vector3 pos = gameObject.transform.localPosition;
						Quaternion rot = gameObject.transform.localRotation;
						stream.Serialize (ref pos);
						stream.Serialize (ref rot);
				}
		// When receiving, buffer the information
		else {
						// Receive latest state information
						Vector3 pos = Vector3.zero;
						Quaternion rot = Quaternion.identity;
						stream.Serialize (ref pos);
						stream.Serialize (ref rot);
						gameObject.transform.localRotation = rot;
						gameObject.transform.localPosition = pos;
				}
		}
}
