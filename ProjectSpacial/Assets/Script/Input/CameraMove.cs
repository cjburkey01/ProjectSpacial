using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour {
	
	public float moveSpeed = 25.0f;
	public float moveSmoothing = 0.1f;
	public float zoomSpeed = 15.0f;
	public float zoomSmoothing = 0.1f;
	public float speedToSizeRatio = 0.1f;

	public Vector2 zoomLimits = new Vector2(5.0f, 50.0f);

	private Camera cam;
	private float goalSize;
	private float sizeSmoothVel;
	private Vector3 goalPos;
	private Vector3 posSmoothVel;

	void Awake() {
		if (ReferenceEquals(cam, null)) {
			cam = GetComponent<Camera>();
			if (ReferenceEquals(cam, null)) {
				Debug.LogError("Failed to locate Camera on CameraMove");
			}
		}
		goalPos = transform.position;
		goalSize = cam.orthographicSize;
	}

	void Update() {
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		input.Normalize();
		input *= Time.deltaTime * moveSpeed * (speedToSizeRatio * cam.orthographicSize);
		goalPos.x += input.x;
		goalPos.y += input.y;

		goalSize -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
		goalSize = Mathf.Clamp(goalSize, zoomLimits.x, zoomLimits.y);

		transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref posSmoothVel, moveSmoothing);
		cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, goalSize, ref sizeSmoothVel, zoomSmoothing);
	}

}