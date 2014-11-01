using UnityEngine;
using System.Collections;

public class MoveDownOnStart : MonoBehaviour
{

		private Vector3 startPosition;
		private Vector3 endPosition;
		public float duration = 4.0f;

		void Start ()
		{

				//-60
				startPosition = transform.position;
				endPosition = new Vector3 (startPosition.x, startPosition.y - 60.0f, startPosition.z);
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.position = Vector3.Lerp (startPosition, endPosition, Time.time / duration);
		}
}
