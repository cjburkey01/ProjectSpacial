using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WorldHandler : MonoBehaviour {

	public static WorldHandler Main { private set; get; }
	
	public int width = 20;
	public int height = 20;
	public float hexSize = 1.0f;
	public HexHandler HexHandler { private set; get; }

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private Mesh mesh;

	public WorldHandler() {
		Main = this;
	}

	void Awake() {
		if (ReferenceEquals(HexHandler, null)) {
			HexHandler = new HexHandler(this, width, height);
		}
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
		RenderMap();
	}

	public void RenderMap() {
		List<Vector3> verts = new List<Vector3>();
		List<int> inds = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		for (int q = 0; q < width; q ++) {
			for (int r = 0; r < height; r ++) {
				HexRender.AddHex(verts, inds, uvs, hexSize, new OffsetHex(q, r));
				Hex h = HexHandler.GetHex(q, r);
			}
		}

		mesh.Clear();
		mesh.SetVertices(verts);
		mesh.SetIndices(inds.ToArray(), MeshTopology.Triangles, 0);
		mesh.SetUVs(0, uvs);
	}

}