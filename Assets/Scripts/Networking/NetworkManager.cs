using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
		private const string typeName = "Dungeon Crawl";
		private const string gameName = "Blue Room";
	
		private bool isRefreshingHostList = false;
		private HostData[] hostList;
	
		public GameObject[] playerPrefabs;
		public GameObject[] tilePrefabs;
		public GameObject gameController;
		public GameObject playerInventory;
		
		
	
		void OnGUI ()
		{
				if (!Network.isClient && !Network.isServer) {
						if (GUI.Button (new Rect (100, 100, 250, 100), "Start Server"))
								StartServer ();
			
						if (GUI.Button (new Rect (100, 250, 250, 100), "Refresh Hosts"))
								RefreshHostList ();
			
						if (hostList != null) {
								for (int i = 0; i < hostList.Length; i++) {
										if (GUI.Button (new Rect (400, 100 + (110 * i), 300, 100), hostList [i].gameName))
												JoinServer (hostList [i]);
								}
						}
				}
		}
	
		private void StartServer ()
		{
				//MasterServer.ipAddress = "127.0.0.1";
				Network.InitializeServer (4, 25000, !Network.HavePublicAddress ());
				MasterServer.RegisterHost (typeName, gameName);
		}
	
		void OnServerInitialized ()
		{
				SpawnBoard ();
				//SpawnInventory ();
				//SpawnPlayer (0);	
				//SpawnInventory ();
		}
	
	
		void Update ()
		{
				if (isRefreshingHostList && MasterServer.PollHostList ().Length > 0) {
						isRefreshingHostList = false;
						hostList = MasterServer.PollHostList ();
				}
		}
	
		private void RefreshHostList ()
		{
				if (!isRefreshingHostList) {
						isRefreshingHostList = true;
						MasterServer.RequestHostList (typeName);
				}
		}
	
	
		private void JoinServer (HostData hostData)
		{
				Network.Connect (hostData);
		}
	
		void OnConnectedToServer ()
		{
				//SpawnPlayer (int.Parse (Network.player.ToString ()));
				//networkView.RPC ("SpawnInventory", Network.player);
				SpawnInventory ();
		}

		private void SpawnBoard ()
		{
				Network.Instantiate (gameController, Vector3.up * 5, Quaternion.identity, 0);
				BuildBoard (42);					
				
		}

	
		private void SpawnPlayer (int index)
		{
				NetworkViewID viewID = Network.AllocateViewID ();
//		Network.Instantiate(playerPrefab[index], spawnPoint[index].transform.position, spawnPoint[index].transform.rotation, 0);

				GameObject clone = Network.Instantiate (playerPrefabs [index], Vector3.up * 5, Quaternion.identity, 0) as GameObject;
				clone.GetComponent<NetworkView> ().viewID = viewID;
		}

		void OnDisconnectedFromServer (NetworkDisconnection info)
		{
				Debug.Log ("Disconnected from server: " + info);
		}

		void OnPlayerDisconnected (NetworkPlayer networkPlayer)
		{
				int id = int.Parse (networkPlayer.ToString ());
				Network.RemoveRPCs (networkPlayer);
//				Network.Destroy (playerPrefabs [id]);
//				if (id == 0) {
//						Network.DestroyPlayerObjects (networkPlayer);
//						Debug.Log ("Server disconnected");
//				}

				Debug.Log ("Player disconnected");
		}
	
		private void SpawnInventory ()
		{
				Instantiate (playerInventory, Vector3.up * 5, Quaternion.identity);
		}

		public void BuildBoard (int seed)
		{
				Random.seed = seed;
				int i = 0;
				for (int x=0; x < GameController.boardSizeX; x++) {
						for (int y=0; y < GameController.boardSizeY; y++) {
								Vector3 position = new Vector3 (TileMap.tileSize * (1.5f + x), TileMap.tileSize * (1.5f + y), 0.0f);
								GameObject tilePrefab = tilePrefabs [Random.Range (0, tilePrefabs.Length)];
								Quaternion rotation = Quaternion.Euler (0, 0, 90 * (Random.Range (0, 4)));								
								GameObject tileObject = Network.Instantiate (tilePrefab, position, rotation, 0) as GameObject;
								tileObject.transform.GetComponent<SpriteRenderer> ().sortingLayerName = "Board Tile";
								Tile tile;
								if (tileObject.GetComponent<Tile> () == null) {
										tile = tileObject.AddComponent<Tile> ();
										tile.SetTileType (tilePrefab.GetComponent<Tile> ().type);
								} else {
										tile = tileObject.GetComponent<Tile> ();
								}
								tile.coordinate = new Vector2 (x, y);
								i++;
						}
				}  
		}



}
