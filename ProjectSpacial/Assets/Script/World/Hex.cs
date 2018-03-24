using UnityEngine;

public class Hex {

	public readonly WorldHandler worldHandler;
	public readonly OffsetHex position;
	public readonly Vector2 starPos;

	public Hex(WorldHandler worldHandler, OffsetHex position) : this(worldHandler, position, position.GetRandomPoint(worldHandler.hexSize)) {
	}

	public Hex(WorldHandler worldHandler, OffsetHex position, Vector2 starPos) {
		this.worldHandler = worldHandler;
		this.position = position;
		this.starPos = starPos;
	}

}