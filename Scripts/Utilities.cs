using UnityEngine;

public static class Utilities
{
    private static Camera _mainCamera;
    
    public static Vector3 GetMouseWorldPosition()
    {
        if (!_mainCamera) _mainCamera = Camera.main;
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        return mouseWorldPosition;
    }

    public static Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }

    public static float GetAngleDegreesFromVector(Vector3 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }
    
    public static float GetAngleRadiansFromVector(Vector3 vector)
    {
        return Mathf.Atan2(vector.y, vector.x);
    }
}
