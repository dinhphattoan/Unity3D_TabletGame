using UnityEngine;

public class CameraMenuManager : MonoBehaviour
{
    public Transform mapTransform;
    [Header("Camera Attributes")]
    public float rotationSpeed;
    public float distance = 10.0f;
    public Vector3 cameraOffSet = Vector3.zero;

    void Start()
    {
        
    }
    void HandleRotation()
    {
        this.transform.RotateAround(mapTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
    void Update()
    {
        HandleRotation();
        this.transform.LookAt(mapTransform);
    }
}
