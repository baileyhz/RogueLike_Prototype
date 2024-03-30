using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTiles : MonoBehaviour
{
	public Tilemap tilemap;
	private void Awake()
	{
		tilemap = GetComponent<Tilemap>();
	}

	public void MakeDot(Vector3 pos)
	{
		Vector3Int cellPosition = tilemap.WorldToCell(pos);
		Debug.Log("DestructibleTiles " + cellPosition);
		tilemap.SetTile(cellPosition,null);
	}
}
