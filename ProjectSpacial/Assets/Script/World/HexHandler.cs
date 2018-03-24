using UnityEngine;

public class HexHandler {

	public static readonly float SQRT3 = Mathf.Sqrt(3.0f);
	
	public readonly int width;
	public readonly int height;

	private Hex[,] grid;

	public HexHandler(WorldHandler worldHandler, int width, int height) {
		this.width = width;
		this.height = height;
		grid = new Hex[width, height];
		for (int q = 0; q < width; q ++) {
			for (int r = 0; r < height; r ++) {
				grid[q, r] = new Hex(worldHandler, new OffsetHex(q, r));
			}
		}
	}

	public Hex GetHex(int q, int r) {
		if (q < 0 || q >= width || r < 0 || r >= height) {
			return null;
		}
		return grid[q, r];
	}

}

public class CubeHex {

	public readonly int x;
	public readonly int y;
	public readonly int z;

	public CubeHex(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public OffsetHex ToOffset() {
		return new OffsetHex(x, z + (x - (x & 1)) / 2);
	}
	
	public override string ToString() {
		return "Cube (" + x + ", " + y + ", " + z + ")";
	}

	public override bool Equals(object obj) {
		var hex = obj as CubeHex;
		return hex != null && x == hex.x && y == hex.y && z == hex.z;
	}
	
	public override int GetHashCode() {
		var hashCode = 373119288;
		hashCode = hashCode * -1521134295 + x.GetHashCode();
		hashCode = hashCode * -1521134295 + y.GetHashCode();
		hashCode = hashCode * -1521134295 + z.GetHashCode();
		return hashCode;
	}

	public static CubeHex CubeRound(float x, float y, float z) {
		int rx = Mathf.RoundToInt(x);
		int ry = Mathf.RoundToInt(y);
		int rz = Mathf.RoundToInt(z);

		float xDiff = Mathf.Abs(rx - x);
		float yDiff = Mathf.Abs(ry - y);
		float zDiff = Mathf.Abs(rz - z);

		if (xDiff > yDiff && xDiff > zDiff) {
			rx = -ry - rz;
		} else if (yDiff > zDiff) {
			ry = -rx - rz;
		} else {
			rz = -rx - ry;
		}

		return new CubeHex(rx, ry, rz);
	}

}

public class OffsetHex {

	public readonly int col;
	public readonly int row;

	public OffsetHex(int col, int row) {
		this.col = col;
		this.row = row;
	}

	public CubeHex ToCube() {
		int x = col - (row - (row & 1)) / 2;
		return new CubeHex(x, -x - row, row);
	}

	public Vector2 ToPixel(float hexSize) {
		return new Vector2(hexSize * 3.0f / 2.0f * col, hexSize * HexHandler.SQRT3 * (row + 0.5f * (col & 1)));
	}

	public bool ContainsPoint(float hexSize, Vector2 point) {
		return FromPixel(hexSize, point.x, point.y).Equals(this);
	}

	public Vector2 GetRandomPoint(float hexSize) {
		Vector2 p = Vector2.zero;
		int tries = 0;
		do {
			Vector2 center = ToPixel(hexSize);
			float h = HexHandler.SQRT3 * hexSize;
			float x = Random.Range(center.x - hexSize, center.x + hexSize);
			float y = Random.Range(center.y - h / 2, center.y + h / 2);
			p = new Vector2(x, y);
			tries ++;
		} while(!ContainsPoint(hexSize, p) && tries < 1000);
		if (tries >= 1000) {
			Debug.LogError("Failed to find point for hex: " + ToString());
			return Vector2.zero;
		}
		return p;
	}

	public override string ToString() {
		return "Offset (" + col + ", " + row + ")";
	}

	public override bool Equals(object obj) {
		var hex = obj as OffsetHex;
		return hex != null && col == hex.col && row == hex.row;
	}

	public override int GetHashCode() {
		var hashCode = -1831622508;
		hashCode = hashCode * -1521134295 + col.GetHashCode();
		hashCode = hashCode * -1521134295 + row.GetHashCode();
		return hashCode;
	}

	public static OffsetHex FromPixel(float hexSize, float x, float y) {
		float q = x * 2.0f / 3.0f / hexSize;
		float r = (-x / 3.0f + HexHandler.SQRT3 / 3.0f * y) / hexSize;
		return CubeHex.CubeRound(q, -q - r, r).ToOffset();
	}
}