using UnityEngine;

public class WorldHandler : MonoBehaviour {

	public static WorldHandler Main { private set; get; }
	public HexHandler HexHandler { private set; get; }

	public HexChunk[,] chunks;
	public GameObject chunkPrefab;
	public int chunkSize = 16;  // 16*16 by 16*16 sized map
	public int chunksX = 5;
	public int chunksY = 5;
	public float hexSize = 1.0f;
	public float starSize = 0.1f;
	public float starPadding = 0.3f;
	public float starChance = 1.0f / 3.0f;
	public Vector2Int minMaxPlanets;

	public WorldHandler() {
		Main = this;
	}

	void Awake() {
		if (ReferenceEquals(HexHandler, null)) {
			HexHandler = new HexHandler(this, chunkSize * chunksX, chunkSize * chunksY, starChance, starSize, minMaxPlanets);
		}
		GenerateMap();
		RenderMap();
	}

	private void GenerateMap() {
		if (chunkPrefab == null) {
			Debug.LogError("ChunkPrefab is null for WorldHandler");
			return;
		}
		foreach (Transform t in transform) {
			Destroy(t.gameObject);
		}
		chunks = new HexChunk[chunksX, chunksY];
		for (int x = 0; x < chunksX; x ++) {
			for (int y = 0; y < chunksY; y ++) {
				GameObject obj = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity);
				HexChunk chunk = obj.GetComponent<HexChunk>();
				if (ReferenceEquals(chunk, null)) {
					Debug.LogError("HexChunk not found on ChunkPrefab");
					return;
				}
				obj.transform.name = "Chunk (" + x + "x" + y + ")";
				obj.transform.parent = transform;
				chunk.chunkPos = new Vector2Int(x, y);
				chunks[x, y] = chunk;
			}
		}
	}

	public void RenderMap() {
		foreach (Transform t in transform) {
			HexChunk chunk = t.gameObject.GetComponent<HexChunk>();
			if (ReferenceEquals(chunk, null)) {
				Debug.LogError("HexChunk not found on chunk in world");
				continue;
			}
			chunk.RenderChunk();
		}
	}

	public void RenderContainingChunk(OffsetHex hex) {
		int chunkX = Mathf.FloorToInt((float) hex.col / chunkSize);
		int chunkY = Mathf.FloorToInt((float) hex.row / chunkSize);
		if (chunkX < 0 || chunkX >= chunksX || chunkY < 0 || chunkY >= chunksY) {
			Debug.LogError("Failed to render chunk: (" + chunkX + ", " + chunkY + "), it does not exist.");
			return;
		}
		chunks[chunkX, chunkY].RenderChunk();
	}

	public int GetWidth() {
		return chunkSize * chunksX;
	}

	public int GetHeight() {
		return chunkSize * chunksY;
	}

	public int GetMaxX() {
		return GetWidth() - 1;
	}

	public int GetMaxY() {
		return GetHeight() - 1;
	}

}