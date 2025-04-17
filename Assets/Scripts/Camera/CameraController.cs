using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private Transform clampMin;

    [SerializeField]
    private Transform clampMax;

    private Camera cam;

    private float halfWidth, halfHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = PlayerController.instance.transform;

        SetupCamera();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();

        ClampPosition();
    }

    private void MoveToTarget()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, clampMin.transform.position.x + halfWidth, clampMax.position.x - halfWidth);

		clampedPosition.y = Mathf.Clamp(clampedPosition.y, clampMin.transform.position.y + halfHeight, clampMax.position.y - halfHeight);

        transform.position = clampedPosition;
	}

    private void SetupCamera()
    {
		clampMin.SetParent(null);

		clampMax.SetParent(null);

		cam = GetComponent<Camera>();

		halfHeight = cam.orthographicSize;

		halfWidth = cam.orthographicSize * cam.aspect;
	}
}
