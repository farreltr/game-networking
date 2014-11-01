using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
		private Inventory inventory;
		private bool restart;
		private bool gameOver;
		private static string EMPTY_STRING = "";
		private static int nextPlayer = 1;
		public int numberOfPLayers = 1;
		public int currentCoordinateX;
		public int currentCoordinateY;
		public static int boardSizeX = TileMap.size_x - 2;
		public static int boardSizeY = TileMap.size_y - 2;
		//public Tile[,] boardTiles = new Tile[8, 8];
		public GameObject[] tilePrefabs;
		Vector3 mousePosition = Vector3.zero;
		public bool isTileMoving = false;
		public bool isCPU = false;
		public string gameOverText = "";

		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				gameOver = false;
				restart = false;
				inventory = GameObject.FindObjectOfType<Inventory> ();
				BuildBoard (Mathf.FloorToInt (Time.time));

		}
	
		void Update ()
		{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hitInfo;
				inventory = GameObject.FindObjectOfType<Inventory> ();
		
				if (TileMap.tileMap != null && TileMap.tileMap.collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
						mousePosition = hitInfo.point;
						currentCoordinateX = Mathf.FloorToInt (hitInfo.point.x / TileMap.tileSize);
						currentCoordinateY = Mathf.FloorToInt (hitInfo.point.y / TileMap.tileSize);
						string tileIdx = EMPTY_STRING;

						if (isTileDropped ()) {
								bool done = HandleTile ();		
								if (done) {
										PlayerManager.playerManager.LoadNextLevel ();	
								}
																			
										
						}
				}
		}

		private bool isTileDropped ()
		{
				return inventory != null && inventory.e != null && inventory.e.type == EventType.mouseUp && inventory.draggingTile;
		}

		private bool HandleTile ()
		{
				
				TilePosition tilePosition = GetPositionFromCoordinate ();
				if (!tilePosition.Equals (TilePosition.NONE)) {
						InstantiateDraggedTile (tilePosition);
						ShiftBoardTiles (tilePosition);
						ShiftPlayers (tilePosition);	
						return true;
				}
				return false;
		
		}

		public void processCPUPlayer ()
		{
				switch (Random.Range (0, 4)) {
				case 0:
						{
								currentCoordinateX = 0;
								currentCoordinateY = Random.Range (0, 9);
								break;
						}

				case 1:
						{
								currentCoordinateX = Random.Range (0, 9);
								currentCoordinateY = 0;
								break;
						}
				case 2:
						{
								currentCoordinateX = TileMap.size_x - 1;
								currentCoordinateY = Random.Range (0, 9);
								break;
						}
				case 3:
						{
								currentCoordinateX = Random.Range (0, 9);
								currentCoordinateY = TileMap.size_y - 1;
								break;
						}
				
				}
				StartCoroutine ("Wait");


		}
	
		IEnumerator Wait ()
		{
				yield return new WaitForSeconds (3);
				HandleTile ();	
				isCPU = false;
				inventory.isCPU = false;
				inventory.isCPUStarted = false;
				PlayerManager.playerManager.LoadNextLevel ();	
		}

		void ShiftPlayers (TilePosition tilePosition)
		{
				PlayerController[] players = GameObject.FindObjectsOfType<PlayerController> ();
				foreach (PlayerController player in players) {
						switch (tilePosition) {
						case TilePosition.LEFT:
								player.ShiftRight (currentCoordinateX, currentCoordinateY);
								break;
						case TilePosition.RIGHT:
								player.ShiftLeft (currentCoordinateX, currentCoordinateY);
								break;
						case TilePosition.TOP:
								player.ShiftDown (currentCoordinateX, currentCoordinateY);
								break;
						case TilePosition.BOTTOM:
								player.ShiftUp (currentCoordinateX, currentCoordinateY);
								break;
						}

				}
		}

		private Vector2 GetNewCoordinate (TilePosition tilePosition)
		{
				switch (tilePosition) {
				case TilePosition.LEFT:
						return new Vector2 (currentCoordinateX, currentCoordinateY - 1);
				case TilePosition.RIGHT:
						return new Vector2 (currentCoordinateX - 2, currentCoordinateY - 1);
				case TilePosition.TOP:
						return new Vector2 (currentCoordinateX - 1, currentCoordinateY - 2);
				case TilePosition.BOTTOM:
						return new Vector2 (currentCoordinateX - 1, currentCoordinateY);
				default:
						return Vector2.zero;
				}


		}

		private Vector3 GetPositionForNewTile (TilePosition tilePosition)
		{
				Vector3 position = Vector3.zero;
				switch (tilePosition) {
				case TilePosition.LEFT:
						position.x = currentCoordinateX + 1.5f;
						position.y = currentCoordinateY + 0.5f;
						break;
				case TilePosition.RIGHT: 
						position.x = currentCoordinateX - 0.5f;
						position.y = currentCoordinateY + 0.5f;
						break;
				case TilePosition.TOP:
						position.x = currentCoordinateX + 0.5f;
						position.y = currentCoordinateY - 0.5f;
						break;
				case TilePosition.BOTTOM:
						position.x = currentCoordinateX + 0.5f;
						position.y = currentCoordinateY + 1.5f;
						break;
				default:
						break;
				}
				position *= TileMap.tileSize;
				position.z = 0.5f;
				return position;

		}

		private Tile GetTileFromPosition (TilePosition tilePosition)
		{
				switch (tilePosition) {
				case TilePosition.LEFT:
						return GetTileAtCoordinate (new Vector2 (currentCoordinateX + boardSizeX - 1, currentCoordinateY - 1));		
				case TilePosition.RIGHT: 
						return GameObject.FindGameObjectWithTag (string.Concat (currentCoordinateX - boardSizeX - 1, currentCoordinateY - 1)).GetComponent<Tile> ();
				case TilePosition.TOP:
						return GetTileAtCoordinate (new Vector2 (currentCoordinateX - 1, currentCoordinateY + boardSizeY - 1));	
				case TilePosition.BOTTOM:
						return GetTileAtCoordinate (new Vector2 (currentCoordinateX - 1, currentCoordinateY - boardSizeY - 1));		
				default:
						return new Tile ();
						break;
				}
		}
	
		private void ShiftBoardTiles (TilePosition tilePosition)
		{
				switch (tilePosition) {
				case TilePosition.LEFT:
						CloneAndDestroy (GetTileAtCoordinate (new Vector2 (boardSizeX, currentCoordinateY)));
						for (int x=boardSizeX; x>-1; x--) {
								Tile tile = GetTileAtCoordinate (new Vector2 (x, currentCoordinateY));
								if (tile != null) {
										tile.shiftRight ();
								}
								
						}
						break;
				case TilePosition.RIGHT: 
						CloneAndDestroy (GetTileAtCoordinate (new Vector2 (1, currentCoordinateY)));	
						for (int x=1; x<boardSizeX+1; x++) {
								Tile tile = GetTileAtCoordinate (new Vector2 (x, currentCoordinateY));
								if (tile != null) {
										tile.shiftLeft ();
								}
						}
						break;
				case TilePosition.TOP:
						CloneAndDestroy (GetTileAtCoordinate (new Vector2 (currentCoordinateX, 1)));	
						for (int y=1; y<boardSizeY+1; y++) {
								Tile tile = GetTileAtCoordinate (new Vector2 (currentCoordinateX, y));
								if (tile != null) {
										tile.shiftDown ();
								}			
						}
						break;								
				case TilePosition.BOTTOM:
						CloneAndDestroy (GetTileAtCoordinate (new Vector2 (currentCoordinateX, boardSizeY)));	
						for (int y=boardSizeY; y>-1; y--) {	
								Tile tile = GetTileAtCoordinate (new Vector2 (currentCoordinateX, y));
								if (tile != null) {
										tile.shiftUp ();	
								}							
						}
						break;
				}
		}
		
		private void CloneAndDestroy (Tile tile)
		{
				if (tile != null) {
						inventory.inventory [inventory.prevIdx] = CloneObject (tile.gameObject).GetComponent<Tile> ();
						tile.setToDestroy = true;
				}
		}

		private GameObject CloneObject (GameObject gameObject)
		{
				GameObject prefab = Resources.Load<GameObject> ("Tiles/Prefabs/" + GetFormattedName (gameObject));	
				prefab.transform.rotation = gameObject.transform.rotation;
				return prefab;

		}

		public Tile GetTileAtCoordinate (Vector2 coordinate)
		{
				Vector3 position = new Vector3 (TileMap.tileSize * (0.5f + coordinate.x), TileMap.tileSize * (0.5f + coordinate.y), 0.0f);
				GameObject[] boardTiles = GameObject.FindGameObjectsWithTag ("Board Tile");
				foreach (GameObject boardTile in boardTiles) {
						if ((boardTile.transform.position - position).magnitude < 2) {
								return boardTile.GetComponent<Tile> ();
						}
				}
				return null;

		}
	

		TilePosition GetPositionFromCoordinate ()
		{
				if (currentCoordinateX == TileMap.size_x - 1 && currentCoordinateY != 0 && currentCoordinateY != TileMap.size_y - 1) {
						return TilePosition.RIGHT;
				} else if (currentCoordinateX == 0 && currentCoordinateY != 0 && currentCoordinateY != TileMap.size_y - 1) {
						return TilePosition.LEFT;
				} else if (currentCoordinateY == TileMap.size_y - 1 && currentCoordinateX != 0 && currentCoordinateX != TileMap.size_x - 1) {
						return TilePosition.TOP;
				} else if (currentCoordinateY == 0 && currentCoordinateX != 0 && currentCoordinateX != TileMap.size_x - 1) {
						return TilePosition.BOTTOM;
				} else {
						return TilePosition.NONE;
				}

		}
		
		void InstantiateDraggedTile (TilePosition tilePosition)
		{
				Vector3 position = new Vector3 (TileMap.tileSize * (0.5f + currentCoordinateX), TileMap.tileSize * (0.5f + currentCoordinateY), 0.0f);
				if (inventory.draggedTile != null) {
						GameObject clone = CloneObject (inventory.draggedTile.gameObject);
						GameObject tileObject = Instantiate (clone, position, clone.transform.rotation) as GameObject;
						tileObject.transform.parent = GameObject.FindGameObjectWithTag ("Board").transform;
						tileObject.transform.GetComponent<SpriteRenderer> ().sortingLayerName = "Board Tile";
						inventory.draggingTile = false;
						Tile tile;
						if (tileObject.GetComponent<Tile> () == null) {
								tile = tileObject.AddComponent<Tile> ();
								tile.SetTileType (clone.GetComponent<Tile> ().type);
						} else {
								tile = clone.GetComponent<Tile> ();
						}	
						tile.coordinate = new Vector2 (currentCoordinateX, currentCoordinateY);
						if (tileObject.GetComponent<MoveOnStart> () == null) {
								MoveOnStart moveOnStart = tileObject.AddComponent<MoveOnStart> ();
								NetworkView viewMove = tileObject.AddComponent<NetworkView> ();
								viewMove.observed = moveOnStart;
						} 
						tileObject.GetComponent<MoveOnStart> ().direction = GetMoveDirection (tilePosition);
						foreach (BoxCollider2D collider in tileObject.GetComponents<BoxCollider2D>()) {
								collider.enabled = false;
						}

				}

		}

		public bool IsTileMoving ()
		{
				foreach (Tile tile in GameObject.FindObjectsOfType<Tile>()) {
						if (tile.flag) {
								return true;
						}				
				}
				return false;
		}
	
		string GetMoveDirection (TilePosition tilePosition)
		{
				switch (tilePosition) {
				case TilePosition.LEFT:
						return "right";		
				case TilePosition.RIGHT: 
						return "left";
				case TilePosition.TOP:
						return "down";	
				case TilePosition.BOTTOM:
						return "up";
				default:
						throw new System.NotImplementedException ();
				}
				
		}

		private static string GetFormattedName (GameObject o)
		{
				return o.name.Replace ("(Clone)", "");
		}


		public void GameOver ()
		{
				gameOver = true;
				foreach (GameObject player in GameObject.FindGameObjectsWithTag ("Player")) {
						PlayerController controller = player.GetComponent<PlayerController> ();
						Animator animator = player.GetComponent<Animator> ();	
						if (controller != null) {
//								controller.Stop ();
								if (controller.isWinner) {
										PlayerManager.playerManager.WinScreen (controller.GetName ());
//										Instantiate (Resources.Load<GUITexture> ("End Screens/" + controller.GetName ()));
										//GameObject.FindObjectOfType<Inventory> ().SetDisabled ();	
										PlayerManager pm = GameController.FindObjectOfType<PlayerManager> ();
										Player p = pm.GetPlayerByColour (controller.name);
										gameOverText = p.getPlayerName () + " Wins!";										
								}
						}

//						if (animator != null) {
//								animator.enabled = false;
//						}
						restart = true;
				}

		}

		public void BuildBoard (int seed)
		{
				UnityEngine.Random.seed = Mathf.FloorToInt (Time.time);
				GameObject[] tilePrefabs = Resources.LoadAll<GameObject> ("Tiles/Prefabs/"); 
				int i = 0;
				for (int x=0; x < GameController.boardSizeX; x++) {
						for (int y=0; y < GameController.boardSizeY; y++) {
								Vector3 position = new Vector3 (TileMap.tileSize * (1.5f + x), TileMap.tileSize * (1.5f + y), 0.0f);
								GameObject tilePrefab = tilePrefabs [Random.Range (0, tilePrefabs.Length)];
								tilePrefab.tag = "Board Tile";
								tilePrefab.GetComponent<Tile> ().coordinate = new Vector2 (x, y);
								tilePrefab.transform.localScale = Vector3.one * TileMap.tileSize;
								Quaternion rotation = Quaternion.Euler (0, 0, 90 * (Random.Range (0, 4)));								
								GameObject tileObject = Instantiate (tilePrefab, position, rotation) as GameObject;
								tileObject.transform.parent = GameObject.FindGameObjectWithTag ("Board").transform;
								tileObject.transform.GetComponent<SpriteRenderer> ().sortingLayerName = "Board Tile";
								Tile tile;
								if (tileObject.GetComponent<Tile> () == null) {
										tile = tileObject.AddComponent<Tile> ();
										tile.SetTileType (tilePrefab.GetComponent<Tile> ().type);
								} else {
										tile = tileObject.GetComponent<Tile> ();
								}
								tileObject.tag = "Board Tile";
								tile.coordinate = new Vector2 (x, y);
								i++;
						}
				}  
		}  

		public void OnLevelWasLoaded (int i)
		{
				PlayerManager pm = GameObject.FindObjectOfType<PlayerManager> ();
				isCPU = pm.currentPlayer.isCPU;
		}


		public enum TilePosition
		{
				TOP,
				BOTTOM,
				LEFT,
				RIGHT,
				NONE
		}
	
}
