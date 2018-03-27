using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexChunk : MonoBehaviour {

	public Vector2Int chunkPos;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private Mesh mesh;
	private int chunkSize;
	private float hexSize;

	void Awake() {
		if (ReferenceEquals(meshFilter, null)) {
			meshFilter = GetComponent<MeshFilter>();
			if (ReferenceEquals(meshFilter, null)) {
				Debug.LogError("Failed to locate MeshFilter on WorldHandler");
			}
		}
		if (ReferenceEquals(meshRenderer, null)) {
			meshRenderer = GetComponent<MeshRenderer>();
			if (ReferenceEquals(meshRenderer, null)) {
				Debug.LogError("Failed to locate MeshRenderer on WorldHandler");
			}
		}
		if (ReferenceEquals(mesh, null)) {
			mesh = new Mesh() {
				name = "WorldHandlerMesh"
			};
			meshFilter.mesh = mesh;
			meshFilter.sharedMesh = mesh;
		}
		chunkSize = WorldHandler.Main.chunkSize;
		hexSize = WorldHandler.Main.hexSize;
	}

	public void RenderChunk() {
		List<Vector3> verts = new List<Vector3>();
		List<int> hexInds = new List<int>();
		List<int> starInds = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		bool star = false;

		for (int q = 0; q < chunkSize; q++) {
			for (int r = 0; r < chunkSize; r++) {
				RenderHelper.AddBorderedHex(verts, hexInds, uvs, hexSize, 0.05f, new OffsetHex(q + chunkSize * chunkPos.x, r + chunkSize * chunkPos.y));
				Vector2Int chunkOffset = new Vector2Int(chunkPos.x * WorldHandler.Main.chunkSize, chunkPos.y * WorldHandler.Main.chunkSize);
				Hex hex = WorldHandler.Main.HexHandler.GetHex(chunkOffset.x + q, chunkOffset.y + r);
				if (hex.HasStar) {
					star = true;
					RenderHelper.AddFilledCircle(verts, starInds, uvs, 10, WorldHandler.Main.starSize / 2, hex.star.worldPos, -0.01f);
				}
			}
		}

		mesh.Clear();
		mesh.SetVertices(verts);
		mesh.SetTriangles(hexInds.ToArray(), 0);
		if (star) {
			mesh.subMeshCount = 2;
			mesh.SetTriangles(starInds.ToArray(), 1);
		}
		if (!star) {
			meshRenderer.materials[1] = null;
		}
		mesh.SetUVs(0, uvs);
	}

}