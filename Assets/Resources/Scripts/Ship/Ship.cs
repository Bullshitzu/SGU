using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {
	
	public static Ship shipRef;
	ShipTile[,] Tiles;
	
	void Start () {
		shipRef = this;
		//Tiles = new ShipTile[4096,4096];
	}
	
	#region Tiling System
	
	
	
	public static Vector2 gridToWorldPos (Vector2 gridPos) {
		
		gridPos += new Vector2(-2048, -2048);
		
		return gridPos;
	}
	public static Vector2 worldToGridPos (Vector2 worldPos) {
		
		if(worldPos.x < 0) worldPos += new Vector2(-1, 0);
		if(worldPos.y < 0) worldPos += new Vector2(0, -1);
		
		worldPos += new Vector2(2048, 2048);
		worldPos = new Vector2((int)worldPos.x, (int)worldPos.y);
		
		return worldPos;
	}
	#endregion
	
	
}
