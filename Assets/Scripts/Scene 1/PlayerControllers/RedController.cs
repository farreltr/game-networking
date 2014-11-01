using UnityEngine;
using System.Collections;

public class RedController : PlayerController
{

		Vector2 startTileCoordinate = new Vector2 (GameController.boardSizeX, 1.0f);
		public Sprite respawnSprite;
		// Use this for initialization
		void Start ()
		{
				name = "red";
				respawnPosition = new Vector3 (910.0f, 150.0f, 0);
				startDirection = new Vector2 (-1.0f, 0.0f);
				base.Start ();
		}

		public override string GetName ()
		{
				return name;
		}
		public override Vector3 GetRespawnPosition ()
		{
				return respawnPosition;
		}
		public override string GetRespawnName ()
		{
				return "yellow";
		}

		public override Sprite GetRespawnSprite ()
		{
				return respawnSprite;
		}

		public override bool canMove ()
		{	
				Tile startBlock = GetTileAtCoordinate (startTileCoordinate);
				if (startBlock == null)
						return false;
				float rotation = startBlock.transform.rotation.eulerAngles.z;
				switch (startBlock.type) {
				case Tile.TileType.CrossJunction:
						return true;
				case Tile.TileType.Curve:
						{
								if ((isEqual (rotation, ZERO) || isEqual (rotation, TWO_SEVENTY))) {
										return true;
								}
								break;
			
						}
				case Tile.TileType.TJunction:
						{
								if (!isEqual (rotation, TWO_SEVENTY)) {
										return true;
								}
								break;
			
						}
				case Tile.TileType.Straight:
						{
								if ((isEqual (rotation, ZERO) || isEqual (rotation, ONE_EIGHTY))) {
										return true;
								}
								break;
			
						}
						
		
				}
				return false;
	
	
		}
}