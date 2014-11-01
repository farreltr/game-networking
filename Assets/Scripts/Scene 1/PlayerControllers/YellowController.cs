using UnityEngine;
using System.Collections;

public class YellowController : PlayerController
{
		Vector2 startTileCoordinate = new Vector2 (1.0f, GameController.boardSizeY);
		public Sprite respawnSprite;
	
		// Use this for initialization
		void Start ()
		{
				name = "yellow";
				startDirection = new Vector2 (1.0f, 0.0f);
				respawnPosition = new Vector3 (90.0f, 850.0f, 0);	
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
				return "red";
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
								if ((isEqual (rotation, NINETY) || isEqual (rotation, ONE_EIGHTY))) {
										return true;
								}
								break;
			
						}
				case Tile.TileType.TJunction:
						{
								if ((!isEqual (rotation, NINETY))) {
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
