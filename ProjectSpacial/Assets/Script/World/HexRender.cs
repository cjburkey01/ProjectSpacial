using System.Collections.Generic;
using UnityEngine;

public static class HexRender {
	
	public static void AddHex(List<Vector3> verts, List<int> inds, List<Vector2> uvs, float hexSize, OffsetHex hex) {
		Vector2 center = hex.ToPixel(hexSize);
		int startInd = verts.Count;
		verts.Add(center);
		for (int i = 0; i < 6; i++) {
			verts.Add(GetHexCorner(center, hexSize, i));
			inds.Add(startInd);
			inds.Add(startInd + (i + 1) % 6 + 1);
			inds.Add(startInd + i + 1);
		}
	}
	
	public static Vector3 GetHexCorner(Vector2 center, float hexSize, int corner) {
		return new Vector3(center.x + hexSize * Mathf.Cos((Mathf.PI / 180) * (60 * corner)), center.y + hexSize * Mathf.Sin((Mathf.PI / 180) * (60 * corner)));
	}

}