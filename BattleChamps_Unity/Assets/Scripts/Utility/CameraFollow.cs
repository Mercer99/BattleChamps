using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    public List<Transform> targetPositions = new List<Transform>();

    public Vector3 offset = new Vector3(0, 25, -25);
    public float smoothTime = 0.2f;

    public float minZoom = 60f;
    public float maxZoom = 10f;
    public float zoomLimit = 15f;

    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targetPositions.Count == 0)
        { return; }

        cameraMove();
        cameraZoom();
    }

    void cameraMove()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void cameraZoom()
    {
        float greatestDistance;

        if (GetGreatestDistanceX() <= GetGreatestDistanceZ())
        { greatestDistance = GetGreatestDistanceZ(); }
        else { greatestDistance = GetGreatestDistanceX(); }

        float newZoom = Mathf.Lerp(maxZoom, minZoom, greatestDistance / zoomLimit);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestDistanceX()
    {
        var bounds = new Bounds(targetPositions[0].position, Vector3.zero);
        for (int i = 0; i < targetPositions.Count; i++)
        {
            bounds.Encapsulate(targetPositions[i].position);
        }

        return bounds.size.x;
    }
    float GetGreatestDistanceZ()
    {
        var bounds = new Bounds(targetPositions[0].position, Vector3.zero);
        for (int i = 0; i < targetPositions.Count; i++)
        {
            bounds.Encapsulate(targetPositions[i].position);
        }

        return bounds.size.z;
    }
    Vector3 GetCenterPoint()
    {
        if (targetPositions.Count == 0)
        { return targetPositions[0].position; }

        var bounds = new Bounds(targetPositions[0].position, Vector3.zero);
        for (int i = 0; i < targetPositions.Count; i++)
        {
            bounds.Encapsulate(targetPositions[i].position);
        }

        return bounds.center;
    }
}
