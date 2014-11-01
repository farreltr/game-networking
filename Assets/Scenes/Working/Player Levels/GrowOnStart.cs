using UnityEngine;
using System.Collections;

public class GrowOnStart : MonoBehaviour
{

		private float interval = 0.0001f;
		private Vector3 originalScale = new Vector3 (100.0f, 100.0f, 1.0f);
		private Vector3 targetScale = new Vector3 (150.0f, 150.0f, 1.0f);
		private Vector3 startPosition;
		private Vector3 endPosition;
		public float duration = 4.0f;
	
		void Start ()
		{
				startPosition = transform.position;
				endPosition = new Vector3 (startPosition.x + 25.0f, startPosition.y, startPosition.z);		
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.localScale = Vector3.Lerp (originalScale, targetScale, Time.time / duration);
				transform.position = Vector3.Lerp (startPosition, endPosition, Time.time / duration);
	
		}
}
