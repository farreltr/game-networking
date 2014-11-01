using UnityEngine;
using System.Collections;

public class BlueController : PlayerController
{
		Vector2 startTileCoordinate = new Vector2 (GameController.boardSizeX, GameController.boardSizeY);
		public Sprite respawnSprite;

		// Use this for initialization
		void Start ()
		{
				name = "blue";
				respawnPosition = new Vector3 (850.0f, 910.0f, 0);
				startDirection = new Vector2 (0.0f, -1.0f);
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
				return "green";
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
								if ((isEqual (rotation, ZERO) || isEqual (rotation, NINETY))) {
										return true;
								}
								break;

						}
				case Tile.TileType.TJunction:
						{
								if ((!isEqual (rotation, ZERO))) {
										return true;
								}
								break;

						}
				case Tile.TileType.Straight:
						{
								if ((isEqual (rotation, NINETY) || isEqual (rotation, TWO_SEVENTY))) {
										return true;
								}
								break;

						}
				}
				return false;

		}
	
}
