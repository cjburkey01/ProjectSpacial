using System.Collections.Generic;
using UnityEngine;

public static class RenderHelper {
	
	public static void AddFilledHex(List<Vector3> verts, List<int> inds, List<Vector2> uvs, float radius, OffsetHex hex) {
		AddFilledHex(verts, inds, uvs, radius, hex.ToPixel(radius));
	}

	public static void AddFilledHex(List<Vector3> verts, List<int> inds, List<Vector2> uvs, float radius, Vector2 center) {
		AddFilledCircle(verts, inds, uvs, 6, radius, center, 0.0f);
	}

	public static void AddBorderedHex(List<Vector3> verts, List<int> inds, List<Vector2> uvs, float radius, float borderSize, OffsetHex hex) {
		AddBorderedHex(verts, inds, uvs, radius, borderSize, hex.ToPixel(radius));
	}

	public static void AddBorderedHex(List<Vector3> verts, List<int> inds, List<Vector2> uvs, float radius, float borderSize, Vector2 center) {
		AddBorderedCircle(verts, inds, uvs, 6, radius, borderSize, center, 0.0f);
	}

	public static void AddFilledCircle(List<Vector3> verts, List<int> inds, List<Vector2> uvs, int lod, float radius, Vector2 center, float z) {
		int startInd = verts.Count;
		verts.Add(center);
		for (int i = 0; i < lod; i ++) {
			verts.Add(GetCirclePoint(center, radius, i, lod, z));
			inds.Add(startInd);
			inds.Add(startInd + (i + 1) % lod + 1);
			inds.Add(startInd + i + 1);
		}
	}

	public static void AddBorderedCircle(List<Vector3> verts, List<int> inds, List<Vector2> uvs, int lod, float radius, float borderSize, Vector2 center, float z) {
		int startInd = verts.Count;
		for (int i = 0; i < lod; i++) {
			verts.Add(GetCirclePoint(center, radius, i, lod, z));
		}
		for (int i = 0; i < lod; i++) {
			verts.Add(GetCirclePoint(center, radius - borderSize, i, lod, z));
		}
		for (int i = 0; i < lod; i++) {
			inds.Add(startInd + i);
			inds.Add(startInd + i + lod);
			inds.Add(startInd + (i + 1) % lod);

			inds.Add(startInd + i + lod);
			inds.Add(startInd + ((i + 1) % lod) + lod);
			inds.Add(startInd + (i + 1) % lod);
		}
	}

	public static Vector3 GetCirclePoint(Vector2 center, float radius, int corner, int lod, float z) {
		return new Vector3(center.x + radius * Mathf.Cos((Mathf.PI / 180) * ((360.0f / lod) * corner)), center.y + radius * Mathf.Sin((Mathf.PI / 180) * ((360.0f / lod) * corner)), z);
	}

}