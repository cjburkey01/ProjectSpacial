using UnityEngine;

public class Hex {

	public readonly WorldHandler worldHandler;
	public readonly OffsetHex position;
	public readonly Star star;

	public bool HasStar {
		get {
			return !ReferenceEquals(star, null) && star.valid;
		}
	}

	public Hex(WorldHandler worldHandler, OffsetHex position, bool spawnStar, Vector2Int planets) {
		this.worldHandler = worldHandler;
		this.position = position;
		star = (spawnStar) ? new Star("xyzzyx", this, Random.Range(planets.x, planets.y + 1)) : null;
	}

}