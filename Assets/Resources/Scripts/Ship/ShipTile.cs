using UnityEngine;
using System.Collections;

public class ShipTile : MonoBehaviour {
	
	public enum Rotation {
		Up = 0,
		Down = 180,
		Left = 270,
		Right = 90,
	}
	
	
	public Vector2 Size;
	public GameObject GORef; //self-reference
	
	public bool IsSmall () {
		if(this.Size == new Vector2(1,1)) return true;
		return false;
	}
	
	
	
	
}
