﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SelectionHex : MonoBehaviour {

	public static SelectionHex Main { private set; get; }

	public float smoothing = 0.1f;
	public OffsetHex Current { private set; get; }
	public OffsetHex SelectedHex { private set; get; }

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private Mesh mesh;
	private OffsetHex prevHex;
	private Vector3 goalPos;
	private Vector3 smoothVel;

	void Awake() {
		Main = this;
		if (ReferenceEquals(meshFilter, null)) {
			meshFilter = GetComponent<MeshFilter>();
			if (ReferenceEquals(meshFilter, null)) {
				Debug.LogError("Failed to locate MeshFilter on SingleHexRender");
			}
		}
		if (ReferenceEquals(meshRenderer, null)) {
			meshRenderer = GetComponent<MeshRenderer>();
			if (ReferenceEquals(meshRenderer, null)) {
				Debug.LogError("Failed to locate MeshRenderer on SingleHexRender");
			}
		}
		if (ReferenceEquals(mesh, null)) {
			mesh = new Mesh() {
				name = "SingleHexRenderMesh"
			};
			meshFilter.mesh = mesh;
			meshFilter.sharedMesh = mesh;
		}
		Current = new OffsetHex(0, 0);
		transform.position = new Vector3(0.0f, 0.0f, -1.0f);
		goalPos = transform.position;
		Render();
	}

	void Update() {
		if (!MouseHandler.HexMousePosition.Equals(prevHex)) {
			prevHex = MouseHandler.HexMousePosition;
			int col = Mathf.Clamp(MouseHandler.HexMousePosition.col, 0, WorldHandler.Main.GetMaxX());
			int row = Mathf.Clamp(MouseHandler.HexMousePosition.row, 0, WorldHandler.Main.GetMaxY());
			Vector2 s = (Current = new OffsetHex(col, row)).ToPixel(WorldHandler.Main.hexSize);
			goalPos = new Vector3(s.x, s.y, -1.0f);
		}
		transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref smoothVel, smoothing);
		if (Input.GetMouseButtonDown(0)) {
			SelectedHex = Current;
		} else if (Input.GetMouseButtonDown(0)) {
			SelectedHex = null;
		}
	}

	private void Render() {
		List<Vector3> verts = new List<Vector3>();
		List<int> inds = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		RenderHelper.AddBorderedHex(verts, inds, uvs, WorldHandler.Main.hexSize, 0.05f, new OffsetHex(0, 0));

		mesh.Clear();
		mesh.SetVertices(verts);
		mesh.SetIndices(inds.ToArray(), MeshTopology.Triangles, 0);
		mesh.SetUVs(0, uvs);
	}

}