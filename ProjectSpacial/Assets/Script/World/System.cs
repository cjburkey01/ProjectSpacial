using System.Collections.Generic;
using UnityEngine;

public class Star {

	public string name;
	public readonly Hex hex;
	public readonly Vector2 worldPos;
	public readonly bool valid;
	public readonly List<Planet> planets = new List<Planet>();

	public Star(string name, Hex hex, int planets) {
		this.name = name;
		this.hex = hex;
		valid = hex.position.GetRandomPoint(hex.worldHandler.hexSize, hex.worldHandler.starSize + hex.worldHandler.starPadding, out worldPos);
		for (int i = 0; i < planets; i ++) {
			this.planets.Add(new Planet(name + " " + i));
		}
	}

}

public class Planet {

	public static readonly int minTile = 1;
	public static readonly int maxTile = 5;

	public string name;
	private readonly PlanetTile[,] tiles;
	public int TilesX { private set; get; }
	public int TilesY { private set; get; }

	public Planet(string name) {
		TilesX = Random.Range(minTile, maxTile + 1);
		TilesY = Random.Range(minTile, maxTile + 1);
		tiles = new PlanetTile[TilesX, TilesY];
		for (int x = 0; x < TilesX; x ++) {
			for (int y = 0; y < TilesY; y ++) {
				tiles[x, y] = new PlanetTile();
			}
		}
	}

	public PlanetTile GetTile(int x, int y) {
		if (x < 0 || x >= TilesX || y < 0 || y >= TilesY) {
			return null;
		}
		return tiles[x, y];
	}

}

public class PlanetTile {

	// TODO: ADD STUFF

}