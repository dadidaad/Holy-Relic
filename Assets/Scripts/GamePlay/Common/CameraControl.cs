using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    public SpriteRenderer focusObjectRenderer;
    public float offsetX = 0f;
    // Vertical offset from focus object edges
    public float offsetY = 0f;
    // Camera speed when moving (dragging)
    public float dragSpeed = 2f;

    // Restrictive points for camera moving
    private float maxX, minX, maxY, minY;
    // Camera dragging at now vector
    private float moveX, moveY;
    // Camera component from this gameobject
    private Camera cam;
    // Origin camera aspect ratio
    private float originAspect;

	// Start is called before the first frame update
	Vector3 dragOrigin;

	private readonly Vector3 nullVector = new Vector3(-1000, -1000, -1000);

	void Start()
    {
        cam = GetComponent<Camera>();
        Debug.Assert(focusObjectRenderer && cam, "Wrong initial settings");
        originAspect = cam.aspect;
        // Get restrictive points from focus object's corners
        maxX = focusObjectRenderer.bounds.max.x;
        minX = focusObjectRenderer.bounds.min.x;
        maxY = focusObjectRenderer.bounds.max.y;
        minY = focusObjectRenderer.bounds.min.y;
		dragOrigin = nullVector;
    }

	// Update is called once per frame
	private void Update()
	{

	}
    void LateUpdate()
	{
		
		// Camera aspect ratio is changed
		if (originAspect != cam.aspect)
		{
			originAspect = cam.aspect;
		}
		// Need to move camera horizontally
		if (moveX != 0f)
		{
			bool permit = false;
			// Move to right
			if (moveX > 0f)
			{
				// If restrictive point does not reached
				if (cam.transform.position.x + (cam.orthographicSize * cam.aspect) < maxX - offsetX)
				{
					permit = true;
				}
			}
			// Move to left
			else
			{
				// If restrictive point does not reached
				if (cam.transform.position.x - (cam.orthographicSize * cam.aspect) > minX + offsetX)
				{
					permit = true;
				}
			}
			if (permit == true)
			{
				// Move camera
				transform.Translate(Vector3.right * moveX * dragSpeed, Space.World);
			}
			moveX = 0f;
		}
		// Need to move camera vertically
		if (moveY != 0f)
		{
			bool permit = false;
			// Move up
			if (moveY > 0f)
			{
				// If restrictive point does not reached
				if (cam.transform.position.y + cam.orthographicSize < maxY - offsetY)
				{
					permit = true;
				}
			}
			// Move down
			else
			{
				// If restrictive point does not reached
				if (cam.transform.position.y - cam.orthographicSize > minY + offsetY)
				{
					permit = true;
				}
			}
			if (permit == true)
			{
				// Move camera
				transform.Translate(Vector3.up * moveY * dragSpeed, Space.World);
			}
			moveY = 0f;
		}
	}
	public void MoveX(float distance)
	{
		moveX = distance;
	}

	public void MoveY(float distance)
	{
		moveY = distance;
	}
}
