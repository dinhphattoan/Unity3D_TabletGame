using UnityEngine;

public class CameraMenuManager : MonoBehaviour
{
    public Transform mapTransform;
    [Header("Camera Attributes")]
    public float rotationSpeed;
    public float distance = 10.0f;
    public Vector3 cameraOffSet = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //Make camera travel around the map globaly
    void HandleRotation()
    {
        this.transform.RotateAround(mapTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        this.transform.LookAt(mapTransform);
    }
}
