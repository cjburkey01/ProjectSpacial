using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour {
	
	public float moveSpeed = 25.0f;
	public float moveSmoothing = 0.1f;
	public float zoomSpeed = 15.0f;
	public float zoomSmoothing = 0.1f;
	public float speedToSizeRatio = 0.1f;

	public Vector2 zoomLimits = new Vector2(3.0f, 40.0f);

	private Camera cam;
	private float goalSize;
	private float sizeSmoothVel;
	private Vector3 goalPos;
	private Vector3 posSmoothVel;
	private Vector4 world;
	
	private Vector3 dragOrigin = Vector3.zero;

	void Awake() {
		if (ReferenceEquals(cam, null)) {
			cam = GetComponent<Camera>();
			if (ReferenceEquals(cam, null)) {
				Debug.LogError("Failed to locate Camera on CameraMove");
			}
		}
		goalPos = transform.position;
		goalSize = cam.orthographicSize;

		float maxX = WorldHandler.Main.hexSize * (3.0f / 2.0f) * WorldHandler.Main.GetMaxX();
		float maxY = WorldHandler.Main.hexSize * HexHandler.SQRT3 * WorldHandler.Main.GetMaxY();
		world = new Vector4(0.0f, 0.0f, maxX, maxY);
	}

	void Update() {
		// Keyboard Input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		input.Normalize();
		input *= Time.deltaTime * moveSpeed * (speedToSizeRatio * cam.orthographicSize);
		goalPos.x += input.x;
		goalPos.y += input.y;

		// MouseDrag input

		if (Input.GetMouseButton(2)) {
			input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
			goalPos -= new Vector3(input.x, input.y, 0.0f) * Time.deltaTime * cam.orthographicSize * 2 * cam.aspect;
		}

		// MouseWheel Input
		goalSize -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
		goalSize = Mathf.Clamp(goalSize, zoomLimits.x, zoomLimits.y);

		goalPos.x = Mathf.Clamp(goalPos.x, world.x, world.z);
		goalPos.y = Mathf.Clamp(goalPos.y, world.y, world.w);

		transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref posSmoothVel, moveSmoothing);
		cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, goalSize, ref sizeSmoothVel, zoomSmoothing);
	}

	private Vector3 ZoomToPoint(Vector3 point, float amt, Vector3 goalPos) {
		return ((point - goalPos) / cam.orthographicSize) * amt;
	}

}