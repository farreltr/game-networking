using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
		public static Grid grid;
		public GameObject box;
	
		void Awake ()
		{
				if (grid == null) {
						DontDestroyOnLoad (grid);
						grid = this;
				} else if (grid != this) {
						Destroy (gameObject);
				}
		}
	
		// Use this for initialization
		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				box = Resources.Load<GameObject> ("Grid/Box");
				for (int x=0; x < TileMap.size_x; x++) {
						for (int y=0; y < TileMap.size_y; y++) {
								Vector3 position = new Vector3 (x * TileMap.tileSize + (TileMap.tileSize * 0.5f), y * TileMap.tileSize + (TileMap.tileSize * 0.5f), 0);
								SpriteRenderer renderer = box.GetComponent<SpriteRenderer> ();
								renderer.sortingLayerName = "Grid";			
								GameObject boxClone = (GameObject)Instantiate (box, position, box.transform.rotation);
								boxClone.name = "box";
								boxClone.transform.localScale = Vector3.one * TileMap.tileSize;
								boxClone.transform.parent = gameObject.transform;
								boxClone.layer = this.gameObject.layer;
				
						}
				} 	
		}

}