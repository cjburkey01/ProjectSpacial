using UnityEngine;

public class MouseHandler : MonoBehaviour {
	
	public static Vector2 WorldMousePosition { private set; get; }
	public static Vector2 PrevWorldMousePosition { private set; get; }
	public static OffsetHex HexMousePosition { private set; get; }

	private Plane plane;
	private Ray ray;
	private float dist;
	private Vector3 world;

	void Awake() {
		WorldMousePosition = Vector2.zero;
		PrevWorldMousePosition = Vector2.zero;
		plane = new Plane(Vector3.forward, Vector3.zero);
		HexMousePosition = new OffsetHex(0, 0);
	}

	void Update() {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (plane.Raycast(ray, out dist)) {
			world = ray.GetPoint(dist);
			WorldMousePosition = new Vector2(world.x, world.y);
			if (!WorldMousePosition.Equals(PrevWorldMousePosition)) {
				PrevWorldMousePosition = WorldMousePosition;
				HexMousePosition = OffsetHex.FromPixel(WorldHandler.Main.hexSize, WorldMousePosition.x, WorldMousePosition.y);
			}
		}
	}

}