using UnityEngine;
using System.Collections.Generic;

public class TileDB : MonoBehaviour
{

		public List<Tile> tiles = new List<Tile> ();

		void Start ()
		{
				Tile[] prefabs = Resources.LoadAll<Tile> ("Tiles/Prefabs/");
				foreach (Tile prefab in prefabs) {
						tiles.Add (prefab);
				}
		}



}

